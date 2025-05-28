using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // 게임 설정
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
    public bool isMuted;

    //플레이어 커스터마이징 정보
    public List<CustomizeData> playersCustomData = new List<CustomizeData> {
        new CustomizeData(),
        new CustomizeData()
    };
}
