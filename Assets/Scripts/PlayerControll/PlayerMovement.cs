using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float minValue = 0.1f;
    public float moveSpeed = 5.0f;

    private Animator m_Animator;

    private Rigidbody rb;

    private Vector3 moveDirection;

    private Transform leftHandTarget = null;
    private Transform rightHandTarget = null;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += DisconnectDevice;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= DisconnectDevice;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        m_Animator.SetFloat("State", 1);
        m_Animator.SetFloat("Hor", 0);
    }

    void Update()
    {
        rb.velocity = moveDirection * moveSpeed;

        if (moveDirection == Vector3.zero) return;

        // 이동 방향을 향하도록 회전
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 카메라의 Y축 회전만 반영해서 이동 방향 계산
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Y 축 회전은 무시하고, 수평 방향만 사용
        forward.y = 0;
        right.y = 0;

        Vector3 direction = forward * context.ReadValue<Vector2>().y + right * context.ReadValue<Vector2>().x;
        direction.Normalize();
        if (direction.magnitude > minValue)
        {
            moveDirection = direction.normalized;

            m_Animator.SetFloat("Vert", direction.magnitude);
        }
        else
        {
            moveDirection = Vector3.zero;
            m_Animator.SetFloat("Vert", 0);
        }
    }

    public void DisconnectDevice(InputDevice device, InputDeviceChange change)
    {
        foreach (InputDevice inputDevice in GetComponent<PlayerInput>().devices)
        {
            if (device == inputDevice && change == InputDeviceChange.Removed)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetIK(Transform left, Transform right)
    {
        leftHandTarget = left;
        rightHandTarget = right;
    }

    public void RemoveIK()
    {
        leftHandTarget = null;
        rightHandTarget = null;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (m_Animator == null) return;

        ApplyIK(AvatarIKGoal.LeftHand, leftHandTarget);
        ApplyIK(AvatarIKGoal.RightHand, rightHandTarget);
    }

    private void ApplyIK(AvatarIKGoal goal, Transform target)
    {
        if (target != null)
        {
            m_Animator.SetIKPositionWeight(goal, 1f);
            m_Animator.SetIKRotationWeight(goal, 1f);

            m_Animator.SetIKPosition(goal, target.position);
            m_Animator.SetIKRotation(goal, target.rotation);
        }
        else
        {
            m_Animator.SetIKPositionWeight(goal, 0f);
            m_Animator.SetIKRotationWeight(goal, 0f);
        }
    }
}
