using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private bool _isPaused;

    [SerializeField] private GameObject _menuContainer;
    [SerializeField] private GameObject _mainMenuObj;
    [SerializeField] private GameObject _duelMenuObj;
    [SerializeField] private GameObject _settingsMenuObj;

    [SerializeField] private Button _startDuelingButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _localDuelButton;
    [SerializeField] private Button _onlineDuelButton;
    [SerializeField] private Button _backDuelingButton;
    [SerializeField] private TMP_InputField _snailNameTextField;

    [SerializeField] private Button _backSettingsButton;

    private string _localSnailName;

    private void Awake()
    {
        _startDuelingButton.onClick.AddListener(OnDuelPressed);
        _settingsButton.onClick.AddListener(OnSettingsPressed);
        _exitButton.onClick.AddListener(OnExitPressed);

        _localDuelButton.onClick.AddListener(OnLocalDuelPressed);
        _onlineDuelButton.onClick.AddListener(OnOnlineDuelPressed);
        _backDuelingButton.onClick.AddListener(BackToMainMenu);
        _snailNameTextField.onValueChanged.AddListener(OnSnailNameChanged);

        _localSnailName = PlayerPrefs.GetString("LocalSnailName", "Anonymous Snail");
        _snailNameTextField.SetTextWithoutNotify(_localSnailName);

        _backSettingsButton.onClick.AddListener(BackToMainMenu);

        SetMenuState(true);
    }

    private void Update()
    {
        // if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        // {
        //     SetMenuState(!_isPaused);
        // }
    }

    private void OnDuelPressed()
    {
        _mainMenuObj.SetActive(false);
        _settingsMenuObj.SetActive(false);
        _duelMenuObj.SetActive(true);
    }

    private void OnLocalDuelPressed()
    {
        SceneManager.LoadScene("DuelScene");
    }

    private void OnOnlineDuelPressed()
    {
        RegisterPlayerWithDb playerDB = new();
        playerDB.RegisterPlayer(_localSnailName);

        playerDB.registrationFail += OnRegisterFail;
        playerDB.regitratinSucess += OnRegisterSuccess;
    }

    private void OnRegisterFail()
    {
        Debug.Log("Player register failed D:");
    }

    private void OnRegisterSuccess()
    {
        Debug.Log("Player register succeeded :D");

        SceneManager.LoadScene("DuelScene");
    }

    private void OnSnailNameChanged(string value)
    {
        _localSnailName = value;
        PlayerPrefs.SetString("LocalSnailName", _localSnailName);
    }

    private void OnSettingsPressed()
    {
        _mainMenuObj.SetActive(false);
        _settingsMenuObj.SetActive(true);
        _duelMenuObj.SetActive(false);
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }

    private void BackToMainMenu()
    {
        _mainMenuObj.SetActive(true);
        _settingsMenuObj.SetActive(false);
        _duelMenuObj.SetActive(false);
    }

    private void SetMenuState(bool paused)
    {
        _isPaused = paused;
        _menuContainer.SetActive(paused);

        if (paused)
        {
            _mainMenuObj.SetActive(true);
            _settingsMenuObj.SetActive(false);
            _duelMenuObj.SetActive(false);
        }
    }
}
