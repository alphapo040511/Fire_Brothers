using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public bool interactable = true;

    protected CooldownUI coodownUI;
    protected ProgressUI progressUI;
    protected float timer;

    void Start()
    {
        if (interactData == null) return;       //클래스 상속 재정리 필요

        if (interactData.reuseable)
        {
            coodownUI = GameSceneUIManger.instance.CreatingCooldownUI(interactData.sprite, transform);
        }
    }

    void Update()
    {
        if (interactData == null) return;

        if (interactData.reuseable)
        {
            Timer(Time.deltaTime);
        }
    }

    public abstract void Interact(PlayerInteraction playerData);          //상호작용시 호출하는 메서드

    public abstract void Complite(PlayerInteraction playerData);

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
