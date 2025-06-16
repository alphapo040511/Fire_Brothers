using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressUIPresenter : MonoBehaviour
{
    public static GameProgressUIPresenter Instance;

    public Slider progress;

    private float totalLength = 0;
    
    private float movedLength = 0;

    private float targetPoint = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyTotalValue(float total)
    {
        totalLength = total;
    }

    public void NewPoint(float addLength)
    {
        targetPoint += addLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPoint == 0 || totalLength == 0)
        {
            progress.value = 0;
            return;
        }

        movedLength = Mathf.Lerp(movedLength, targetPoint, Time.deltaTime * 0.5f);

        progress.value = movedLength / totalLength;
    }
}
