using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireInteraction : MonoBehaviour, IInteractionEffect
{
    public List<ParticleSystem> fireEffect;
    
    public void OnInteractComplete()
    {
        for(int i = 0; i < fireEffect.Count; i++) 
        {
            fireEffect[i].Stop();
        }

        ScoreManager.Instance.GainScore(150f);
    }
}
