using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SpacetimeDB.Types;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerMono : MonoBehaviour
{
    [SerializeField]
    private bool singlePlayer = false;

    [SerializeField] private CountDown countDown;
    [SerializeField] private ShootManager shootManager;

    public bool started = false;
    private bool timergone = false;

    public float secondsBeforeDissappearCountdown = 3;

    private float startTime;

    private MatchMaking _matchMaking;
    private ConnectionService _connectionService;

    private bool matchFinished = false;

    public void Start()
    {
        _matchMaking = ServiceRegistration.ServiceProvider.GetRequiredService<MatchMaking>();
        _matchMaking.onMathcUpdate += OnMathcUpdate;

        _connectionService = ServiceRegistration.ServiceProvider.GetRequiredService<ConnectionService>();

        countDown.timeText.gameObject.SetActive(true);
    }

    private void OnMathcUpdate(Match obj)
    {
        if (obj.LeftPlayerReady && obj.RightPlayerReady)
        {
            started = true;
            startTime = Time.time;
        }
    }

    private void Update()
    {
        if (started == true)
        {
            if (Time.time - startTime > secondsBeforeDissappearCountdown)
            {
                    if (timergone)
                        return;

                    countDown.timeText.gameObject.SetActive(false);
                    timergone = true;
            }

        } else
        {
            if (singlePlayer)
            {
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    started = true;
                    shootManager.OnGunBattleGo();
                    //countDown.TimerIsRunning = true;
                    startTime = Time.time;
                }
            }
            else
            {
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && matchFinished == true)
                {
                   _matchMaking.MakeaDaMactch();
                   matchFinished = false;
                }
            }
        }
    }

    public void ServerMatchFinished()
    {
        matchFinished = true;
    }
}
