using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : Interactable
{
    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);
        Destroy(gameObject);                    //일단 완료되면 파괴되도록 설정
    }
}
