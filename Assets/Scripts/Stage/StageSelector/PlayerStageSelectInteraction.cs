using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStageSelectInteraction : MonoBehaviour
{
    private StageEntrance currentPlate;   // 현재 발판 참조

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StageEntrance plate))
        {
            currentPlate = plate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StageEntrance plate) && currentPlate == plate)
        {
            currentPlate = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 상호작용 키
        {
            currentPlate?.TryInteract();
        }
    }
}