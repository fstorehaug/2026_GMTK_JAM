using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OtherSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _windowModeDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private int _currentFullscreenModeIndex;

    private double _currentRefreshRate;
    private int _currentResolutionIndex;

    public void Start()
    {
        LoadWindowModeDropdown();
        LoadResolutionDropdown();
    }

    private void LoadWindowModeDropdown()
    {
        _windowModeDropdown.ClearOptions();

        List<string> options = new();
        for (int i = 0; i < Enum.GetNames(typeof(FullScreenMode)).Length; i++)
        {
            string fullScreenOption = Enum.GetName(typeof(FullScreenMode), i);
            options.Add(fullScreenOption);

            if (Enum.GetNames(typeof(FullScreenMode))[i] == Enum.GetName(typeof(FullScreenMode), Screen.fullScreenMode))
                _currentFullscreenModeIndex = i;
        }

        _windowModeDropdown.AddOptions(options);
        _windowModeDropdown.value = _currentFullscreenModeIndex;
        _windowModeDropdown.RefreshShownValue();
    }

    private void LoadResolutionDropdown()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new();

        _resolutionDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].refreshRateRatio.value == _currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = $"{_filteredResolutions[i].width} x {_filteredResolutions[i].height} {_currentRefreshRate}hz";
            options.Add(resolutionOption);

            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
                _currentResolutionIndex = i;
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreenMode(int fullScreenModeIndex)
    {
        Screen.fullScreenMode = (FullScreenMode)fullScreenModeIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
