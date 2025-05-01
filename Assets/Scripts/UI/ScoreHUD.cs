using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    public Image scoreBar;


    void Start()
    {
        StageStatsManager.Instance.OnScoreChanged += ChangeScore;
    }

    void OnDestroy()
    {
        StageStatsManager.Instance.OnScoreChanged -= ChangeScore;
    }

    private void ChangeScore(float score)
    {
        if (scoreBar != null) { scoreBar.fillAmount = score / 1000f; }
    }
}
