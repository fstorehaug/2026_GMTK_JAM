using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum MatchUIState
{
    Waiting,
    Dueling,
    RoundOver,
}

public class MatchUIManager : MonoBehaviour
{
    public MatchUIState UIState;
    // private MatchUIState _uIState;
    // public MatchUIState UIState
    // {
    //     get => _uIState;
    //     set
    //     {
    //         if (value == _uIState) return;

    //         _uIState = value;
            
    //         _waitingUI.SetActive

    //     }
    // }

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

    [SerializeField] private Button _findNewDuelButton;
    [SerializeField] private Button _roundOverMainMenuButton;

    private float _timeVersusShown;
    private float _timeToShowVersus = 3f;

    private void Awake()
    {
        _waitingMainMenuButton.onClick.AddListener(OnMainMenuPressed);
    
        _findNewDuelButton.onClick.AddListener(OnFindNewDuelPressed);
        _roundOverMainMenuButton.onClick.AddListener(OnMainMenuPressed);
    }

    void Update()
    {
        if (_versusUI.activeSelf && Time.timeSinceLevelLoad > _timeVersusShown + _timeToShowVersus)
            _versusUI.SetActive(false);
    }

    private void OnFindNewDuelPressed()
    {
        SceneManager.LoadScene("DuelScene");
    }

    private void OnMainMenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetUIState(MatchUIState state)
    {
        UIState = state;

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
        _leftSnailTimeText.text = time;
    }

    public void SetRightShootTime(string time)
    {
        _rightSnailTimeText.text = time;
    }
}
