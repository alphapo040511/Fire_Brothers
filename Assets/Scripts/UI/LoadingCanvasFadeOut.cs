using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LoadingCanvasFadeOut : MonoBehaviour
{
    public float fadeTime = 1.0f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        canvasGroup.alpha = 1;
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.unscaledDeltaTime / fadeTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
