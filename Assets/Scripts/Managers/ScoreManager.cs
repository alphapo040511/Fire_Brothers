using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreType
{
    WaterRefill,        //물탱크
    FireFighting,       //화재 진압
    Rescue              //인명 구조
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float waterRefillScore { get; private set; }                     //물탱크 점수
    public float firefightingScore { get; private set; }                    //화재 진압 점수
    public float rescueScore { get; private set; }                          //인명 구조 점수


    private float decreaseRate = 3f;                                        //매 초 감소할 수치


    public event Action<float, float, float> OnScoreChanged;                //점수 변경 이벤트

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
        firefightingScore = 0;
        rescueScore = 0;
        waterRefillScore = 0;
        OnScoreChanged?.Invoke(waterRefillScore, firefightingScore, rescueScore);
    }

    private IEnumerator ScoreDecrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            waterRefillScore = Mathf.Max(0f, waterRefillScore - decreaseRate);
            firefightingScore = Mathf.Max(0f, firefightingScore - decreaseRate);
            rescueScore = Mathf.Max(0f, rescueScore - decreaseRate);

            OnScoreChanged?.Invoke(waterRefillScore, firefightingScore, rescueScore);
        }
    }

    public void GainScore(ScoreType scoreType, float amount)
    {
        switch (scoreType)
        {
            case ScoreType.WaterRefill:
                waterRefillScore = Mathf.Min(1000f, waterRefillScore + amount);
                break;
            case ScoreType.FireFighting:
                firefightingScore = Mathf.Min(1000f, firefightingScore + amount);
                break;
            case ScoreType.Rescue:
                rescueScore = Mathf.Min(1000f, rescueScore + amount);
                break;
        }

        OnScoreChanged?.Invoke(waterRefillScore, firefightingScore, rescueScore);
    }
}
