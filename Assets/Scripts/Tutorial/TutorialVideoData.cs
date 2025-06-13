using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class TutorialVideoData
{
    public VideoClip videoClip;

    [TextArea] public string description;
}
