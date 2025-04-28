using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour, IInteractionEffect
{
    public void OnInteractComplete()
    {
        ScoreManager.Instance.GainScore(150f);
    }
}
