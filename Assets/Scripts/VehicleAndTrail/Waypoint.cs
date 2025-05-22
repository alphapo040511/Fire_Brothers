using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isAccessible = true;

    public void UnlockPath()
    {
        isAccessible = true;
        PlayableObjectsManager.Instance.AccessibleChecking();
    }
}
