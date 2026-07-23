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
            timeRemaining -= Time.deltaTime;
            timeText.text = FormatTime(timeRemaining);
        }
    }

    public string FormatTime(float timeToDisplay)
    {
        TimeSpan ts = TimeSpan.FromSeconds(timeToDisplay);
        string minus = timeToDisplay < 0 ? "-" : "";

        return "<mspace=0.58em>" + minus + ts.ToString("mm\\:ss\\:fff");
    }

    public float ExponentialScale()
    {
        float t = 1.0f - (timeRemaining / startTime);
        float exponentialTime = t * t;

        return Mathf.Lerp(0, 1, exponentialTime);
    }
}
