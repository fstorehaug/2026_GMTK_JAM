using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private GameManagerMono gameManager;
    [SerializeField] private float speedMod = 1;

    [SerializeField] private Transform leftPlayer;
    [SerializeField] private Transform rightPlayer;

    [SerializeField] private GameObject ShootPlane;
    [SerializeField] private CountDown countDown;


    private bool _moving = false;
    private float linarScaling = 0.206f;

    public float startTime = 0;
    public float shootTime = 0;

    private bool hasShot = false;

    public void Start()
    {
        gameManager.GunBattleGo += OnGunBattleGo;
    }

    public void OnGunBattleGo()
    {
        startTime = Time.time;
        _moving = true;
        TurnAround(leftPlayer);
        TurnAround(rightPlayer);
    }

    private void Update()
    {

        if (hasShot == true)
            return;

        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            ShootLeft();
            _moving = false;
        }
  
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
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
        leftPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * speedMod * -1 ;
        rightPlayer.position += new Vector3(deltaTime * UnityEngine.Random.value,0 ,0) * speedMod;
        
        leftPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* linarScaling* speedMod;
        rightPlayer.position += new Vector3(0, deltaTime * UnityEngine.Random.value,0)* linarScaling* speedMod;
    }

    private void ShootRight()
    {
        TurnAround(rightPlayer);
        ShootPlane.SetActive(true);
        shootTime = Time.time;
        countDown.timeText.gameObject.SetActive(true);
        countDown.TimerIsRunning = false;
        hasShot = true;
    }
    public void ShootLeft()
    {
        hasShot = true;
        TurnAround(leftPlayer);
        ShootPlane.SetActive(true);
        shootTime = Time.time;
        countDown.timeText.gameObject.SetActive(true);
        countDown.TimerIsRunning = false;
    }

    private void TurnAround(Transform shooterTransform)
    {
        var temp = shooterTransform.localScale;
        temp.x = -temp.x;
        shooterTransform.localScale = temp;
    }
}

public enum KeyEnum
{
    AKey, LKey
}
