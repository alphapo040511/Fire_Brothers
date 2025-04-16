using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InstantInteractable : Interactable
{
    private void Start()
    {
        if (interactData.reuseable)
        {
            coodownUI = GameSceneUIManger.instance.CreatingCooldownUI(interactData.sprite, transform);
        }
    }

    private void Update()
    {
        if (interactData.reuseable)
        {
            Timer(Time.deltaTime);
        }
    }

    public override void Interact(PlayerInteraction playerData)
    {
        base.Interact(playerData);
        Complite(playerData);
    }

    public override void Complite(PlayerInteraction playerData)
    {
        HeldItem item = Instantiate(interactData.rewardItem, GenPosition(playerData.transform.position), Quaternion.identity);
        item.Handling(playerData.pivot);
        playerData.GetNewItem(item);
    }

    public void Timer(float deltaTime)
    {
        if (!interactable)
        {
            timer += deltaTime;

            coodownUI.UpdateCooltime(timer / interactData.reuseDelay);

            if (timer >= interactData.reuseDelay)
            {
                interactable = true;
            }
        }
    }

    public Vector3 GenPosition(Vector3 playerPostion)
    {
        Vector3 dir = (playerPostion - transform.position).normalized;

        return dir * 0.75f + playerPostion;
    }
}
