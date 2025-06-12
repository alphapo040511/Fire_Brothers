using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TempLoader : MonoBehaviour
{
    public Image loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.unscaledDeltaTime;
            loadingBar.fillAmount = 0.9f + time / 1f;
            yield return null;
        }

        GameManager.Instance.ChangeState(GameState.Ready);
    }
}
