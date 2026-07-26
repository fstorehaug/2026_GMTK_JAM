using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine.SceneManagement;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private GameManagerMono _gameManager;
    [SerializeField] private float _speedMod = 0.5f;
    [SerializeField] private float _maxScore = 100;

    [SerializeField] private Transform _leftPlayer;
    [SerializeField] private Transform _rightPlayer;

    [SerializeField] private SnailPlayer _leftPlayerScript;
    [SerializeField] private SnailPlayer _rightPlayerScript;

    [SerializeField] private TextMeshProUGUI _leftTimeText;
    [SerializeField] private TextMeshProUGUI _rightTimeText;

    [SerializeField] private GameObject _shootPlane;

    [SerializeField] private AnimationCurve _scoreCurve;

    [SerializeField] private AudioManager MyAudioManager;

    [SerializeField] private BackgroundManager[] backgrounds;


    private ShootService _ShootService;
    private MatchMaking _matchService;

    private bool _moving = false;
    // For moving up the slope at an angle.
    private float _linearScaling = 0.206f;

    public float WinningShootTime { get; private set; }
    public float ShootTimeLeft { get; private set; }
    public float ShootTimeRight { get; private set; }
    public float ScoreLeft { get; private set; }
    public float ScoreRight { get; private set; }

    private bool _leftHasShot = false;
    private bool _rightHasShot = false;

    private int randomMapIndex;

    private CountDownData _countDownData;

    public void Awake()
    {
        ServiceRegistration.ServiceProvider.GetRequiredService<ScopeService>().OnRenewScope += () =>
        {
            _countDownData = ServiceRegistration.ServiceProvider.GetRequiredService<CountDownData>();
        };

        _countDownData = ServiceRegistration.ServiceProvider.GetRequiredService<CountDownData>();

        
        foreach (BackgroundManager g in backgrounds)
        {
            g.gameObject.SetActive(false);
        }
        
        randomMapIndex = UnityEngine.Random.RandomRange(0, backgrounds.Length);
        backgrounds[randomMapIndex].gameObject.SetActive(true);
        
        /** use this to test specific stage in duelscene scene
        backgrounds[1].gameObject.SetActive(true);
        backgrounds[1].BeginAnimations();
        **/
    }
    public void Start()
    {
        _ShootService = ServiceRegistration.ServiceProvider.GetRequiredService<ShootService>();
        _matchService = ServiceRegistration.ServiceProvider.GetRequiredService<MatchMaking>();
        
        _leftTimeText.gameObject.SetActive(false);
        _rightTimeText.gameObject.SetActive(false);

        MyAudioManager.playAudio(4);
        MyAudioManager.fade(4,1,1);

        _leftPlayerScript.updateVisuals(TournamentState.players[0].timesShot);
        _rightPlayerScript.updateVisuals(TournamentState.players[1].timesShot);
    }

    public void OnGunBattleGo()
    {
        _countDownData = ServiceRegistration.ServiceProvider.GetRequiredService<CountDownData>();
        _countDownData.TimerIsRunning = true;
        _moving = true;
        _leftPlayerScript.TurnAround(180);
        _rightPlayerScript.TurnAround(180);
        MyAudioManager.playAudio(2);
        backgrounds[randomMapIndex].BeginAnimations();
    }

    private void Update()
    {
        if (_moving)
        {
            DoMove(Time.deltaTime);
        }
        //TODO: this code is fine - admire it.
        if (_countDownData.TimerIsRunning && !_leftHasShot && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShootLeft();
            _moving = false;
        }


#if UNITY_EDITOR
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // 2. Get the name of the currently active scene
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 3. Pass that name into the LoadScene method
            SceneManager.LoadScene(currentSceneName);
        }
#endif
    }

    private void DoMove(float deltaTime)
    {
        _leftPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * _speedMod * -1 ;
        _rightPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * _speedMod;
        
        _leftPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* _speedMod;
        _rightPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* _speedMod;

        _leftPlayerScript.Move();
        _rightPlayerScript.Move();
    }

    private void ShootRightRemotePlayer(float time)
    {
        _countDownData.TimerVisible = true;

        _rightPlayerScript.TurnAround(180);
        ShootTimeRight = time;
        
        _rightTimeText.text = CountDownData.FormatTime(ShootTimeRight);
        _rightHasShot = true;
        _rightTimeText.gameObject.SetActive(true);
        _shootPlane.SetActive(true);
        _moving = false;

        if (time > 10)
        {
            _leftPlayerScript.GetShot();
        }
        
        _rightPlayerScript.Shoot(time);
    }

    public void ShootLeft()
    {
        _countDownData.TimerVisible = true;

        _leftPlayerScript.TurnAround(180);
        ShootTimeLeft = _countDownData.TimeSinceStrat;

        _ShootService.SHOOT(ShootTimeLeft, _matchService.CurrentMatchId);
        
        _leftHasShot = true;
        _countDownData.StopTimerVisually = true;
        _leftTimeText.gameObject.SetActive(true);
        _leftTimeText.text = CountDownData.FormatTime(ShootTimeLeft);
        _shootPlane.SetActive(true);
        
        if (ShootTimeLeft > 10)
        {
            _rightPlayerScript.GetShot();
        }

        _leftPlayerScript.Shoot(ShootTimeLeft);
    }

    public void OpponentShootTimeFromServer(float opponentDataShootTimeInMiliseconds)
    {
        ShootRightRemotePlayer(opponentDataShootTimeInMiliseconds);
    }
    public void ServerMatchFinised()
    {
        
    }

    public void HandleLocalPlayerShotLogic(float? LocalPlayerShootTimeServer)
    {
        _leftHasShot = true;
        if (LocalPlayerShootTimeServer > 10)
        {
            if (LocalPlayerShootTimeServer + 4 > _countDownData.TimeSinceStrat)
            {
                _ShootService.SHOOT(0, _matchService.CurrentMatchId);
            }
        }
        else
        {
            if (LocalPlayerShootTimeServer + 20 > _countDownData.TimeSinceStrat)
            {
                _ShootService.SHOOT(0, _matchService.CurrentMatchId);
            }
        }
    }
}
