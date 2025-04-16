using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInteractable : InstantInteractable
{
    public int currentProgress = 0;



    public override void Interact(PlayerInteraction playerData)          //상호작용시 호출하는 메서드
    {
        if (ProgressInteraction())
        {
            Complite(playerData);
        }
    }

    public bool ProgressInteraction()
    {
        if (progressUI == null && interactData.maxProgress > 0)
        {
            progressUI = GameSceneUIManger.instance.CreatingProgressUI(transform);
        }

        currentProgress++;

        Debug.Log($"현재 진행도 : {currentProgress}/{interactData.maxProgress}");

        if (progressUI != null)
        {
            progressUI.UpdateProgess((float)currentProgress / (float)interactData.maxProgress);
        }

        if (currentProgress >= interactData.maxProgress)
        {
            Debug.Log("상호작용 완료");

            if (progressUI != null)
            {
                Destroy(progressUI.gameObject);
                progressUI = null;
            }

            currentProgress = 0;
            interactable = false;

            return true;
        }

        return false;
    }
}
