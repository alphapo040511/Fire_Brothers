using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float minValue = 0.1f;
    public float moveSpeed = 5.0f;

    public CharacterController characterController;

    private Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (moveDirection == Vector3.zero) return;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 이동 방향을 향하도록 회전
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 direction = Vector3.forward * context.ReadValue<Vector2>().y + Vector3.right * context.ReadValue<Vector2>().x;
        direction.Normalize();
        if (direction.magnitude > minValue)
        {
            moveDirection = direction.normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }
}
