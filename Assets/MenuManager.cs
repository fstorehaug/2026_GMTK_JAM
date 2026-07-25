using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    enum MenuPage
    {
        Main,
        Duel,
        Leaderboard,
        Settings,
    }

    [SerializeField] private GameObject _menuContainer;
    [SerializeField] private GameObject _mainMenuPage;
    [SerializeField] private GameObject _duelPage;
    [SerializeField] private GameObject _leaderboardPage;
    [SerializeField] private GameObject _settingsPage;

    // Main menu page
    [SerializeField] private Button _startDuelingButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    // Duel page
    [SerializeField] private Button _localDuelButton;
    [SerializeField] private Button _onlineDuelButton;
    [SerializeField] private Button _backDuelingButton;
    [SerializeField] private TMP_InputField _snailNameTextField;

    // Leaderboard page
    [SerializeField] private Button _backLeaderboardButton;

    // Settings page
    [SerializeField] private Button _backSettingsButton;

    private string _localSnailName;

    private void Awake()
    {
        _startDuelingButton.onClick.AddListener(OnDuelPressed);
        _leaderboardButton.onClick.AddListener(OnLeaderboardPressed);
        _settingsButton.onClick.AddListener(OnSettingsPressed);
        _exitButton.onClick.AddListener(OnExitPressed);

        _localDuelButton.onClick.AddListener(OnLocalDuelPressed);
        _onlineDuelButton.onClick.AddListener(OnOnlineDuelPressed);
        _backDuelingButton.onClick.AddListener(BackToMainMenu);
        _snailNameTextField.onValueChanged.AddListener(OnSnailNameChanged);

        _localSnailName = PlayerPrefs.GetString("LocalSnailName", "Anonymous Snail");
        _snailNameTextField.SetTextWithoutNotify(_localSnailName);

        _backLeaderboardButton.onClick.AddListener(BackToMainMenu);

        _backSettingsButton.onClick.AddListener(BackToMainMenu);

        SetMenuState(MenuPage.Main);
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
        SetMenuState(MenuPage.Duel);
    }

    private void OnLocalDuelPressed()
    {
        SceneManager.LoadScene("DuelScene");
    }

    private void OnOnlineDuelPressed()
    {
        RegisterPlayerWithDb playerDB = ServiceRegistration.ServiceProvider.GetRequiredService<RegisterPlayerWithDb>();
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

    private void OnLeaderboardPressed()
    {
        SetMenuState(MenuPage.Leaderboard);
    }

    private void OnSettingsPressed()
    {
        SetMenuState(MenuPage.Settings);
    }

    private void OnFullscreenToggle(bool value)
    {
        Screen.fullScreen = value;
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }

    private void BackToMainMenu()
    {
        SetMenuState(MenuPage.Main);
    }

    private void SetMenuState(MenuPage page)
    {
        _mainMenuPage.SetActive(page == MenuPage.Main);
        _duelPage.SetActive(page == MenuPage.Duel);
        _leaderboardPage.SetActive(page == MenuPage.Leaderboard);
        _settingsPage.SetActive(page == MenuPage.Settings);
    }
}
