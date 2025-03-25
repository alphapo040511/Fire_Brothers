using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractData", menuName = "ScriptableData/InteractData")]
public class InteractData : ScriptableObject
{
    public string interactObjectName;                   //������Ʈ �̸�
    public Sprite sprite;                               //������Ʈ �̹���
    public List<HeldItemType> needItems;                //��ȣ�ۿ�� �ʿ��� ������
    public int maxProgress;                             //�ִ� �۾�(��ȣ�ۿ�) Ƚ��
    public HeldItemType rewardItem;                     //�۾� �Ϸ� �� ����
    public bool reuseable;                              //���� ��������
    public float reuseDelay;                            //���� ��Ÿ��
}
