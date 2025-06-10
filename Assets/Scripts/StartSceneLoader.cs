using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    public float fadeTime = 1.5f;
    public float fadeOutTime = 0.75f;
    public string sceneName;

    private CanvasGroup canvasGroup;

    private bool fading = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LogoFadeIn());
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LogoFadeIn()
    {
        yield return new WaitForSecondsRealtime(0.5f);              //잠시 대기

        canvasGroup.alpha = 0;
        float alpha = 0;
        while(alpha < 1)                                    //로고 페이드 인
        {
            alpha += Time.unscaledDeltaTime / fadeTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSecondsRealtime(1.5f);

        alpha = 1;
        while (alpha > 0)                                   //로고 페이드 아웃
        {
            alpha -= Time.unscaledDeltaTime / fadeOutTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 0;

        fading = false;
    }

    private IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while(!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && !fading)
            {
                    asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
