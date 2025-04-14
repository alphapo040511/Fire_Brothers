using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInteractable : Interactable
{
    public int currentProgress = 0;



    public override void Interact(PlayerInteraction playerData)          //상호작용시 호출하는 메서드
    {
        if (interactable == false)
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }

        if (interactData.CurrentItemChecking(playerData.heldItem))
        {
            if (ProgressInteraction())
            {
                Complite(playerData);
            }
        }
        else
        {
            //상호 작용이 안될 경우 애니메이션 등을 관리 해야하니 false 리턴을 통해 진행 안됨을 표시하도록 추가
            Debug.LogWarning("적절한 아이템을 들고 있지 않습니다.");
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

            if (interactData.reuseable)
            {
                timer = 0;
            }

            currentProgress = 0;
            interactable = false;

            return true;
        }

        return false;
    }

    public override void Complite(PlayerInteraction playerData)
    {
        HeldItem item = Instantiate(interactData.rewardItem, GenPosition(playerData.transform.position), Quaternion.identity);
        item.Handling(playerData.pivot);
        playerData.CompliteInteractin(interactData.rewardItem);
    }

}
