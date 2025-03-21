using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public virtual void Interact(PlayerInteraction playerData)          //상호작용시 호출하는 메서드
    {
        if (interactData.interactable == false)
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }

        if (interactData.needItems.Contains(playerData.heldItemType))
        {
            if (interactData.ProgressInteraction())
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

    public virtual void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(interactData.rewardItem);
        //가로수 같이 완료 후 파괴되는 오브젝트는 따로 관리
    }

    void Update()
    {
        if (interactData.reuseable)
        {
            interactData.Timer(Time.deltaTime);
        }
    }
}
