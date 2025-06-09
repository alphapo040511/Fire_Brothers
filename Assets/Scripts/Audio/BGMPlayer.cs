using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public string soundName;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayLoopSound(soundName);
    }

}
