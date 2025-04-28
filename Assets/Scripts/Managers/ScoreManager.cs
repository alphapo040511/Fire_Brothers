using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float currentScore { get; private set; }                     //현재 점수

    private float[] scoreStarThreshold = new float[3] { 100, 150, 300}; //각 별을 획득할 경계
    private float minScore = 0;                                         //별 획득시 감소를 막을 수치

    private float decreaseRate = 3f;                                    //매 초 감소할 수치


    public event Action<float> OnScoreChanged;                          //점수 변경 이벤트

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
            yield return new WaitForSeconds(1f);
            currentScore = Mathf.Max(minScore, currentScore - decreaseRate);

            OnScoreChanged?.Invoke(currentScore);
        }
    }

    public void GainScore(float amount)
    {
        currentScore = Mathf.Min(1000f, currentScore + amount);

        for(int i = scoreStarThreshold.Length - 1; i >= 0; i--)
        {
            if(currentScore >= scoreStarThreshold[i])
            {
                minScore = scoreStarThreshold[i];
                Debug.Log($"{i + 1}번째 별 획득");
                break;
            }
        }

        OnScoreChanged?.Invoke(currentScore);
    }
}
