using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public HeldItemType itemType;
    public Vector3 childPosition;
    public Vector3 childRotation;

    public Rigidbody rb;
    public Collider _collider;


    public void Handling(Transform parents)
    {
        rb.useGravity = false;
        _collider.enabled = false;
        transform.SetParent(parents);
        transform.localPosition = childPosition;
        transform.localEulerAngles = childRotation;
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.useGravity = true;
        _collider.enabled = true;
    }

    public void BreakItem()
    {
        Destroy(gameObject);    //일단 오브젝트 파괴로 진행
    }
}
