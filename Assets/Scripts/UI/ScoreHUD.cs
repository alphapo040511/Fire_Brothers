using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    public Image scoreBar;

    public GameObject[] starbar = new GameObject[3];
    public GameObject[] stars = new GameObject[3];

    void Start()
    {
        StageStatsManager.Instance.OnScoreChanged += ChangeScore;
        StageStatsManager.Instance.GetStar += GetStar;
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
        starbar[0].transform.localPosition = Vector3.up * (StageStatsManager.Instance.scoreStarThreshold[0] / 1000) * 600;
        starbar[1].transform.localPosition = Vector3.up * (StageStatsManager.Instance.scoreStarThreshold[1] / 1000) * 600;
        starbar[2].transform.localPosition = Vector3.up * (StageStatsManager.Instance.scoreStarThreshold[2] / 1000) * 600;
    }
}
