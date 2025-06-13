using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem.Controls;

public class GameOverUI : MonoBehaviour
{
    public GameObject ResultCanvas;
    public Button quitButton;

    public TextMeshProUGUI scoreText;
    public Image[] stars = new Image[3];

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

        int[] starThreshold = StageManager.Instance.GetStarThreshold();

        if (scoreText != null)
        {
            for (int i = 0; i <= score; i++)
            {
                scoreText.text = $"Scroe : {i}";
                for (int j = 0; j < starThreshold.Length; j++)
                {
                    if (stars[j] != null && i >= starThreshold[j])
                    {
                        stars[j].gameObject.SetActive(true);
                    }
                }
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
