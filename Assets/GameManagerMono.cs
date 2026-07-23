using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerMono : MonoBehaviour
{

    [HideInInspector] public Action GunBattleGo;

    [SerializeField] private CountDown countDown;

    private bool started = false;
    private bool timergone = false;

    public float secondsBeforeDissappearCountdown = 3;

    private float startTime;

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

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
           

            started = true;
            GunBattleGo?.Invoke();
            countDown.TimerIsRunning = true;
            startTime = Time.time;
        }
        }


    }
}
