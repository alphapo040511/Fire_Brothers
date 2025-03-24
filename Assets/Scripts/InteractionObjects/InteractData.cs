using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractData", menuName = "ScriptableData/InteractData")]
public class InteractData : ScriptableObject
{
    public string interactObjectName;
    public List<HeldItemType> needItems;
    public int maxProgress;
    public HeldItemType rewardItem;
    public bool reuseable;
    public float reuseDelay;
}
