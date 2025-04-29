using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearTest : MonoBehaviour
{
    [Header("테스트용 스테이지 번호")]
    [SerializeField] private int testStageIndex = 1;

    void Start()
    {
        TestCompleteStage();
    }

    private void TestCompleteStage()
    {
        if (StageManager.Instance == null || ScoreManager.Instance == null)
        {
            Debug.LogError("StageManager 또는 ScoreManager 인스턴스가 존재하지 않습니다.");
            return;
        }

        int scoreToSave = Mathf.RoundToInt(ScoreManager.Instance.currentScore);

        StageManager.Instance.CompleteStage(testStageIndex, scoreToSave);

        Debug.Log($"스테이지 {testStageIndex} 클리어 처리 완료 (점수: {scoreToSave})");
        Debug.Log($"현재 총 별 개수: {StageManager.Instance.GetTotalStars()}");
    }
}

