using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactableDistance = 3f;
    public float interactableAngle = 30f;

    public HeldItemType heldItemType;

    private Interactable target;
    private float angleThreshold;

    private float interactDelay = 0.75f;
    private float lastInteractTime;


    void Start()
    {
        angleThreshold = Mathf.Cos(Mathf.Deg2Rad * interactableAngle);
    }

    void Update()
    {
        FindTargetObject();
    }

    private void FindTargetObject()             //��ȣ�ۿ� ������ ������Ʈ�� ã�� �޼���(��ȣ�ۿ� ����� ȣ��� ���� ����)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, interactableDistance, 1 << 3);       //���̾� �߰�

        if (targets == null) return;

        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);

        Vector2 targetPos = Vector2.zero;
        if(target != null)
        {
            targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        }
        float distance = Vector2.Distance(playerPos, targetPos);
        float dot = Vector3.Dot(transform.forward, targetPos - playerPos);

        foreach (var temp in targets)
        {
            if (temp == null || temp == target) return;
            
            Vector2 newPos = new Vector2(temp.transform.position.x, temp.transform.position.z);
            float newDistance = Vector2.Distance(playerPos, newPos);
            float newDot = Vector3.Dot(transform.forward, newPos - playerPos);

            if (newDot > angleThreshold) return;

            if(newDot < dot || newDistance < distance || target == null)
            {
                Interactable interactable = temp.GetComponent<Interactable>();
                if (interactable != null)
                {
                    target = interactable;
                    Debug.Log("���ο� Ÿ�� ����");
                }
            }
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (target == null || lastInteractTime + interactDelay > Time.time) return;

        //������Ʈ�� ��ȣ�ۿ� ������ �Ÿ��� �ִ��� Ȯ��

        lastInteractTime = Time.time;

        target.Interact(this);
    }

    public void CompliteInteractin(HeldItemType itemType)
    {
        heldItemType = itemType;
        Debug.Log($"{itemType}�� �տ� ��� ����");
    }
}
