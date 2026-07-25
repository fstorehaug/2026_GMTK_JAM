using System;
using System.Linq;
using SpacetimeDB;
using SpacetimeDB.Types;
using TMPro.EditorUtilities;
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
        var matchiter = _connectionService.Connection.Db.Match.Iter();
        var openMatchers = matchiter.Where(x => x.State != 3).ToList();
        var match = openMatchers.FirstOrDefault(x => x.LeftPlayer == _connectionService.Connection.Identity || x.RightPlayer == _connectionService.Connection.Identity);

        if (match != null)
        {
            onMatchMakingSuccess.Invoke(match);
            _connectionService.Connection.Db.Match.OnUpdate += (context, row, newRow) =>
            {
                if (newRow.Id == match.Id)
                {
                    onMathcUpdate?.Invoke(newRow);
                }
            };
            return;
        }

        _connectionService.Connection.Reducers.OnMatchMaking += Reducers_OnMatchMaking;
        _connectionService.Connection.Reducers.MatchMaking();
    }

    private void Reducers_OnMatchMaking(SpacetimeDB.Types.ReducerEventContext ctx)
    {
        var matchiter = _connectionService.Connection.Db.Match.Iter();
        var openMatchers = matchiter.Where(x => x.State != 3).ToList();
        var match = openMatchers.First(x => x.LeftPlayer == _connectionService.Connection.Identity || x.RightPlayer == _connectionService.Connection.Identity);

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
