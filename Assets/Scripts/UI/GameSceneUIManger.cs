using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManger : MonoBehaviour
{
    public static GameSceneUIManger instance;

    void Awake()
    {
        instance = this;
    }

    public Transform canvas;

    public CooldownUI cooldownUIPrefab;
    public ProgressUI progressUIPrefab;

    public CooldownUI CreatingCooldownUI(Sprite sprite, Transform worldTransform)
    {
        CooldownUI temp = Instantiate(cooldownUIPrefab, canvas);
        temp.Initialize(sprite, worldTransform);
        return temp;
    }

    public ProgressUI CreatingProgressUI(Transform worldTransform)
    {
        ProgressUI temp = Instantiate(progressUIPrefab, canvas);
        temp.Initialize(worldTransform);
        return temp;
    }
}
