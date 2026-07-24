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

    [SpacetimeDB.Table(Accessor = "Tournament", Public = true)]
    public partial struct Tournament
    {
        [SpacetimeDB.AutoInc]
        [SpacetimeDB.PrimaryKey]
        public ulong Id;

        public string TournamentName;
        public int MaxParticipants;

        [SpacetimeDB.Index.BTree]
        public int Progress; // 0 - lobby, 1 - started, 2 - ended

        public Identity Winner;
    }

    [SpacetimeDB.Table(Accessor = "TournamentParticipant", Public = true)]
    public partial struct TournamentParticipant
    {
        [SpacetimeDB.PrimaryKey]
        public Identity PlayerId;

        [SpacetimeDB.Index.BTree]
        public ulong TournamentId;
    }

    [SpacetimeDB.Table(Accessor = "Match", Public = true)]
    public partial struct Match
    {
        [SpacetimeDB.AutoInc]
        [SpacetimeDB.PrimaryKey]
        public ulong Id;

        [SpacetimeDB.Index.BTree]
        public ulong TournamentId;
        public int Round;

        public Identity? Player1;
        public Identity? Player2;

        public float? Player1Time;
        public float? Player2Time;

        public Identity? Winner;

        public ulong NextMatchId;
    }


    [SpacetimeDB.Reducer]
    public static void JoinOrCreateTournamentLobby(ReducerContext ctx, string preferredName)
    {
        var existingRegistration = ctx.Db.TournamentParticipant.PlayerId.Find(ctx.Sender);
        if (existingRegistration != null)
        {
            throw new Exception("AlreadyInATournament");
        }

        Tournament? bestTargetLobby = null;
        int highestPlayerCountFound = -1;

        foreach (var lobby in ctx.Db.Tournament.Iter())
        {
            if (lobby.Progress == 0)
            {
                int currentCount = ctx.Db.TournamentParticipant.Iter()
                    .Count(p => p.TournamentId == lobby.Id);

                if (currentCount < lobby.MaxParticipants && currentCount > highestPlayerCountFound)
                {
                    highestPlayerCountFound = currentCount;
                    bestTargetLobby = lobby;
                }
            }
        }

        if (bestTargetLobby == null)
        {
            var newLobby = new Tournament
            {
                TournamentName = string.IsNullOrEmpty(preferredName) ? "SnailGunBulletRound" : preferredName,
                MaxParticipants = 64,
                Progress = 0,
                Winner = null
            };

            bestTargetLobby = ctx.Db.Tournament.Insert(newLobby);
        }

        ctx.Db.TournamentParticipant.Insert(new TournamentParticipant
        {
            PlayerId = ctx.Sender,
            TournamentId = bestTargetLobby.Value.Id
        });

        int finalCount = ctx.Db.TournamentParticipant.Iter().Count(p => p.TournamentId == bestTargetLobby.Value.Id);
        if (finalCount >= bestTargetLobby.Value.MaxParticipants)
        {
            var activeMatch = bestTargetLobby.Value;
            activeMatch.Progress = 1; // Transition state from "Lobby" (0) to "Started" (1)
            ctx.Db.Tournament.Id.Update(activeMatch);
        }
    }


    [SpacetimeDB.Reducer]
    public static void CreateTournament(ReducerContext ctx, string tournamentName, int maxParticipants)
    {
        ctx.Db.Tournament.Insert(
            new Tournament() { MaxParticipants = maxParticipants, TournamentName = tournamentName });
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
        }
    }
    
   
}
