using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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
        Vector3 pos = targetPosition.position;
        pos.y += 1.5f;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);

        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x >= 0 && viewportPos.x <= 1 &&
                         viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isVisible) //화면 안에 있는 경우
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
            screenPos.y += 100;
            transform.position = screenPos;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCooltime(float percent)
    {
        itemImage.fillAmount = percent;
    }
}
