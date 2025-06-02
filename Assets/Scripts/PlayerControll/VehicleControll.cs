using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class VehicleControll : MonoBehaviour
{
    public float minValue = 0.1f;
    public float moveSpeed = 5.0f;
    public InputDevice inputDevice;

    private PlayerInput playerInput;
    private Rigidbody rb;

    private Vector3 moveDirection;


    private void OnEnable()
    {
        InputSystem.onDeviceChange += DisconnectDevice;
        GameManager.Instance.OnGameStateChanged += OnGameStatsChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= DisconnectDevice;
        GameManager.Instance.OnGameStateChanged -= OnGameStatsChange;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero)
        {
            // 이동 방향을 향하도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, t));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //해당 디바이스가 없거나 1번 디바이스가 아닌 경우
        if (InputDeviceManager.Instance.InputDevices[0] != context.control.device) return;

        // 카메라의 Y축 회전만 반영해서 이동 방향 계산
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

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

    public void DisconnectDevice(InputDevice device, InputDeviceChange change)
    {
        if (device == inputDevice && change == InputDeviceChange.Removed && gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public void OnIntertacting(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, 9, 1 << 3);       //레이어 추가

            if (targets.Length <= 0)
            {
                return;
            }

            for(int i =0; i < targets.Length; i++)
            {
                StageEntrance interact = targets[i].GetComponent<StageEntrance>();
                if(interact != null)
                {
                    interact.Entrance();
                    return;
                }
            }
        }
    }

    public void OnGameStatsChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Ready:
            case GameState.Paused:
            case GameState.GameOver:
                playerInput.enabled = false;
                break;

            case GameState.Playing:
                playerInput.enabled = true;
                break;

        }
    }

    public void GamePause(InputAction.CallbackContext context)
    {
        UIManager.Instance.ShowScreen(ScreenType.Pause);
    }
}
