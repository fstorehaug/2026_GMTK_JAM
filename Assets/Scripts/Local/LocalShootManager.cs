using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine.SceneManagement;

public class LocalShootManager : MonoBehaviour
{
    [SerializeField] private LocalGameManagerMono _gameManager;
    [SerializeField] private MatchUIManager _matchUIManager;
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


    private bool _leftMoving = false;
    private bool _rightMoving = false;
    // For moving up the slope at an angle.
    private float _linearScaling = 0.206f;

    public float WinningShootTime { get; private set; }
    public float ShootTimeLeft { get; private set; }
    public float ShootTimeRight { get; private set; }
    public float ScoreLeft { get; private set; }
    public float ScoreRight { get; private set; }

    private bool _leftHasShot = false;
    private bool _leftGotShot = false;
    private bool _rightHasShot = false;
    private bool _rightGotShot = false;

    private int randomMapIndex;

    private CountDownData _countDownData;

    public void Awake()
    {
        ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<ScopeService>().OnRenewScope += () =>
        {
            _countDownData = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<CountDownData>();
        };

        _countDownData = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<CountDownData>();

        
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
        _leftTimeText.gameObject.SetActive(false);
        _rightTimeText.gameObject.SetActive(false);

        MyAudioManager.playAudio(4);
        MyAudioManager.fade(4,1,1);

        _leftPlayerScript.updateVisuals(TournamentState.players[0].timesShot);
        _rightPlayerScript.updateVisuals(TournamentState.players[1].timesShot);
    }

    public void OnGunBattleGo()
    {
        _countDownData = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<CountDownData>();
        _countDownData.TimerIsRunning = true;
        _leftMoving = true;
        _rightMoving = true;
        _leftPlayerScript.TurnAround(180);
        _rightPlayerScript.TurnAround(180);
        MyAudioManager.playAudio(2);
        backgrounds[randomMapIndex].BeginAnimations();
    }

    private void Update()
    {
        DoMove(Time.deltaTime);

        if (_countDownData.TimerIsRunning && !_leftHasShot && Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            ShootLeft();
        }

        if (_countDownData.TimerIsRunning && !_rightHasShot && Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            ShootRight();
        }

        if (_leftHasShot && _rightHasShot)
        {
            _matchUIManager.SetUIState(MatchUIState.RoundOver);
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
        if (_leftMoving)
        {
            _leftPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * _speedMod * -1 ;
            _leftPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* _speedMod;
            _leftPlayerScript.Move();
        }

        if (_rightMoving)
        {
            _rightPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * _speedMod;
            _rightPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* _speedMod;
            _rightPlayerScript.Move();
        }
    }

    public void ShootLeft()
    {
        _countDownData.TimerVisible = true;
        _leftMoving = false;

        ShootTimeLeft = _countDownData.TimeSinceStrat;
        
        _leftHasShot = true;
        _countDownData.StopTimerVisually = true;
        _leftTimeText.gameObject.SetActive(true);
        _leftTimeText.text = CountDownData.FormatTime(ShootTimeLeft);

        if (_leftGotShot) return;

        _leftPlayerScript.TurnAround(180);


        if (ShootTimeLeft >= 10)
        {
            _rightPlayerScript.GetShot();
            _leftPlayerScript.Shoot(ShootTimeLeft);
            _shootPlane.SetActive(true);
            _rightMoving = false;
        }
        else
            _leftPlayerScript.TooSoon();
        
    }

    public void ShootRight()
    {
        _countDownData.TimerVisible = true;
        _rightMoving = false;

        ShootTimeRight = _countDownData.TimeSinceStrat;
        
        _rightHasShot = true;
        _countDownData.StopTimerVisually = true;
        _rightTimeText.gameObject.SetActive(true);
        _rightTimeText.text = CountDownData.FormatTime(ShootTimeRight);

        if (_rightGotShot) return;

        _rightPlayerScript.TurnAround(180);


        if (ShootTimeRight >= 10)
        {
            _leftPlayerScript.GetShot();
            _rightPlayerScript.Shoot(ShootTimeRight);
            _shootPlane.SetActive(true);
            _leftMoving = false;
        }
        else
            _rightPlayerScript.TooSoon();
        
    }
}
