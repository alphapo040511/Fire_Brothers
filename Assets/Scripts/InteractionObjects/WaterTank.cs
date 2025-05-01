using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour, IInteractionEffect
{
    public void OnInteractComplete()
    {
        StageStatsManager.Instance.GainScore(150);
    }
}
