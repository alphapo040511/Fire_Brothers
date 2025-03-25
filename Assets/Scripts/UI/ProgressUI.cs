using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    public Image itemImage;
    public Image background;

    private Transform targetPosition;

    public void Initialize(Transform worldTransform)
    {
        targetPosition = worldTransform;
        itemImage.fillAmount = 0;
    }

    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(targetPosition.position);
        screenPos.y += 100;
        transform.position = screenPos;
    }

    public void UpdateProgess(float percent)
    {
        Debug.Log(percent);
        itemImage.fillAmount = percent;
    }
}
