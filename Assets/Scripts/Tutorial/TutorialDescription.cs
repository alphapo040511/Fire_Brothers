using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class TutorialDescription : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI descriptionText;

    public List<TutorialVideoData> videoDatas = new List<TutorialVideoData>();

    private int index = 0;
    private bool isContinuable = false;

    void Start()
    {
        if (videoDatas.Count > 0)
        {
            ShowDescription(index);
        }
    }

    public void OnEnable()
    {
        if (videoDatas.Count > 0)
        {
            index = 0;
            ShowDescription(index);
        }
        else
        {
            Close();
        }
    }

    public void Next()
    {
        if (isContinuable)
        {
            isContinuable = false;
            index = (int)Mathf.Repeat(index + 1, videoDatas.Count);
            ShowDescription(index);
        }
        else
        {
            isContinuable = true;
        }
    }

    public void Pre()
    {
        if (isContinuable)
        {
            isContinuable = false;
            index = (int)Mathf.Repeat(index - 1, videoDatas.Count);
            ShowDescription(index);
        }
        else
        {
            isContinuable = true;
        }
    }

    public void Close()
    {
        UIManager.Instance.HideScreen();
    }


    private void ShowDescription(int index)
    {
        videoPlayer.clip = videoDatas[index].videoClip;
        videoPlayer.Play();
        descriptionText.text = videoDatas[index].description;
        StartCoroutine(CountinueTimer(videoDatas[index].description.Length));
    }

    private IEnumerator CountinueTimer(int length)
    {
        yield return new WaitForSecondsRealtime(length * 0.1f);
        isContinuable = true;
    }
}

