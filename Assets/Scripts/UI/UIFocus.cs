using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UIFocus : MonoBehaviour
{
    public GameObject firstSelected;
    public GameState targetState = GameState.Paused;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && firstSelected != null && GameManager.Instance.CurrentState == targetState)
        {
            // 마우스 클릭이 아닌 경우에만 감지
            if (Keyboard.current.anyKey.wasPressedThisFrame ||
                Gamepad.current != null && Gamepad.current.allControls.Any(c => c is ButtonControl btn && btn.wasPressedThisFrame))
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
                Debug.Log("포커스 재설정");
            }
        }
    }

    private void OnDisable()
    {
        if(EventSystem.current != null)
        EventSystem.current.SetSelectedGameObject(null);
    }
}
