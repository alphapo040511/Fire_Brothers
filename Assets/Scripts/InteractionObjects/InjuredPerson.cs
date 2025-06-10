using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredPerson : ProgressInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetInteger("Type", Random.Range(0, 2));
    }

    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);

        if(StageStatsManager.Instance != null)
        {
            StageStatsManager.Instance.GainScore(60);
        }

        //사라지는 효과 추가
    }
}
