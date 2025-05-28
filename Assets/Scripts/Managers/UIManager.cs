using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public enum ScreenType
{
    None,
    MainMenu,
    GamePlay,
    Pause,
    GameOver,
    ControllerSet,
}

[System.Serializable]
public class UIScreen
{
    public ScreenType screenType;
    public GameObject screenObject;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private PlayerInput playerInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeScreens();
    }

    [SerializeField] private List<UIScreen> screens = new List<UIScreen>();
    
    private Dictionary<ScreenType, GameObject> screenDictionary = new Dictionary<ScreenType, GameObject>();


    // 현재 활성화된 화면
    public ScreenType CurrentScreen { get; private set; } = ScreenType.None;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        // 초기 화면 설정 (메인 메뉴)
        ShowScreen(ScreenType.ControllerSet);
    }

    private void InitializeScreens()
    {
        screenDictionary.Clear();

        foreach (UIScreen screen in screens)
        {
            screenDictionary[screen.screenType] = screen.screenObject;
            screen.screenObject.SetActive(false);
        }
    }

    public void ShowScreen(ScreenType screenType)
    {
        //기존 화변 비활성화
        if (CurrentScreen != ScreenType.None && screenDictionary.ContainsKey(CurrentScreen))
        {
            screenDictionary[CurrentScreen].SetActive(false);
        }

        if (screenDictionary.ContainsKey(screenType))
        {

            if (screenType == ScreenType.ControllerSet && InputDeviceManager.Instance != null)
            {
                InputDeviceManager.Instance.ResetDevices();
            }

            screenDictionary[screenType].SetActive(true);
            CurrentScreen = screenType;
            playerInput.enabled = true;
            GameManager.Instance.ChangeState(GameState.Paused);
        }
        else
        {
            Debug.LogWarning("Screen " + screenType + " not found in UIManager!");
        }
    }

    public void HideScreen()
    {
        //기존 화변 비활성화
        if (CurrentScreen != ScreenType.None && screenDictionary.ContainsKey(CurrentScreen))
        {
            screenDictionary[CurrentScreen].SetActive(false);
        }

        GameManager.Instance.ChangeState(GameState.Playing);
    }

    public void AddOnScreen(UIScreen newScreen)
    {
        screens.Add(newScreen);
        InitializeScreens();
    }

    public void RemoveAtScreen(UIScreen screen)
    {
        screens.Remove(screen);
        InitializeScreens();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if(CurrentScreen != ScreenType.None && CurrentScreen != ScreenType.ControllerSet)
        {
            HideScreen();
        }
    }
}
