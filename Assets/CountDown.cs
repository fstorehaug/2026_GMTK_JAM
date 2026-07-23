using System;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] public float startTime = 120f;
    [HideInInspector] public float timeRemaining;

    [SerializeField] public TextMeshProUGUI timeText;

    public bool TimerIsRunning
    {
        get;
        set;
    } = false;

    private void Awake()
    {
        timeRemaining = startTime;
    }

    private void Update()
    {
        if (TimerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                TimerIsRunning = false;
                DisplayTime(timeRemaining);
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = Mathf.FloorToInt((timeToDisplay % 1) * 100);
        float microFractions = Mathf.FloorToInt((timeToDisplay * 10000) % 100);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            minutes,
            seconds,
            milliseconds,
            microFractions);
    }

    public float ExponentialScale()
    {
        float t = 1.0f - (timeRemaining / startTime);
        float exponentialTime = t * t;

        return Mathf.Lerp(0, 1, exponentialTime);
    }


}
