using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum PlayerStats
{
    Idle,         // 기본 대기 상태 (움직이지 않음, 입력 대기)
    Controllable, // 조작 가능 (이동, 상호작용 가능)
    Interacting   // 상호작용 중 (조작 불가)
}

public class PlayerMovement : MonoBehaviour
{
    public float minValue = 0.1f;
    public float moveSpeed = 5.0f;
    public InputDevice inputDevice;
    public int playerIndex;

    public PlayerInput playerInput;
    private Animator m_Animator;

    private Rigidbody rb;

    private Vector3 moveDirection;

    private Transform leftHandTarget = null;
    private Transform rightHandTarget = null;

    public PlayerStats playerStats { get; private set; } = PlayerStats.Controllable;

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

        m_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        m_Animator.SetFloat("State", 1);
        m_Animator.SetFloat("Hor", 0);

        inputDevice = InputDeviceManager.Instance.FindDevice(playerIndex);
        Debug.Log($"{playerIndex} 플레이어 {inputDevice}와 연결됨");
    }

    void Update()
    {
        if (playerStats != PlayerStats.Controllable)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;

            if (moveDirection != Vector3.zero)
            {
                // 이동 방향을 향하도록 회전
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        m_Animator.SetFloat("Vert", rb.velocity.magnitude);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.control.device != inputDevice)
        {
            return;
        }

        // 카메라의 Y축 회전만 반영해서 이동 방향 계산
        Vector3 forward = Vector3.left;
        Vector3 right = Vector3.forward;

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
        if (device == inputDevice)
        {
            if (change == InputDeviceChange.Removed)
            {
                inputDevice = null;
                GameManager.Instance.ChangeState(GameState.Paused);
            }
            else if (change == InputDeviceChange.Added)
            {
                inputDevice = InputDeviceManager.Instance.FindDevice(playerIndex);
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

    public void OnGameStatsChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Ready:
            case GameState.Paused:
            case GameState.GameOver:
                playerStats = PlayerStats.Idle;
                playerInput.enabled = false;
                break;

            case GameState.Playing:
                playerStats = PlayerStats.Controllable;
                playerInput.enabled = true;
                break;

        }
    }

    public void OnPlayerStatsChange(PlayerStats newStats)
    {
        playerStats = newStats;
    }

    public void GamePause(InputAction.CallbackContext context)
    {
        UIManager.Instance.ShowScreen(ScreenType.Pause);
    }
}
