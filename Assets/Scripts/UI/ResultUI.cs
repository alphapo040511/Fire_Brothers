using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject ResultCanvas;

    public TextMeshProUGUI scoreText;
    public Image[] stars = new Image[3];

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += ChangeGameState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeGameState;
    }

    public void ChangeGameState(GameState state)
    {
        if(state == GameState.GameOver)
        {
            ShowScore();
        }
    }

    public void ShowScore()
    {
        ResultCanvas.SetActive(true);

        int score = StageStatsManager.Instance.currentScore;

        int starCount = StageManager.Instance.CalculateStars(score, StageManager.Instance.GetStarThreshold());

        if(scoreText != null)
        {
            scoreText.text = $"Scroe : {score}";
        }

        for(int i = 0; i < starCount; i++)
        {
            if (stars[i] != null)
            {
                stars[i].gameObject.SetActive(true);
            }
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("StageSelectScene");

        GameManager.Instance.ChangeState(GameState.Playing);
    }
}
