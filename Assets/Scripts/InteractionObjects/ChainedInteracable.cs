using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedInteracable : ProgressInteractable
{
    public Collider nextCollider;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);
        _collider.enabled = false;
        nextCollider.enabled = true;
    }
}
