using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    public Image waterRefillScore;
    public Image firefightingScore;
    public Image rescueScore;


    void Start()
    {
        ScoreManager.Instance.OnScoreChanged += ChangeScore;
    }

    void OnDestroy()
    {
        ScoreManager.Instance.OnScoreChanged -= ChangeScore;
    }

    private void ChangeScore(float water, float fire, float rescue)
    {
        if (waterRefillScore != null) { waterRefillScore.fillAmount = water / 1000f; }
        if (firefightingScore != null) { firefightingScore.fillAmount = fire / 1000f; }
        if (rescueScore != null) { rescueScore.fillAmount = rescue / 1000f; }
    }
}
