using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using SpacetimeDB;

public static partial class Module
{
    [SpacetimeDB.Table(Accessor = "Player", Public = true)]
    public partial struct Player
    {
        [SpacetimeDB.PrimaryKey]
        public Identity Identity;

        [SpacetimeDB.Unique]
        public string SnailName;
    }

    [SpacetimeDB.Table(Accessor = "Winstreak", Public = true)]
    public partial struct WinStreak
    {
        [SpacetimeDB.AutoInc] [SpacetimeDB.PrimaryKey]
        public ulong Id;

        [SpacetimeDB.Index.BTree] public Identity PlayerIdentity;

        public int CurrentWinStreak;
        public int MaxWinStreak;
    }

    [SpacetimeDB.Table(Accessor = "Match", Public = true)]
    public partial struct Match
    {
        [SpacetimeDB.AutoInc]
        [SpacetimeDB.PrimaryKey] 
        public ulong Id;

        public Identity? LeftPlayer;
        public Identity? RightPlayer;

        //state 0: MatchMaking, state 1: preapareing, state 2:started, state 3: finished
        public int State;
        public Identity? Winner;

        public float? TimeInMilSecondPlayerLeft;
        public float? TimeInMilSecondsPlayerRight;

        public bool LeftPlayerReady;
        public bool RightPlayerReady;
    }

    [SpacetimeDB.Reducer]
    public static void Shoot(ReducerContext ctx, float timeInMilSeconds, ulong matchId)
    {
        var match = ctx.Db.Match.Iter().Single(x => x.Id == matchId);
        
        if (match.LeftPlayer == ctx.Sender)
        {
            match.TimeInMilSecondPlayerLeft = timeInMilSeconds;
        } 

        if (match.RightPlayer == ctx.Sender)
        {
            match.TimeInMilSecondsPlayerRight = timeInMilSeconds;
        }

        if (match.TimeInMilSecondsPlayerRight == null || match.TimeInMilSecondPlayerLeft == null)
        {
            ctx.Db.Match.Id.Update(match);
            return;
        }
        
        match.State = 3;
        var rightWif = match.TimeInMilSecondsPlayerRight < 10000;
        var leftWif = match.TimeInMilSecondPlayerLeft < 10000;

        if (rightWif && leftWif)
        {
            var winStreakLeft = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.LeftPlayer).Single();
            winStreakLeft.CurrentWinStreak = 0;
            ctx.Db.Winstreak.Id.Update(winStreakLeft);

            var winStreakRight = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.RightPlayer).Single();
            winStreakRight.CurrentWinStreak = 0;
            ctx.Db.Winstreak.Id.Update(winStreakRight);
            ctx.Db.Match.Id.Update(match);
            return;
        }

        var leftWin = (match.TimeInMilSecondsPlayerRight > match.TimeInMilSecondPlayerLeft) && !leftWif;

        if (leftWin)
        {
            match.Winner = match.LeftPlayer;

            var winStreakLeft = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.LeftPlayer).Single();
            winStreakLeft.CurrentWinStreak++;
            if (winStreakLeft.CurrentWinStreak > winStreakLeft.MaxWinStreak)
            {
                winStreakLeft.MaxWinStreak = winStreakLeft.CurrentWinStreak;
            }
            ctx.Db.Winstreak.Id.Update(winStreakLeft);

            var winStreakRight = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.RightPlayer).Single();
            winStreakRight.CurrentWinStreak = 0;
            ctx.Db.Winstreak.Id.Update(winStreakRight);

        }
        else
        {
            match.Winner = match.RightPlayer;

            var winStreakLeft = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.LeftPlayer).Single();
            winStreakLeft.CurrentWinStreak = 0;
            ctx.Db.Winstreak.Id.Update(winStreakLeft);

            var winStreakRight = ctx.Db.Winstreak.PlayerIdentity.Filter((Identity)match.RightPlayer).Single();
            winStreakRight.CurrentWinStreak++;
            if (winStreakRight.CurrentWinStreak > winStreakRight.MaxWinStreak)
            {
                winStreakRight.MaxWinStreak = winStreakRight.CurrentWinStreak;
            }
            ctx.Db.Winstreak.Id.Update(winStreakRight);
        }

        ctx.Db.Match.Id.Update(match);
    }

    [SpacetimeDB.Reducer]
    public static void MatchMaking(ReducerContext ctx)
    {
        var openMatches = ctx.Db.Match.Iter().Where(a => a.State == 0);

        if (!openMatches.Any())
        {
            ctx.Db.Match.Insert(new Match
            {
                LeftPlayer = ctx.Sender,
                RightPlayer = null, 
                State = 0
            });
            return;
        }

        foreach (var match in openMatches)
        {
            var updatedMatch = match;

            if (!updatedMatch.LeftPlayer.HasValue)
            {
                updatedMatch.LeftPlayer = ctx.Sender;

                if (updatedMatch.RightPlayer.HasValue)
                {
                    updatedMatch.State = 1;
                }

                ctx.Db.Match.Id.Update(updatedMatch);
                return; 
            }
            else if (!updatedMatch.RightPlayer.HasValue)
            {
                updatedMatch.RightPlayer = ctx.Sender;

                if (updatedMatch.LeftPlayer.HasValue)
                {
                    updatedMatch.State = 1;
                }

                ctx.Db.Match.Id.Update(updatedMatch);
                return; 
            }
        }

        ctx.Db.Match.Insert(new Match
        {
            LeftPlayer = ctx.Sender,
            RightPlayer = null,
            State = 0
        });
    }

    [SpacetimeDB.Reducer]
    public static void CreateOrLoadPlayer(ReducerContext ctx, string requestedName)
    {
        if (ctx.Db.Player.SnailName.Find(requestedName) != null)
        {
            throw new Exception("Name is taken!");
        }

        var existingPlayer = ctx.Db.Player.Identity.Find(ctx.Sender);

        if (existingPlayer == null)
        {
            ctx.Db.Player.Insert(new Player { Identity = ctx.Sender, SnailName = requestedName });
            ctx.Db.Winstreak.Insert(new WinStreak(){CurrentWinStreak = 0, MaxWinStreak = 0, PlayerIdentity = ctx.Sender});
        }
    }

    [SpacetimeDB.Reducer]
    public static void PlayerIsReady(ReducerContext ctx, ulong matchId)
    {
        var match = ctx.Db.Match.Iter().Single(x => x.Id == matchId);
        
        if (match.LeftPlayer == ctx.Sender)
        {
            match.LeftPlayerReady = true;
        }
        
        if (match.RightPlayer == ctx.Sender)
        {
            match.RightPlayerReady = true;
        }

        if (match.RightPlayerReady && match.LeftPlayerReady)
        {
            match.State = 2;
        }

        ctx.Db.Match.Id.Update(match);
        
    }
}
