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
    public AccentUI accentUIPrefab;
    public ProgressUI progressUIPrefab;
    public PlayerPositionUI playerPositionUIPrefab;

    public CooldownUI CreatingCooldownUI(Sprite sprite, Transform worldTransform)
    {
        CooldownUI temp = Instantiate(cooldownUIPrefab, canvas);
        temp.Initialize(sprite, worldTransform);
        return temp;
    }

    public AccentUI CreatingaccentUI(Transform worldTransform)
    {
        AccentUI temp = Instantiate(accentUIPrefab, canvas);
        temp.Initialize(worldTransform);
        return temp;
    }

    public ProgressUI CreatingProgressUI(Transform worldTransform)
    {
        ProgressUI temp = Instantiate(progressUIPrefab, canvas);
        temp.Initialize(worldTransform);
        return temp;
    }

    public void CreatingPlayerUI(int playerIndex, Transform worldTransform, PlayerMovement player)
    {
        PlayerPositionUI temp = Instantiate(playerPositionUIPrefab, canvas);
        temp.Initialize(playerIndex, worldTransform, player);
    }
}
