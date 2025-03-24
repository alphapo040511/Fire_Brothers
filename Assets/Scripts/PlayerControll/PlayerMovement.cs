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
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }
}
