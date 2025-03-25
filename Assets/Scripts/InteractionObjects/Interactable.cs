using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public int currentProgress = 0;
    public bool interactable = true;


    private CooldownUI coodownUI;
    private ProgressUI progressUI;
    private float timer;

    void Start()
    {
        if(interactData.reuseable)
        {
            coodownUI = GameSceneUIManger.instance.CreatingCooldownUI(interactData.sprite, transform);
        }
    }

    void Update()
    {
        if (interactData.reuseable)
        {
            Timer(Time.deltaTime);
        }
    }

    public virtual void Interact(PlayerInteraction playerData)          //��ȣ�ۿ�� ȣ���ϴ� �޼���
    {
        if (interactable == false)
        {
            Debug.Log("�� �̻� ����� �� �����ϴ�.");
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
            //��ȣ �ۿ��� �ȵ� ��� �ִϸ��̼� ���� ���� �ؾ��ϴ� false ������ ���� ���� �ȵ��� ǥ���ϵ��� �߰�
            Debug.LogWarning("������ �������� ��� ���� �ʽ��ϴ�.");
        }
    }

    public bool ProgressInteraction()
    {
        if(progressUI == null && interactData.maxProgress > 0)
        {
            progressUI = GameSceneUIManger.instance.CreatingProgressUI(transform);
        }

        currentProgress++;

        Debug.Log($"���� ���൵ : {currentProgress}/{interactData.maxProgress}");

        if (progressUI != null)
        {
            progressUI.UpdateProgess((float)currentProgress / (float)interactData.maxProgress);
        }

        if (currentProgress >= interactData.maxProgress)
        {
            Debug.Log("��ȣ�ۿ� �Ϸ�");

            if (progressUI != null)
            {
                Destroy(progressUI.gameObject);
                progressUI = null;
            }

            if (interactData.reuseable)
            {
                InitTimer();
            }

            currentProgress = 0;
            interactable = false;

            return true;
        }

        return false;
    }

    public virtual void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(interactData.rewardItem);
        //���μ� ���� �Ϸ� �� �ı��Ǵ� ������Ʈ�� ���� ����(�̺�Ʈ�� �ϴ°͵� ����ϵ�)
    }

    private void InitTimer()
    {
        timer = 0;
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
