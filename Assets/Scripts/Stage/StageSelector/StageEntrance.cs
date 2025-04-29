using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEntrance : MonoBehaviour
{
    public int stageIndex;      // 발판과 연결된 스테이지 번호
    public string sceneName;    // 이동할 씬 이름 (Inspector에서 설정)

    private bool isPlayerOnPlate = false;   // 플레이어가 발판 위에 있는가?
    private int interactionCount = 0;       // 상호작용 횟수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlate = true;
            interactionCount = 0;   // 올라갈 때마다 초기화
            Debug.Log("발판에 올라갔습니다. 상호작용 2회 시 입장, 1회 시 정보 표시");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlate = false;
            interactionCount = 0;   // 내려갈 때마다 초기화
            Debug.Log("발판에서 내려갔습니다.");
        }
    }

    public void TryInteract()
    {
        if (!isPlayerOnPlate) return;

        // StageManager에서 해금 여부 확인
        if (!StageManager.Instance.IsStageUnlocked(stageIndex))
        {
            Debug.LogWarning("잠겨 있는 스테이지입니다.");
            return;
        }

        interactionCount++;

        if (interactionCount == 1)
        {
            Debug.Log($"스테이지 {stageIndex}가 선택되었습니다. 다시 상호작용 시 입장합니다.");
        }
        else if (interactionCount >= 2)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("씬 이름이 설정되지 않았습니다.");
                return;
            }

            StageLoader.nextStageIndex = stageIndex;
            SceneManager.LoadScene(sceneName);
        }
    }
}
