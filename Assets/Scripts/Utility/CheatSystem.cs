using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MessegeType
{
    Normal,
    System,
    Complite,
    Warning,
    Error
}

public class CheatSystem : MonoBehaviour
{
    public static CheatSystem Instance { get; private set; }

    public Transform logTransform;
    public TextMeshProUGUI textPrefs;

    public TMP_InputField commandInput;

    private Dictionary<string, System.Action<string[]>> commands;

    private void OnEnable()
    {
        commandInput.Select();
        commandInput.ActivateInputField();
        commandInput.text = "";
        Log("/Help - 사용 가능한 명령어 목록을 봅니다.", MessegeType.System);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCommands();
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, System.Action<string[]>>()
        {
            { "/help", ShowHelp },
            { "/allclear", AllClear },
            { "/clearlog", ClearLog },
            { "/decreaseset", Decrease }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ExecuteCommand();
        }
    }

    private void ExecuteCommand()
    {
        if (commandInput == null || string.IsNullOrEmpty(commandInput.text)) return;

        string command = commandInput.text.Trim().ToLower();
        string[] parts = command.Split(' ');

        if (commands.ContainsKey(parts[0]))
        {
            commands[parts[0]](parts);
        }
        else if (parts[0][0] == '/')
        {
            Log($"알수 없는 명령어 : {parts[0]}", MessegeType.Error);
        }
        else
        {
            Log(commandInput.text, MessegeType.Normal);
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void ShowHelp(string[] args)
    {
        Log("---명령어 목록---", MessegeType.System);
        Log("/AllClear - 모든 스테이지 클리어", MessegeType.System);
        Log("/ClearLog - 로그 정리", MessegeType.System);
        Log("/DecreaseSet {time} {rate} - 점수 감소율 설정", MessegeType.System);
    }

    private void AllClear(string[] args)
    {
        StageManager.Instance.AllClear();
        Log("모든 스테이지 클리어 처리 완료", MessegeType.Complite);
    }

    private void ClearLog(string[] args)
    {
        foreach(Transform child in logTransform)
        {
            Destroy(child.gameObject);
        }
        Log("로그 정리됨", MessegeType.Complite);
    }

    private void Decrease(string[] args)
    {
        string command = commandInput.text.Trim().ToLower();
        string[] parts = command.Split(' ');

        if(parts.Length < 3)                        //내용이 빠져 있는 경우
        {
            Log("Time과 Rate를 입력 해 주십시오.", MessegeType.Error);
            return;
        }

        float time;
        if(!float.TryParse(parts[1], out time))
        {
            Log("Time을 적용 할 수 없습니다.", MessegeType.Error);
            return;
        }
        else if(time <= 0)
        {
            Log("Time은 0 보다 커야합니다.", MessegeType.Error);
            return;
        }

        int rate;
        if(!int.TryParse(parts[2], out rate))
        {
            Log("Rate을 적용 할 수 없습니다.", MessegeType.Error);
            return;
        }
        else if(rate <= 0)
        {
            Log("Rate가 음수입니다. 점수가 증가 할 수 있습니다.", MessegeType.Warning);
        }

        if(StageStatsManager.Instance != null)
        {
            StageStatsManager.Instance.DecreaseSetting(time, rate);
            Log($"현재 스테이지의 점수 감소 시간 {time}, 감소량 {rate}으로 변경 되었습니다.", MessegeType.Complite);
        }
        else
        {
            Log("해당 명령어는 스테이지 진입 후 사용 가능합니다.", MessegeType.Warning);
        }
    }


    private void Log(string text, MessegeType messegeType = MessegeType.System)
    {
        TextMeshProUGUI textBox = Instantiate(textPrefs, logTransform);
        string messege = "[System] ";
        Color color;
        
        switch(messegeType)
        {
            case MessegeType.Normal:
                color = Color.white;
                messege = "[Player] ";
                break;
            case MessegeType.System:
                color = Color.gray;
                break;
            case MessegeType.Complite:
                color = Color.green;
                break;
            case MessegeType.Warning:
                color = Color.yellow;
                break;
            case MessegeType.Error:
                color = Color.red;
                break;
            default:
                color = Color.gray;
                break;
        }
        textBox.color = color;
        textBox.text = messege + text;
    }
}
