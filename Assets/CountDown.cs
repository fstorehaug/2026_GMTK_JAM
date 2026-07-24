using System;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] public float CountdownTime = 10f;
    public float TimeRemaining { get; private set; }
    public float CurrentTime { get => CountdownTime - TimeRemaining; }

    [SerializeField] public TextMeshProUGUI timeText;

    public bool TimerIsRunning { get; set; } = false;
    public bool StopTimerVisually { get; set; }

    private void Awake()
    {
        TimeRemaining = CountdownTime;
    }

    private void Update()
    {
        if (TimerIsRunning)
        {
            TimeRemaining -= Time.deltaTime;

            if (!StopTimerVisually)
                timeText.text = FormatTime(TimeRemaining);
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
        float t = 1.0f - (TimeRemaining / CountdownTime);
        float exponentialTime = t * t;

        return Mathf.Lerp(0, 1, exponentialTime);
    }
}
