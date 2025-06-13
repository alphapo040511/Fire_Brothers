using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleSceneUI : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += ChangeGameState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeGameState;
    }

    public void ChangeGameState(GameState gameState)
    {
        Debug.Log("Start에 포커스");
        //추후 메인 메뉴 상태일때로 변경
        if(gameState == GameState.Playing)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log(sceneName + " 씬 로드 시작");
        SceneLoader.LoadScene(sceneName);
    }

    public void Pause()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.ShowScreen(ScreenType.Pause);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
