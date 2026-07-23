using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private bool _isPaused;

    [SerializeField] private GameObject _menuContainer;
    [SerializeField] private GameObject _mainMenuObj;
    [SerializeField] private GameObject _settingsMenuObj;

    [SerializeField] private Button _startDuelButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _applySettingsButton;

    private void Awake()
    {
        _startDuelButton.onClick.AddListener(OnDuelPressed);
        _settingsButton.onClick.AddListener(OnSettingsPressed);
        _exitButton.onClick.AddListener(OnExitPressed);

        _applySettingsButton.onClick.AddListener(OnApplySettingsPressed);

        SetMenuState(true);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetMenuState(!_isPaused);
        }
    }

    private void OnDuelPressed()
    {
        SetMenuState(false);
    }

    private void OnSettingsPressed()
    {
        _mainMenuObj.SetActive(false);
        _settingsMenuObj.SetActive(true);
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }

    private void OnApplySettingsPressed()
    {
        _mainMenuObj.SetActive(true);
        _settingsMenuObj.SetActive(false);
    }

    private void SetMenuState(bool paused)
    {
        _isPaused = paused;
        _menuContainer.SetActive(paused);

        if (paused)
        {
            _mainMenuObj.SetActive(true);
            _settingsMenuObj.SetActive(false);
        }

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }
}
