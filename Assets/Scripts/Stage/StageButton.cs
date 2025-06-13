using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public List<ParticleSystem> effects = new List<ParticleSystem>();

    public int stageIndex;
    public string sceneName;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI[] scoresText = new TextMeshProUGUI[3];
    public GameObject[] stars = new GameObject[3];

    private bool isUnlocked = false;

    void Start()
    {
        if(!StageManager.Instance.IsStageClaer(stageIndex))
        {
            GetComponent<Button>().interactable = false;
        }
        
    }

    public void Entrance()
    {
        // StageManager에서 해금 여부 확인
        if (!StageManager.Instance.IsStageUnlocked(stageIndex))
        {
            Debug.LogWarning("잠겨 있는 스테이지입니다.");
            return;
        }


        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다.");
            return;
        }

        GameManager.Instance.CurrentStageSetting(stageIndex);
        //StageLoader.nextStageIndex = stageIndex;
        SceneLoader.LoadScene(sceneName);
    }

    
    public void ShowStageInfo(int index, Transform target)
    {
        if(StageManager.Instance != null)
        {
            int[] targetScores = StageManager.Instance.GetStarThreshold(index);
            int starCount = StageManager.Instance.GetStageBestStars(index);
            int highScore = StageManager.Instance.GetHighScore(index);

            highScoreText.text = $"Best : {highScore}";
            for (int i = 0; i < 3; i++)
            {
                scoresText[i].text = targetScores[i].ToString();
                stars[i].SetActive(i < starCount);
            }
        }
    }
}
