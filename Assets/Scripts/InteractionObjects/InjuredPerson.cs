using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredPerson : ProgressInteractable
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //GetComponent<Animator>().SetInteger("Type", Random.Range(0, 2));      //애니메이션이 별로라서..
        GetComponent<Animator>().SetInteger("Type", 1);
    }

    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);

        if(StageStatsManager.Instance != null)
        {
            StageStatsManager.Instance.GainScore(35);
        }

        //사라지는 효과 추가
    }
}
