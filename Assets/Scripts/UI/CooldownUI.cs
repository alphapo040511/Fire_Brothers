using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public Image itemImage;
    public Image background;

    private Transform targetPosition;

    public void Initialize(Sprite sprite, Transform worldTransform)
    {
        targetPosition = worldTransform;
        itemImage.sprite = sprite;
        background.sprite = sprite;
        itemImage.fillAmount = 1;
    }

    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(targetPosition.position);
        screenPos.y += 100;
        transform.position = screenPos;
    }

    public void UpdateCooltime(float percent)
    {
        itemImage.fillAmount = percent;
    }
}
