using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeldItemType
{
    None,          // 아무것도 안 든 상태
    BucketEmpty,   // 비어있는 양동이
    BucketFull,    // 가득찬 양동이
    Axe,           // 도끼
    Extinguisher,   // 소화기
    Tree,            // 가로수
    Ambulance,       // 구급차
    Firstaidkit,    // 구급키트
    HealedPerson,   // 치료된 사람
}
