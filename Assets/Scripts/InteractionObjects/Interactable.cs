using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public virtual void Interact(PlayerInteraction playerData)          //��ȣ�ۿ�� ȣ���ϴ� �޼���
    {
         if (interactData.needItems.Contains(playerData.heldItemType))
        {
            if (interactData.ProgressInteraction())
            {
                Complite(playerData);
            }
        }
        else
        {
            //��ȣ �ۿ��� �ȵ� ��� �ִϸ��̼� ���� ���� �ؾ��ϴ� false ������ ���� ���� �ȵ��� ǥ���ϵ��� �߰�
            Debug.LogWarning("������ �������� ��� ���� �ʽ��ϴ�.");
        }
    }

    public virtual void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(interactData.rewardItem);
    }
}
