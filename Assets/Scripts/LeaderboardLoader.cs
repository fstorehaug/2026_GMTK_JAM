using UnityEngine;
using System.Linq;
using System;
using SpacetimeDB.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private Transform _leaderboardListRoot;
    [SerializeField] private LeaderboardEntry _leaderboardEntryTemplate;

    [SerializeField] private List<LeaderboardEntry> _leaderboardList;

    private ConnectionService _cService;

    public void OnEnable()
    {
        for (int i = 0; i < _leaderboardList.Count; i++)
        {
            Destroy(_leaderboardList[i].gameObject);
        }

        _leaderboardList.Clear();

        _cService = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<ConnectionService>();
        var winstreakList = _cService.Connection.Db.Winstreak.Iter().OrderByDescending(x => x.MaxWinStreak).Take(50).ToList();
        _cService.Connection.Db.Winstreak.OnUpdate += UpdateLeaderboard;

        for (int i = 0; i < winstreakList.Count; i++)
        {
            LeaderboardEntry entry = Instantiate(_leaderboardEntryTemplate, _leaderboardListRoot);

            entry.Ranking = i + 1;
            entry.UserName = _cService.Connection.Db.Player.Iter().First(x => x.Identity == winstreakList[i].PlayerIdentity).SnailName;
            entry.StreakCount = winstreakList[i].MaxWinStreak;
            entry.gameObject.SetActive(true);

            _leaderboardList.Add(entry);
        }
    }

    public void OnDisable()
    {
        _cService.Connection.Db.Winstreak.OnUpdate -= UpdateLeaderboard;
    }

    private void UpdateLeaderboard(EventContext context, WinStreak oldRow, WinStreak newRow)
    {
        throw new NotImplementedException();
    }
}
