using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractData interactData;

    public int currentProgress = 0;
    public bool interactable = true;

    private float timer;

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

    public virtual void Complite(PlayerInteraction playerData)
    {
        playerData.CompliteInteractin(interactData.rewardItem);
        //���μ� ���� �Ϸ� �� �ı��Ǵ� ������Ʈ�� ���� ����(�̺�Ʈ�� �ϴ°͵� ����ϵ�)
    }

    void Update()
    {
        if (interactData.reuseable)
        {
            Timer(Time.deltaTime);
        }
    }

    public bool ProgressInteraction()
    {
        currentProgress++;

        Debug.Log($"���� ���൵ : {currentProgress}/{interactData.maxProgress}");

        if (currentProgress >= interactData.maxProgress)
        {
            Debug.Log("��ȣ�ۿ� �Ϸ�");
            currentProgress = 0;

            interactable = false;

            if (interactData.reuseable)
            {
                InitTimer();
            }

            return true;
        }

        return false;
    }

    private void InitTimer()
    {
        timer = interactData.reuseDelay;
    }

    public void Timer(float deltaTime)
    {
        if (!interactable)
        {
            timer -= deltaTime;

            if (timer <= 0)
            {
                interactable = true;
            }
        }
    }
}
