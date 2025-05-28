using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageInfoView : MonoBehaviour
{
    public TextMeshProUGUI[] scoresText = new TextMeshProUGUI[3];
    public GameObject[] stars = new GameObject[3];
    public TextMeshProUGUI hightScoreText;

    private Transform targetTransform;

    private void Update()
    {
        if (targetTransform != null)
        {
            Vector3 pos = targetTransform.position;
            pos.y += 7.5f;

            Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
            transform.position = screenPos;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false); 
    }

    public void UpdateData(int[] targetScores, int starCount, int highScore, Transform target)
    {
        for(int i = 0; i < 3; i++)
        {
            scoresText[i].text = targetScores[i].ToString();
            stars[i].SetActive(i < starCount);
        }

        hightScoreText.text = $"HIGH SCORE : {highScore}";

        targetTransform = target;
    }
}
