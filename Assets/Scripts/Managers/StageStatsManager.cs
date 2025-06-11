using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatsManager : MonoBehaviour
{
    public static StageStatsManager Instance { get; private set; }

    public int currentScore { get; private set; }               //현재 점수

    public int[] scoreStarThreshold = new int[3];               //각 별을 획득할 기준
    private int minScore = 0;                                   //별 획득시 감소를 막을 수치

    private int decreaseRate = 7;                               //매 초 감소할 수치

    private int currenStarCount = 0;

    public event Action<float> OnScoreChanged;                  //점수 변경 이벤트
    public event Action<int> GetStar;                           //점수 변경 이벤트

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
    }

    void Start()
    {
        InitScore();
        StartCoroutine(ScoreDecrease());
        scoreStarThreshold = StageManager.Instance.GetStarThreshold();
        Debug.Log($"이번 스테이지의 클리어 조건 {scoreStarThreshold[0]}, {scoreStarThreshold[1]}, {scoreStarThreshold[2]}");
        GameManager.Instance.ChangeState(GameState.Ready);
    }


    private void InitScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    private IEnumerator ScoreDecrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            currentScore = Mathf.Max(minScore, currentScore - decreaseRate);

            OnScoreChanged?.Invoke(currentScore);
        }
    }

    public void GainScore(int amount)
    {
        currentScore = Mathf.Min(1000, currentScore + amount);

        for(int i = scoreStarThreshold.Length - 1; i >= 0; i--)
        {
            if (i < currenStarCount) return;

            if(currentScore >= scoreStarThreshold[i])
            {
                minScore = scoreStarThreshold[i];
                Debug.Log($"{i + 1}번째 별 획득");
                currenStarCount = i + 1;
                SoundManager.instance.PlayShootSound("GetStar");
                GetStar?.Invoke(i + 1);
                break;
            }
        }

        OnScoreChanged?.Invoke(currentScore);
    }

    public void GameEnd()
    {
        if (StageManager.Instance.CompleteStage(GameManager.Instance.currentStageIndex, currentScore))
        {
            Debug.Log($"스테이지 {GameManager.Instance.currentStageIndex} 클리어 처리 완료 (점수: {currentScore})");
            Debug.Log($"현재 총 별 개수: {StageManager.Instance.GetTotalStars()}");
        }
        else
        {
            //게임 오버 연출
        }

        GameManager.Instance.ChangeState(GameState.GameOver);
    }
}
