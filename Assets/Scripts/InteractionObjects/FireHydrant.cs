using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : Interactable
{
    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);
        Debug.Log("화재 진압 완료");
        //점수 관련 내용 추가 등에 사용
    }
}
