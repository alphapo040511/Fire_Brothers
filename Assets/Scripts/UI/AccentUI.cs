using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccentUI : MonoBehaviour
{


    private Transform targetPosition;

    public void Initialize(Transform worldTransform)
    {
        targetPosition = worldTransform;
    }

    void Update()
    {
        Vector3 pos = targetPosition.position;
        pos.y += 1.5f;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        screenPos.y += 50;
        transform.position = screenPos;
    }

    public void Complite()
    {
        Destroy(gameObject);
    }
}
