using System;
using System.Linq;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;

public class MatchMaking
{
    private ConnectionService _connectionService;

    public Action onMatchMakingFailed;
    public Action<Match> onMatchMakingSuccess;
    public Action<Match> onMathcUpdate;

    public MatchMaking(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public void MakeaDaMactch()
    {
        _connectionService.Connection.Reducers.OnMatchMaking += Reducers_OnMatchMaking;
        _connectionService.Connection.Reducers.MatchMaking();
    }

    private void Reducers_OnMatchMaking(SpacetimeDB.Types.ReducerEventContext ctx)
    {
        if (ctx.Event.Status is Status.Failed)
        {
            Debug.Log("FailedToFindMatch");
            onMatchMakingFailed?.Invoke();
            return;
        }

        var match = ctx.Db.Match.Iter().Where(x =>
            x.State == 0).Single(x => x.LeftPlayer == _connectionService.Identity || x.RightPlayer == _connectionService.Identity);

        onMatchMakingSuccess?.Invoke(match);

        ctx.Db.Match.OnUpdate += (context, row, newRow) =>
        {
            if (row.Id == match.Id)
            {
                onMathcUpdate?.Invoke(newRow);
            }
        };

    }
}
