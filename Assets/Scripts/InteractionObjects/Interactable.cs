using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public bool interactable = true;

    public HeldItem heldItem;
    protected CooldownUI coodownUI;
    protected ProgressUI progressUI;
    protected float timer;

    void Start()
    {
        if (interactData != null)
        { 
            heldItem = interactData.rewardItem; 
        }
    }


    public virtual void Interact(PlayerInteraction playerData)          //상호작용시 호출하는 메서드
    {
        if (interactable == false || interactData == null)              //사용 불가능 할거나 데이터가 없을 때
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }
        else                                                    
        { 
            if (!interactData.CurrentItemChecking(playerData.heldItem)) //사용 조건이 안 맞을 때
            {
                return;
            }

            if (interactData.reuseable)
            {
                timer = 0;
            }
        }
    }

    public virtual void Complite(PlayerInteraction playerData)
    {
        if (heldItem != null)
        {
            playerData.GetNewItem(heldItem);
        }
    }
}
