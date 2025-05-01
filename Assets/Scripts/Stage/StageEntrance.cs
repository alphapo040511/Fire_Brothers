using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEntrance : MonoBehaviour
{
    public int stageIndex;
    public string sceneName;
    public void Entrance()
    {
        // StageManager에서 해금 여부 확인
        if (!StageManager.Instance.IsStageUnlocked(stageIndex))
        {
            Debug.LogWarning("잠겨 있는 스테이지입니다.");
            return;
        }


        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다.");
            return;
        }

        GameManager.Instance.CurrentStageSetting(stageIndex);
        StageLoader.nextStageIndex = stageIndex;
        SceneManager.LoadScene(sceneName);
    }
}
