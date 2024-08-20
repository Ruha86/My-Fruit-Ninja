using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public DifficultyChanger DifficultyChanger;
    private TMP_Text _scoreText;
    private int _score;
    private const string BestScoreKey = "BestScore";
    private int _bestScore;
    private bool _isNewBestScore;

    private void Start()
    {
        FillComponents();
        SetScore(0);
        LoadBestScore();
    }

    private void FillComponents()
    {
        _scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddScore(int value) 
    {
        SetScore(_score + value);
    }

    private void SetScore(int value)
    {
        _score = value;
        SetScoreText(value);
        DifficultyChanger.SetDifficultByScore(value);
    }

    private void SetScoreText(int value)
    {
        _scoreText.text = "Очки: " + value;
    }

    public int GetScore()
    {
        return _score;
    }

    public void Restart() 
    {
        SetScore(0);
    }

    public int GetBestScore() 
    {
        return _bestScore;
    }

    public void SetBestScore(int value)
    {
        _bestScore = value;
        SaveBestScore(value);
    }

    private void LoadBestScore() 
    {
        _bestScore = PlayerPrefs.GetInt(BestScoreKey);
    }

    private void SaveBestScore(int value)
    {
        PlayerPrefs.SetInt(BestScoreKey, value);
    }
}
