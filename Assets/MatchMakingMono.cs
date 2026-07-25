using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SpacetimeDB;
using SpacetimeDB.Types;
using TMPro;
using UnityEngine;

public class MatchMakingMono : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loaclPlayerName;
    [SerializeField] private TextMeshProUGUI _RemotePlayerName;
    [SerializeField] private TextMeshProUGUI _StatusText;
    [SerializeField] private GameManagerMono _gameManagerMono;
    [SerializeField] private ShootManager _shootMAnager;

    private ConnectionService _connectionService;
    private OpponentData _opponentData;

    struct OpponentData
    {
        public bool leftPlayer;
        public Identity identity;
        public string name;
    }

    void Start()
    {
        var matchMaking = ServiceRegistration.ServiceProvider.GetRequiredService<MatchMaking>();
        _connectionService = ServiceRegistration.ServiceProvider.GetRequiredService<ConnectionService>();
        _StatusText.text = "Waiting For Match";

        matchMaking.onMatchMakingFailed += () => { Debug.Log("Rohrough, faar ikke laav aa spille"); };
        matchMaking.onMatchMakingSuccess += OnMatchFound;
        matchMaking.onMathcUpdate += OnMatchUpdated;
        matchMaking.MakeaDaMactch();
    }

    public void OnMatchFound(Match match)
    {
        _loaclPlayerName.text = _connectionService.Connection.Db.Player.Iter()
            .Single(x => x.Identity == _connectionService.Identity).SnailName;
        
        if (match.LeftPlayer == _connectionService.Identity)
        {
            _RemotePlayerName.text = _connectionService.Connection.Db.Player.Iter()
                .Single(x => x.Identity == match.RightPlayer).SnailName;
        }
        else
        {
            _RemotePlayerName.text = _connectionService.Connection.Db.Player.Iter()
                .Single(x => x.Identity == match.LeftPlayer).SnailName;
        }

        //player is ready.
    }

    public void OnMatchUpdated(Match match)
    {
        switch (match.State)
        {
            case 0:
                StateLookingForMatch(match);
                break;
            case 1:
                StatePreparingForMatch(match);
                break;
            case 2:
                StateMatchReadyLetsGo(match);
                break;
            case 3:
                StateMatchFinished(match);
                break;
            default:
                ThrowPlayerBackToMenu(match);
                break;
        }
    }

    private void StateMatchFinished(Match match)
    {
        _StatusText.text = "MatchFinished";
    }

    private void StateMatchReadyLetsGo(Match match)
    {
        _StatusText.text = "MatchStarted";
        if (!_gameManagerMono.started)
        {
            _gameManagerMono.GunBattleGo?.Invoke();
            _gameManagerMono.started = true;
        }

    }

    private void StatePreparingForMatch(Match match)
    {
        _StatusText.text = "OpponentFound";
    }

    private void StateLookingForMatch(Match match)
    {
        _StatusText.text = "Searching for opponent";
    }


    private void ThrowPlayerBackToMenu(Match match)
    {
        //TODO: this is for when shit hits the fan
        _StatusText.text = "Something went wrong";

        Debug.Log("Throwign the player out, state is fed");
    }
}
