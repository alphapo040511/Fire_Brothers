using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static int nextStageIndex = -1;

    private void Start()
    {
        if (nextStageIndex >= 0)
        {
            LevelDataConverter.LoadLevelDataAddressable(this, nextStageIndex);
            Debug.Log($"스테이지 {nextStageIndex} 로드 시작");
            nextStageIndex = -1;
        }
    }
}
