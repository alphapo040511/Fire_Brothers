using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInteractable : Interactable
{
    private CooldownUI coodownUI;

    private ProgressUI progressUI;

    private int currentProgress = 0;
    private float timer;

    private void Start()
    {
        if (interactData.reuseable)
        {
            coodownUI = GameSceneUIManger.instance?.CreatingCooldownUI(interactData.sprite, transform);
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
        if (interactable == false || interactData == null)              //사용 불가능 할거나 데이터가 없을 때
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }
        else
        {
            if (!interactData.CurrentItemChecking(playerData.heldItem)) //사용 조건이 안 맞을 때
            {
                return;
            }
        }

        if (interactData.maxProgress > 0)
        {
            if (ProgressInteraction())
            {
                Complite(playerData);
            }
        }
        else
        {
            Complite(playerData);
        }
    }

    public override void Complite(PlayerInteraction playerData)
    {
        IInteractionEffect effect = GetComponent<IInteractionEffect>();

        if (effect != null)
        {
            effect.OnInteractComplete();
        }

        if (interactData.rewardItem != null)
        {
            HeldItem item = Instantiate(interactData.rewardItem, GenPosition(playerData.transform.position), Quaternion.identity);
            item.Handling(playerData.pivot);
            playerData.GetNewItem(item);
        }
        else
        {
            playerData.GetNewItem();
        }
        interactable = false;
    }

    public bool ProgressInteraction()
    {
        if (progressUI == null && interactData.maxProgress > 0)
        {
            progressUI = GameSceneUIManger.instance.CreatingProgressUI(transform);
        }

        currentProgress++;

        Debug.Log($"현재 진행도 : {currentProgress}/{interactData.maxProgress}");

        if (progressUI != null)
        {
            progressUI.UpdateProgess((float)currentProgress / (float)interactData.maxProgress);
        }

        if (currentProgress >= interactData.maxProgress)
        {
            Debug.Log("상호작용 완료");

            if (progressUI != null)
            {
                Destroy(progressUI.gameObject);
                progressUI = null;
            }

            currentProgress = 0;
            interactable = false;

            return true;
        }

        return false;
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
                timer = 0;
            }
        }
    }

    public Vector3 GenPosition(Vector3 playerPostion)
    {
        Vector3 dir = (playerPostion - transform.position).normalized;

        return dir * 0.75f + playerPostion;
    }
}
