using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _streakText;
    [SerializeField] private TMP_Text _rankText;

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;

            if (_nameText != null)
                _nameText.text = value;
        }
    }

    public int _streakCount { get; set; }
    public int StreakCount
    {
        get => _streakCount;
        set
        {
            _streakCount = value;

            if (_streakText != null)
                _streakText.text = $"Best Streak: {value}";
        }
    }

    public int _ranking { get; set; }
    public int Ranking
    {
        get => _ranking;
        set
        {
            _ranking = value;

            if (_rankText != null)
                _rankText.text = $"#{value:00}";
        }
    }
}
