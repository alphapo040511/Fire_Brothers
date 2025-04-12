using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public abstract void Interact(PlayerInteraction playerData);          //상호작용시 호출하는 메서드

    public abstract void Complite(PlayerInteraction playerData);

}
