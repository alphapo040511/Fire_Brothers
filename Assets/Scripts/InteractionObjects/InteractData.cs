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
    public HeldItemType rewardItem;                     //작업 완료 시 보상
    public bool reuseable;                              //재사용 가능한지
    public float reuseDelay;                            //재사용 쿨타임
}
