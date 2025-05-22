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
        Vector3 pos = targetPosition.position;
        pos.y += 1.5f;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);

        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x >= 0 && viewportPos.x <= 1 &&
                         viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isVisible) //화면 안에 있는 경우
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
            transform.position = screenPos;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateProgess(float percent)
    {
        itemImage.fillAmount = percent;
    }
}
