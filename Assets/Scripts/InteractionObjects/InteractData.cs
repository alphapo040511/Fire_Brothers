using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractData", menuName = "ScriptableData/InteractData")]
public class InteractData : ScriptableObject
{
    public string interactObjectName;                   //오브젝트 이름
    public Sprite sprite;                               //오브젝트 이미지
    public List<HeldItemType> needItems;                //상호작용시 필요한 아이템
    public int maxProgress;                             //최대 작업(상호작용) 횟수
    public HeldItem rewardItem;                     //작업 완료 시 보상
    public bool reuseable;                              //재사용 가능한지
    public float reuseDelay;                            //재사용 쿨타임
    public string usingSound;                           //사용시 사운드
    public string compliteSound;                        //완료 사운드

    public bool CurrentItemChecking(HeldItem itemtype)
    {   
        if(needItems.Contains(HeldItemType.Any))                        //모든 아이템이 가능하다면 그냥 true 리턴
        {
            return true;
        }

        if(itemtype == null)                                            //손에 아이템이 없는 경우
        {
            return (needItems.Contains(HeldItemType.None));             //요구 아이템이 없다면
        }
        else
        {
            return (needItems.Contains(itemtype.itemType));     //해당 아이템들이 조건을 충족 한다면
        }
       
    }
}
