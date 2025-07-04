using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerInteraction : MonoBehaviour
{
    public Transform pivot;

    public float interactableDistance = 3f;
    public float interactableAngle = 90f;

    public HeldItem heldItem;

    private Interactable target;
    private float angleThreshold;

    private float interactDelay = 0.5f;
    private float lastInteractTime;

    private bool pressedInteractionButton;

    private PlayerMovement playerMovement;

    void Start()
    {
        angleThreshold = Mathf.Cos(Mathf.Deg2Rad * interactableAngle * 0.5f);
        playerMovement = GetComponent<PlayerMovement>();
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
        if (context.control.device != playerMovement.inputDevice || GameManager.Instance.CurrentState != GameState.Playing) return;

        pressedInteractionButton = context.performed;

        if (pressedInteractionButton && target != null)
        {
            playerMovement.OnPlayerStatsChange(PlayerStats.Interacting);
        }
        else if(GameManager.Instance.CurrentState == GameState.Playing)
        {
            playerMovement.OnPlayerStatsChange(PlayerStats.Controllable);
        }
        else
        {
            playerMovement.OnPlayerStatsChange(PlayerStats.Idle);
        }
    }

    public void GetNewItem(HeldItem itemType)
    {
        if(heldItem != null)
        {
            heldItem.BreakItem();
        }

        SoundManager.instance.PlayShootSound("Pickup");

        heldItem = itemType;
        Debug.Log($"{itemType}를 손에 듦");

        if(playerMovement != null)
        {
            playerMovement.SetIK(heldItem.leftGrib, heldItem.rightGrib);
        }

        playerMovement.OnPlayerStatsChange(PlayerStats.Controllable);
    }

    public void GetNewItem()
    {
        if (heldItem != null)
        {
            heldItem.BreakItem();
            heldItem = null;
        }

        if (playerMovement != null)
        {
            playerMovement.RemoveIK();
        }

        playerMovement.OnPlayerStatsChange(PlayerStats.Controllable);
    }

    public void DropHeldItem(InputAction.CallbackContext context)
    {
        if (context.control.device != playerMovement.inputDevice || GameManager.Instance.CurrentState != GameState.Playing) return;

        if (heldItem != null)
        {
            heldItem.Drop();
            heldItem = null;
        }

        if(playerMovement != null)
        {
            playerMovement.RemoveIK();
        }
    }

    private void FindTargetObject()
    {
        if (pivot == null) return;

        Collider[] targets = Physics.OverlapSphere(pivot.position, interactableDistance, 1 << 3);       //레이어 추가

        if (targets.Length <= 0)
        {
            target = null;
            return;
        }

        Vector3 playerPos = PlanePosition(pivot.position);

        Vector3 targetPos = Vector3.zero;


        foreach (var temp in targets)
        {
            if (temp == target) return;

            if (target != null)
            {
                targetPos = PlanePosition(target.transform.position);
            }
            float dot = Vector3.Dot(pivot.forward, (targetPos - playerPos).normalized);

            Vector3 newPos = PlanePosition(temp.ClosestPoint(transform.position));

            float newDot = Vector3.Dot(pivot.forward, (newPos - playerPos).normalized);

            if (newDot < angleThreshold) return;                                //탐색 각도 밖에 있다면 리턴

            if (newDot > dot || target == null)
            {
                Interactable interactable = temp.GetComponent<Interactable>();
                if (interactable != null)
                {
                    target = interactable;
                    //Debug.Log($"새로운 타겟");
                }
            }
        }
    }

    private void Intertaction()
    {
        if (pressedInteractionButton && target != null && playerMovement.playerStats == PlayerStats.Interacting)
        {
            if (lastInteractTime + interactDelay > Time.time) return;

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
