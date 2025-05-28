using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoPresenter : MonoBehaviour
{
    public static StageInfoPresenter Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public StageInfoView stageInfoView;

    public void ShowStageInfo(int index, Transform target)
    {
        if(StageManager.Instance != null)
        {
            int[] targetScores = StageManager.Instance.GetStarThreshold(index);
            int starCount = StageManager.Instance.GetStageBestStars(index);
            int highScore = StageManager.Instance.GetHighScore(index);

            stageInfoView.UpdateData(targetScores, starCount, highScore, target);
            stageInfoView.Show();
        }
    }

    public void HideInfo()
    {
        stageInfoView.Hide();
    }

}
