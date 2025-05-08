using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    public Image scoreBar;

    public RectTransform[] starbar = new RectTransform[3];
    public GameObject[] stars = new GameObject[3];

    void Start()
    {
        StageStatsManager.Instance.OnScoreChanged += ChangeScore;
        StageStatsManager.Instance.GetStar += GetStar;
        scoreBar.fillAmount = 0;
        stars[0].SetActive(false);
        stars[1].SetActive(false);
        stars[2].SetActive(false);
        SetBar();
    }

    void OnDestroy()
    {
        StageStatsManager.Instance.OnScoreChanged -= ChangeScore;
        StageStatsManager.Instance.GetStar -= GetStar;
    }

    private void ChangeScore(float score)
    {
        if (scoreBar != null) { scoreBar.fillAmount = score / 1000f; }
    }

    private void GetStar(int count)
    {
        for(int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
        }
    }

    private void SetBar()
    {
        starbar[0].anchoredPosition = new Vector2(5, (StageStatsManager.Instance.scoreStarThreshold[0] / 1000f) * 600);
        starbar[1].anchoredPosition = new Vector2(5, (StageStatsManager.Instance.scoreStarThreshold[1] / 1000f) * 600);
        starbar[2].anchoredPosition = new Vector2(5, (StageStatsManager.Instance.scoreStarThreshold[2] / 1000f) * 600);
        Debug.Log($"첫번째 별 위치 {new Vector2(5, (StageStatsManager.Instance.scoreStarThreshold[0] / 1000f) * 600)}");
    }
}
