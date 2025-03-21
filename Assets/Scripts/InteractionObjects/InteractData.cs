using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractData
{
    public string interactObjectName;
    public List<HeldItemType> needItems;
    public int maxProgress;
    public HeldItemType rewardItem;
    public bool reuseable;
    public float reuseDelay;

    [System.NonSerialized]
    public int currentProgress = 0;
    public bool interactable = true;

    private float timer;

    public bool ProgressInteraction()
    {
        currentProgress++;

        Debug.Log($"현재 진행도 : {currentProgress}/{maxProgress}");

        if(currentProgress >= maxProgress )
        {
            Debug.Log("상호작용 완료");
            currentProgress = 0;

            interactable = false;

            if (reuseable)
            {
                InitTimer();
            }

            return true;
        }

        return false;
    }

    private void InitTimer()
    {
        timer = reuseDelay;
    }

    public void Timer(float deltaTime)
    {
        if(!interactable)
        {
            timer -= deltaTime;

            if(timer <= 0)
            {
                interactable = true;
            }
        }
    }
}
