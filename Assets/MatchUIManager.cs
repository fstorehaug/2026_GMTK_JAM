using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Microsoft.Extensions.DependencyInjection;

public enum MatchUIState
{
    Waiting,
    Dueling,
    RoundOver,
}

public class MatchUIManager : MonoBehaviour
{
    public MatchUIState UIState;

    [SerializeField] private bool _local;

    [SerializeField] private GameObject _waitingUI;
    [SerializeField] private GameObject _versusUI;
    [SerializeField] private GameObject _roundOverUI;
    [SerializeField] private GameObject _leftSnailShotUI;
    [SerializeField] private GameObject _rightSnailShotUI;

    [SerializeField] private TMP_Text _versusNameLeft;
    [SerializeField] private TMP_Text _versusNameRight;

    [SerializeField] private TMP_Text _leftSnailNameText;
    [SerializeField] private TMP_Text _leftSnailTimeText;
    [SerializeField] private TMP_Text _rightSnailNameText;
    [SerializeField] private TMP_Text _rightSnailTimeText;

    [SerializeField] private Button _waitingMainMenuButton;

    [SerializeField] private Button _rematchButton;
    [SerializeField] private Button _findNewDuelButton;
    [SerializeField] private Button _roundOverMainMenuButton;

    private float _timeVersusShown;
    private float _timeToShowVersus = 2f;

    private void Awake()
    {
        _waitingMainMenuButton.onClick.AddListener(OnMainMenuPressed);
    
        _rematchButton.onClick.AddListener(OnRematchPressed);
        _findNewDuelButton.onClick.AddListener(OnFindNewDuelPressed);
        _roundOverMainMenuButton.onClick.AddListener(OnMainMenuPressed);

        _rematchButton.gameObject.SetActive(_local);
        _findNewDuelButton.gameObject.SetActive(!_local);
    }

    private void Update()
    {
        if (_versusUI.activeSelf && Time.timeSinceLevelLoad > _timeVersusShown + _timeToShowVersus)
            _versusUI.SetActive(false);
    }

    private void OnRematchPressed()
    {
        ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<ScopeService>().RenewScope();
        SceneManager.LoadScene("LocalDuelScene");
    }

    private void OnFindNewDuelPressed()
    {
        // TODO: Find a new match

        SceneManager.LoadScene("DuelScene");
        ServiceRegistration.ServiceProvider.ServiceProvider.GetRequiredService<ScopeService>().RenewScope();
    }

    private void OnMainMenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetUIState(MatchUIState state)
    {
        UIState = state;

        if (_waitingUI == null || _roundOverUI == null || _versusUI == null) return;

        _waitingUI.SetActive(UIState == MatchUIState.Waiting);
        _roundOverUI.SetActive(UIState == MatchUIState.RoundOver);

        if (UIState == MatchUIState.Dueling)
        {
            // Show vs on a timer.
            _timeVersusShown = Time.timeSinceLevelLoad;
            _versusUI.SetActive(true);
        }
    }

    public void SetLeftSnailName(string snailName)
    {
        _leftSnailNameText.text = snailName;
        _versusNameLeft.text = snailName;
    }

    public void SetRightSnailName(string snailName)
    {
        _rightSnailNameText.text = snailName;
        _versusNameRight.text = snailName;
    }

    public void SetLeftShootTime(string time)
    {
        _leftSnailShotUI.SetActive(true);
        _leftSnailTimeText.text = time;
    }

    public void SetRightShootTime(string time)
    {
        _rightSnailShotUI.SetActive(true);
        _rightSnailTimeText.text = time;
    }
}
