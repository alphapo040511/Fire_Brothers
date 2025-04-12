using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantInteractable : Interactable
{
    public HeldItemType returnItemType;

    public override void Interact(PlayerInteraction playerData)
    {
        Complite(playerData);
    }

    public override void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(returnItemType);
    }
}
