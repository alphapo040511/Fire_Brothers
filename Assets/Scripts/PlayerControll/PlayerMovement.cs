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
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 direction = transform.forward * context.ReadValue<Vector2>().y + transform.right * context.ReadValue<Vector2>().x;
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
