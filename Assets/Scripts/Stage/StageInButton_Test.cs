using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInButton_Test : MonoBehaviour
{
    public int stageIndex;
    public string sceneName;

    public void OnClickJoinStage()
    {
        if(!StageManager.Instance.IsStageUnlocked(stageIndex))
        {
            Debug.LogWarning("잠겨 있는 스테이지 입니다.");
            return;
        }

        StageLoader.nextStageIndex = stageIndex;
        SceneManager.LoadScene(sceneName);
    }
}
