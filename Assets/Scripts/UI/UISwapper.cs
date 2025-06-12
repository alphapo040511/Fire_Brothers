using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UISwapper : MonoBehaviour
{
    public GameObject canvasA;
    public GameObject canvasB;

    private float fadeTime = 1f;
    private float showTime = 1f;

    private void Start()
    {
        StartCoroutine(FadeLoop());
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            canvasA.SetActive(true);
            yield return new WaitForSecondsRealtime(showTime);
            canvasA.SetActive(false);

            canvasB.SetActive(true);
            yield return new WaitForSecondsRealtime(showTime);
            canvasB.SetActive(false);
        }
    }


}

