using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : Interactable
{
    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);
        Debug.Log("ȭ�� ���� �Ϸ�");
        //���� ���� ���� �߰� � ���
    }
}
