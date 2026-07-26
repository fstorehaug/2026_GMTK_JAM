using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalGameManagerMono : MonoBehaviour
{
    [SerializeField] private CountDown _countDown;
    [SerializeField] private LocalShootManager _shootManager;
    [SerializeField] private MatchUIManager _matchUIManager;

    public bool started = false;
    private bool timergone = false;

    public float secondsBeforeDissappearCountdown = 3;

    private float startTime;

    private bool matchFinished = false;

    public void Start()
    {
        _countDown.timeText.gameObject.SetActive(true);
    }

    private void Update()
    {
        // if (Time.timeSinceLevelLoad > 3f)
        // {
        //     started = true;
        //     startTime = Time.time;
        // }

        if (started == true)
        {
            if (Time.time - startTime > secondsBeforeDissappearCountdown)
            {
                if (timergone)
                    return;

                _countDown.timeText.gameObject.SetActive(false);
                timergone = true;
            }

        } else
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                started = true;
                _shootManager.OnGunBattleGo();
                //countDown.TimerIsRunning = true;
                startTime = Time.time;
            }
        }
    }
}
