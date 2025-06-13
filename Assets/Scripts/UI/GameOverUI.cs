using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using System;

public class GameOverUI : MonoBehaviour
{
    public GameObject ResultCanvas;
    public Button quitButton;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Image[] stars = new Image[3];
    public TextMeshProUGUI[] scoresText = new TextMeshProUGUI[3];

    public Animator player_1;
    public Animator player_2;

    private bool isWorking = false;
    private bool animationDone = false;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += ChangeGameState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeGameState;
    }

    private void Start()
    {
        int[] targetScores = StageManager.Instance.GetStarThreshold(GameManager.Instance.currentStageIndex);

        for (int i = 0; i < 3; i++)
        {
            scoresText[i].text = targetScores[i].ToString();
        }
    }

    public void ChangeGameState(GameState state)
    {
        if(state == GameState.GameOver && !isWorking)
        {
            StartCoroutine(ShowScore());
        }
    }

    private void Update()
    {
        if(animationDone)
        {
            if (EventSystem.current.currentSelectedGameObject == null && quitButton != null)
            {
                // 마우스 클릭이 아닌 경우에만 감지
                if (Keyboard.current.anyKey.wasPressedThisFrame ||
                    Gamepad.current != null && Gamepad.current.allControls.Any(c => c is ButtonControl btn && btn.wasPressedThisFrame))
                {
                    EventSystem.current.SetSelectedGameObject(quitButton.gameObject);
                    Debug.Log("포커스 재설정");
                }
            }
        }
    }

    public IEnumerator ShowScore()
    {
        isWorking = true;

        ResultCanvas.SetActive(true);

        bool isClear = StageManager.Instance.IsStageClaer(GameManager.Instance.currentStageIndex);
        string trigger = isClear ? "Success" : "Fail";

        player_1.SetTrigger(trigger);
        player_2.SetTrigger(trigger);

        int score = StageStatsManager.Instance.currentScore;
        Debug.Log(score);
        int playTime = StageStatsManager.Instance.playTime;
        int[] starThreshold = StageManager.Instance.GetStarThreshold();

        if (scoreText != null)
        {
            int i = 0; 

            while(i <= score || i <= playTime)
            {
                scoreText.text = $"Scroe : {Mathf.Min(i, score)}";
                for (int j = 0; j < starThreshold.Length; j++)
                {
                    if (stars[j] != null && i >= starThreshold[j])
                    {
                        stars[j].gameObject.SetActive(true);
                    }
                }

                int sumTime = Mathf.Min(i, playTime);

                // TimeSpan으로 변환
                TimeSpan time = TimeSpan.FromSeconds(sumTime);

                // minutes, seconds 바로 접근 가능
                int minutes = time.Minutes;
                int seconds = time.Seconds;

                timeText.text = time.ToString(@"mm\:ss");
                i++;
                yield return null;
            }
        }

        yield return new WaitForSecondsRealtime(1);

        quitButton.interactable = true;
        isWorking = false;
        animationDone = true;
    }

    public void Quit()
    {
        SceneLoader.LoadScene("StageSelectScene");
    }
}
