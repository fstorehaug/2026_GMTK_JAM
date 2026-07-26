using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using TMPro;

public record CountDownData
{
    public const float STARTTIME = 10;
    public CountDownData()
    {
        TimeRemaining = STARTTIME;
        TimerIsRunning = false;
        StopTimerVisually = false;
        TimerVisible = true;
    }

    public float TimeRemaining;
    public bool TimerIsRunning;
    public bool StopTimerVisually;
    public bool TimerVisible;

    public float TimeSinceStrat => STARTTIME - TimeRemaining;

    public static string FormatTime(float timeToDisplay)
    {
        TimeSpan ts = TimeSpan.FromSeconds(timeToDisplay);
        string minus = timeToDisplay < 0 ? "-" : "";

        return "<mspace=0.35em>" + minus + ts.ToString("mm\\:ss\\:fff");
    }

    public float ExponentialScale()
    {
        float t = 1.0f - (TimeRemaining / CountDownData.STARTTIME);
        float exponentialTime = t * t;

        return Mathf.Lerp(0, 1, exponentialTime);
    }
}

public class ScopeService
{
    public Action OnRenewScope;

    public void RenewScope()
    {
        ServiceRegistration.ServiceProvider = ServiceRegistration.ServiceProvider.ServiceProvider.CreateScope();
        OnRenewScope?.Invoke();
    }
}


public class CountDown : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timeText;
    private CountDownData _countDownData;

    private void Awake()
    {
        _countDownData = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<CountDownData>();
        ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<ScopeService>().OnRenewScope += () =>
        {
            _countDownData = ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<CountDownData>();

        };
    }

    private void Update()
    {
        if (_countDownData.TimerVisible && !timeText.gameObject.activeSelf)
        {
            timeText.gameObject.SetActive(true);
        }

        if (!_countDownData.TimerVisible && timeText.gameObject.activeSelf)
        {
            timeText.gameObject.SetActive(false);
        }

        if (!_countDownData.StopTimerVisually)
        {
            timeText.text = CountDownData.FormatTime(_countDownData.TimeRemaining);
        }

        if (!_countDownData.TimerIsRunning)
            return;
        
        _countDownData.TimeRemaining -= Time.deltaTime;
        
        if (_countDownData.TimeRemaining < 7)
        {
            _countDownData.TimerVisible = false;
        }

    }

}
