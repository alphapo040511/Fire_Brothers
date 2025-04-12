using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInteractable : Interactable
{
    public InteractData interactData;

    public int currentProgress = 0;
    public bool interactable = true;


    private CooldownUI coodownUI;
    private ProgressUI progressUI;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        if (interactData.reuseable)
        {
            coodownUI = GameSceneUIManger.instance.CreatingCooldownUI(interactData.sprite, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactData.reuseable)
        {
            Timer(Time.deltaTime);
        }
    }

    public override void Interact(PlayerInteraction playerData)          //상호작용시 호출하는 메서드
    {
        if (interactable == false)
        {
            Debug.Log("더 이상 사용할 수 없습니다.");
            return;
        }

        if (interactData.needItems.Contains(playerData.heldItemType))
        {
            if (ProgressInteraction())
            {
                Complite(playerData);
            }
        }
        else
        {
            //상호 작용이 안될 경우 애니메이션 등을 관리 해야하니 false 리턴을 통해 진행 안됨을 표시하도록 추가
            Debug.LogWarning("적절한 아이템을 들고 있지 않습니다.");
        }
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

            if (interactData.reuseable)
            {
                timer = 0;
            }

            currentProgress = 0;
            interactable = false;

            return true;
        }

        return false;
    }

    public override void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(interactData.rewardItem);
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
}
