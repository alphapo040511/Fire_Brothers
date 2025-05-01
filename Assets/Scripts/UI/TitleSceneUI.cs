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

        //추후 메인 메뉴 상태일때로 변경
        if(gameState == GameState.Playing)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
