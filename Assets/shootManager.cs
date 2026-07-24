using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;
using TMPro;
using System.Collections;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private GameManagerMono _gameManager;
    [SerializeField] private float speedMod = 1;

    [SerializeField] private Transform _leftPlayer;
    [SerializeField] private Transform _rightPlayer;
    
    [SerializeField] private TextMeshProUGUI _leftTimeText;
    [SerializeField] private TextMeshProUGUI _rightTimeText;

    [SerializeField] private GameObject _shootPlane;
    [SerializeField] private CountDown _countDown;


    private Animator _leftPlayerAnimator;
    private Animator _rightPlayerAnimator;
    public GameObject LeftPlayerShootVFX;
    public GameObject RightPlayerShootVFX;
    public GameObject LeftPlayerShootLine;
    public GameObject RightPlayerShootLine;
    public GameObject LeftPlayerSplat;
    public GameObject RightPlayerSplat;
    [SerializeField] private AudioManager MyAudioManager;



    private bool _moving = false;
    // For moving up the slope at an angle.
    private float _linearScaling = 0.206f;

    public float StartTime { get; private set; }
    public float WinningShootTime { get; private set; }
    public float ShootTimeLeft { get; private set; }
    public float ShootTimeRight { get; private set; }

    private bool _leftHasShot = false;
    private bool _rightHasShot = false;

    public void Start()
    {
        _gameManager.GunBattleGo += OnGunBattleGo;
        _leftTimeText.gameObject.SetActive(false);
        _rightTimeText.gameObject.SetActive(false);
        _rightPlayerAnimator = _rightPlayer.GetComponent<Animator>();
        _leftPlayerAnimator = _leftPlayer.GetComponent<Animator>();
        //_leftPlayerShootVFX = _leftPlayer.GetChild(4).GetComponent<MeshRenderer>();
        //_rightPlayerShootVFX = _rightPlayer.GetChild(4).GetComponent<MeshRenderer>();
        MyAudioManager.playAudio(4);
        MyAudioManager.fade(4,1,1f);
    }

    public void OnGunBattleGo()
    {
        StartTime = Time.time;
        _moving = true;
        TurnAround(_leftPlayer);
        TurnAround(_rightPlayer);
        MyAudioManager.playAudio(2);
    }

    private void Update()
    {
        if (!_leftHasShot && Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            ShootLeft();
            _moving = false;
        }
  
        if (!_rightHasShot && Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            ShootRight();
            _moving = false;
        }

        if (_moving)
        {
            DoMove(Time.deltaTime);
        }
    }

    private void DoMove(float deltaTime)
    {
        _leftPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * speedMod * -1 ;
        _rightPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * speedMod;
        
        _leftPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* speedMod;
        _rightPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* _linearScaling* speedMod;

        _leftPlayerAnimator.SetInteger("state", 1);
        _rightPlayerAnimator.SetInteger("state", 1);

    }

    private void ShootRight()
    {
        TurnAround(_rightPlayer);
        _shootPlane.SetActive(true);
        _countDown.timeText.gameObject.SetActive(true);
        _countDown.TimerIsRunning = false;

        ShootTimeRight = Time.time - StartTime;
        _rightTimeText.gameObject.SetActive(true);
        _rightTimeText.text = _countDown.FormatTime(ShootTimeRight);
        if (WinningShootTime == 0) WinningShootTime = ShootTimeRight;
        _rightHasShot = true;

        _rightPlayerAnimator.SetInteger("state", 2);
        _leftPlayerAnimator.SetInteger("state", 3);

        StartCoroutine(FlashShootVFX(false));
        
    }

    public void ShootLeft()
    {
        TurnAround(_leftPlayer);
        _shootPlane.SetActive(true);
        _countDown.timeText.gameObject.SetActive(true);
        _countDown.TimerIsRunning = false;

        ShootTimeLeft = Time.time - StartTime;
        _leftTimeText.gameObject.SetActive(true);
        _leftTimeText.text = _countDown.FormatTime(ShootTimeLeft);
        if (WinningShootTime == 0) WinningShootTime = ShootTimeLeft;
        _leftHasShot = true;

        _leftPlayerAnimator.SetInteger("state", 2);
        _rightPlayerAnimator.SetInteger("state", 3);

        StartCoroutine(FlashShootVFX(true));
    }

    IEnumerator FlashShootVFX(bool left)
    {
        MyAudioManager.playAudio(0);
        MyAudioManager.playAudio(2);
        MyAudioManager.stopAudio(4);
        yield return new WaitForSeconds(0.05f);
        if (left == false)
        {
            RightPlayerShootVFX.SetActive(true);
            RightPlayerShootLine.SetActive(true);
            LeftPlayerSplat.SetActive(true);
        }
        else
        {
            LeftPlayerShootVFX.SetActive(true);
            LeftPlayerShootLine.SetActive(true);
            RightPlayerSplat.SetActive(true);
        }

        
    }

    private void TurnAround(Transform shooterTransform)
    {
        var temp = shooterTransform.localScale;
        temp.x = -temp.x;
        shooterTransform.localScale = temp;
    }
}
