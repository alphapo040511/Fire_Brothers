using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPositionUI : MonoBehaviour
{
    public GameObject UI;
    public List<GameObject> playerDisplay = new List<GameObject>();

    private PlayerMovement player;
    private Transform targetPosition;

    public void Initialize(int playerIndex, Transform worldTransform, PlayerMovement playermovement)
    {
        targetPosition = worldTransform;
        if (playerIndex < playerDisplay.Count)
        {
            playerDisplay[playerIndex].SetActive(true);
        }

        UI.SetActive(false);

        player = playermovement;
    }

    void Update()
    {
        Vector3 pos = targetPosition.position;
        pos.y += 1.5f;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);

        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x >= 0 && viewportPos.x <= 1 &&
                         viewportPos.y >= 0 && viewportPos.y <= 1;

        if (!isVisible)                         //화면 밖에 있는 경우
        {
            UI.SetActive(true);

            Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            float x = Mathf.Clamp(screenPos.x, 35, screenWidth - 35);
            float y = Mathf.Clamp(screenPos.y, 35, screenHeight - 35);
            
            transform.position = new Vector3(x, y, 0);

            if(player != null)
            {
                player.outOfView = true;
            }
        }
        else                                    //화면 안에 있는 경우
        {
            UI.SetActive(false);

            if (player != null)
            {
                player.outOfView = false;
            }
        }
    }
}
