using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Interactable
{
    public HeldItem item;

    public override void Interact(PlayerInteraction playerData)
    {
        if (playerData.heldItem == null)
        {
            Complite(playerData);
        }
    }

    public override void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(item);
        item.Handling(playerData.pivot);
    }
}
