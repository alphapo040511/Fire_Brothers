using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InstantInteractable : Interactable
{

    public override void Interact(PlayerInteraction playerData)
    {
        if (interactable == false)
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }

        if (interactData.CurrentItemChecking(playerData.heldItem))
        {
            if (interactData.reuseable)
            {
                timer = 0;
            }

            interactable = false;

            Complite(playerData);
        }
    }

    public override void Complite(PlayerInteraction playerData)
    {
        HeldItem item = Instantiate(interactData.rewardItem, GenPosition(playerData.transform.position), Quaternion.identity);
        item.Handling(playerData.pivot);
        playerData.CompliteInteractin(item);
    }

}
