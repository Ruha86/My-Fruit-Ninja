using UnityEngine;
using TMPro;

public class GameEnder : MonoBehaviour
{
    public Score Score;
    public Health Health;
    public FruitSpawner FruitSpawner;
    public GameObject GameScreen;
    public GameObject GameEndScreen;
    public DifficultyChanger DifficultyChanger;
    public TMP_Text GameEndScoreText;
    public TMP_Text BestScoreText;

    public AudioSource BackgroundMusic;
    public AudioSource GameOverSound;

    public void EndGame()
    {
        FruitSpawner.Stop();
        SwitchScreens(false);
        RefreshScores();
        SwitchMusic(false);
    }

    public void RestartGame()
    {
        DifficultyChanger.Restart();
        Score.Restart();
        Health.Restart();
        FruitSpawner.Restart();
        SwitchScreens(true);
        SwitchMusic(true);
    }

    private void Start()
    {
        SwitchScreens(true);
        SwitchMusic(true);
    }

    private void SwitchScreens(bool isGame)
    {
        GameScreen.SetActive(isGame);
        GameEndScreen.SetActive(!isGame);
    }

    private void SetGameEndScoreText(int value) 
    {
        if (value == 1 || value == 21 || value == 31)
        {
            GameEndScoreText.text = $"�� ������� {value} ����!";
        }
        else if (value == 2 || value == 22 || value == 32 || value == 3 || value == 23 || value == 33 || value == 4 || value == 24 || value == 34)
        {
            GameEndScoreText.text = $"�� ������� {value} ����!";
        }
        else  
        {
            GameEndScoreText.text = $"�� ������� {value} �����!";
        }
        
    }

    private void RefreshScores()
    {
        int score = Score.GetScore();
        int oldBestScore = Score.GetBestScore();

        bool isNewBestScore = CheckNewBestScore(score, oldBestScore);

        SetActiveGameEndScoreText(!isNewBestScore);

        if (isNewBestScore)
        {
            Score.SetBestScore(score);
            SetNewBestScoreText(score);
        }
        else
        {
            SetGameEndScoreText(score);
            SetOldBestScoreText(oldBestScore);
        }
    }

    private bool CheckNewBestScore(int score, int oldBestScore)
    {
        return score > oldBestScore;
    }

    private void SetOldBestScoreText(int value)
    {
        BestScoreText.text = $"������ ���������: {value}";
    }

    private void SetNewBestScoreText(int value)
    {
        BestScoreText.text = $"����� ������: {value}!";
    }

    private void SetActiveGameEndScoreText(bool value)
    {
        GameEndScoreText.gameObject.SetActive(value);
    }

    private void SwitchMusic(bool isGame)
    {
        if (isGame)
        {
            BackgroundMusic.Play();
        }
        else
        {
            BackgroundMusic.Stop();
            GameOverSound.Play();
        }
    }
}
