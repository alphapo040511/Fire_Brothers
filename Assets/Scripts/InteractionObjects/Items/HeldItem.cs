using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public HeldItemType itemType;
    public Vector3 childPosition;
    public Vector3 childRotation;

    public Collider _collider;

    public Transform rightGrib;
    public Transform leftGrib;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Handling(Transform parents)
    {
        _collider.enabled = false;
        transform.SetParent(parents);
        transform.localPosition = childPosition;
        transform.localEulerAngles = childRotation;
    }

    public void Drop()
    {
        transform.SetParent(null);
        Vector3 groundPosition= transform.localPosition;
        groundPosition.y = 0;
        transform.position = groundPosition;
        _collider.enabled = true;
    }

    public void BreakItem()
    {
        Destroy(gameObject);    //일단 오브젝트 파괴로 진행
    }
}
