using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Transform pivot;

    public float interactableDistance = 3f;
    public float interactableAngle = 90f;

    public HeldItemType heldItemType;

    private Interactable target;
    private float angleThreshold;

    private float interactDelay = 0.5f;
    private float lastInteractTime;

    private bool pressedAButton;

    void Start()
    {
        angleThreshold = Mathf.Cos(Mathf.Deg2Rad * interactableAngle * 0.5f);
    }

    void Update()
    {
        Intertaction();
    }

    private void FixedUpdate()
    {
        FindTargetObject();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        pressedAButton = context.performed;
    }

    public void CompliteInteractin(HeldItemType itemType)
    {
        heldItemType = itemType;
        Debug.Log($"{itemType}를 손에 듦");
    }

    private void FindTargetObject()
    {
        Collider[] targets = Physics.OverlapSphere(pivot.position, interactableDistance, 1 << 3);       //레이어 추가

        if (targets.Length <= 0)
        {
            target = null;
            return;
        }

        Vector3 playerPos = PlanePosition(pivot.position);

        Vector3 targetPos = Vector3.zero;
        if (target != null)
        {
            targetPos = PlanePosition(target.transform.position);
        }
        float dot = Vector3.Dot(pivot.forward, (targetPos - playerPos).normalized);

        foreach (var temp in targets)
        {
            if (temp == target) return;

            Vector3 newPos = PlanePosition(temp.transform.position);

            float newDot = Vector3.Dot(pivot.forward, (newPos - playerPos).normalized);

            if (newDot < angleThreshold) return;                                //탐색 각도 밖에 있다면 리턴

            if (newDot > dot || target == null)
            {
                Interactable interactable = temp.GetComponent<Interactable>();
                if (interactable != null)
                {
                    target = interactable;
                    Debug.Log($"새로운 타겟 [{interactable.interactData.interactObjectName}]");
                }
            }
        }
    }

    private void Intertaction()
    {
        if (pressedAButton)
        {
            if (target == null || lastInteractTime + interactDelay > Time.time) return;

            lastInteractTime = Time.time;

            target.Interact(this);
        }
    }

    private Vector3 PlanePosition(Vector3 pos)
    {
        Vector3 temp = pos;
        temp.y = transform.position.y;
        return temp;
    }
}
