using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static string nextScene;
    public GameObject toGameScene;
    public GameObject toCustomScene;

    public Image loadingBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Start()
    {
        UIManager.Instance.HideScreen();
        if (nextScene == "CustomizeScene")
        {
            toCustomScene.SetActive(true);
        }
        else
        {
            toGameScene.SetActive(true);
        }
        GameManager.Instance.ChangeState(GameState.Loading);
        StartCoroutine(LoadSceneAsync(nextScene));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            loadingBar.fillAmount = operation.progress;
            yield return null;
        }

        loadingBar.fillAmount = operation.progress;

        yield return new WaitForSeconds(0.5f);
        operation.allowSceneActivation = true;
    }
}
