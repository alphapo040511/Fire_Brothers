using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractData
{
    public List<HeldItemType> needItems;
    public int maxProgress;
    public HeldItemType rewardItem;

    [System.NonSerialized]
    public int currentProgress = 0;

    public bool ProgressInteraction()
    {
        currentProgress++;

        Debug.Log($"���� ���൵ : {currentProgress}/{maxProgress}");

        if(currentProgress >= maxProgress )
        {
            Debug.Log("��ȣ�ۿ� �Ϸ�");
            return true;
        }

        return false;
    }
}
