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
    [SerializeField] private MatchUIManager _matchUIManager;

    private ConnectionService _connectionService;
    private OpponentData _opponentData;

    public struct OpponentData
    {
        public bool leftPlayer;
        public Identity identity;
        public string name;
        public float? ShootTimeInMiliseconds;
    }

    void Start()
    {
        var matchMaking = ServiceRegistration.ServiceProvider.GetRequiredService<MatchMaking>();
        _connectionService = ServiceRegistration.ServiceProvider.GetRequiredService<ConnectionService>();
        _StatusText.text = "Waiting For Match";

        _matchUIManager.SetUIState(MatchUIState.Waiting);

        matchMaking.onMatchMakingFailed += () => { Debug.Log("Rohrough, faar ikke laav aa spille"); };
        matchMaking.onMatchMakingSuccess += OnMatchFound;
        matchMaking.onMathcUpdate += OnMatchUpdated;
        matchMaking.MakeaDaMactch();
    }

    public void OnMatchFound(Match match)
    {
        string loaclPlayerName = _connectionService.Connection.Db.Player.Iter()
            .First(x => x.Identity == _connectionService.Connection.Identity).SnailName;

        _loaclPlayerName.text = loaclPlayerName;
        _matchUIManager.SetLeftSnailName(loaclPlayerName);

        if (match.LeftPlayer != null && match.RightPlayer != null)
        {
            _opponentData = SetupOpponentName(match);
        }

        _connectionService.Connection.Reducers.PlayerIsReady(match.Id);

        if (match.State == 2)
        {
            OnMatchUpdated(match);
        }
    }

    public OpponentData SetupOpponentName(Match match)
    {
        string opponentName = "";
        Identity oppIdentity = new Identity();
        bool opponentLeft = false;

        if (match.LeftPlayer == _connectionService.Connection.Identity)
        {
            if (match.RightPlayer != null)
            {
                var opponent = _connectionService.Connection.Db.Player.Iter()
                    .Single(x => x.Identity == match.RightPlayer);

                opponentLeft = false;
                opponentName = opponent.SnailName;
                oppIdentity = opponent.Identity;
            }
        }
        else
        {
            if (match.LeftPlayer != null)
            {

                var opponent = _connectionService.Connection.Db.Player.Iter()
                    .Single(x => x.Identity == match.LeftPlayer);
                opponentLeft = true;
                opponentName = opponent.SnailName;
                oppIdentity = opponent.Identity;

            }
        }

        _RemotePlayerName.text = opponentName;
        _matchUIManager.SetRightSnailName(opponentName);

        return  new OpponentData()
        {
            identity = oppIdentity,
            leftPlayer = opponentLeft,
            name = opponentName,
            ShootTimeInMiliseconds = null
        };
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
        _gameManagerMono.ServerMatchFinished();
        _shootMAnager.ServerMatchFinised();

        _matchUIManager.SetUIState(MatchUIState.RoundOver);
    }

    private void StateMatchReadyLetsGo(Match match)
    {
        _StatusText.text = "MatchStarted";
        if (!_gameManagerMono.started)
        {
            _shootMAnager.OnGunBattleGo();
            _gameManagerMono.started = true;
        }

        if (_opponentData.leftPlayer && match.TimeInMilSecondsPlayerRight != null && match.TimeInMilSecondsPlayerLeft == null)
        {
            _shootMAnager.HandleLocalPlayerShotLogic(match.TimeInMilSecondsPlayerRight);
        }

        if (_opponentData.leftPlayer && match.TimeInMilSecondsPlayerLeft != null && match.TimeInMilSecondsPlayerRight == null)
        {
            _shootMAnager.HandleLocalPlayerShotLogic(match.TimeInMilSecondsPlayerLeft);
        }

        if (_opponentData.ShootTimeInMiliseconds == null)
        {
            if (_opponentData.leftPlayer)
            {
                if (match.TimeInMilSecondsPlayerLeft != null)
                {
                    _opponentData.ShootTimeInMiliseconds = match.TimeInMilSecondsPlayerLeft;
                    _shootMAnager.OpponentShootTimeFromServer((float)_opponentData.ShootTimeInMiliseconds);
                }
            }
            else
            {
                if (match.TimeInMilSecondsPlayerRight != null)
                {
                    _opponentData.ShootTimeInMiliseconds = match.TimeInMilSecondsPlayerRight;
                    _shootMAnager.OpponentShootTimeFromServer((float)_opponentData.ShootTimeInMiliseconds);
                }
            }

            if (_opponentData.ShootTimeInMiliseconds != null)
            {
                //TODO: Do State update opponent shot. mabye dont need??
                //TODO: issue is one player never shoots.
                //TODO: Do a local check to see if the local paleyer shot - if not Shoot with a time of 0;
            }
        }
    }

    private void StatePreparingForMatch(Match match)
    {
        _StatusText.text = "OpponentFound";
        SetupOpponentName(match);

        _matchUIManager.SetUIState(MatchUIState.Dueling);
    }

    private void StateLookingForMatch(Match match)
    {
        _StatusText.text = "Searching for opponent";

        // Seems to not get called?
    }


    private void ThrowPlayerBackToMenu(Match match)
    {
        //TODO: this is for when shit hits the fan
        _StatusText.text = "Something went wrong";

        Debug.Log("Throwign the player out, state is fed");
    }
}
