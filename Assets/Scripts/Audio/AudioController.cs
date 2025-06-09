using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                       //UI
using UnityEngine.Audio;                    //Audio
using UnityEngine.EventSystems;

public enum AudioType
{
    Master,
    BGM,
    SFX
}

public class AudioController : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private AudioMixer audioMixer;                 //private 선언한 것들을 인스팩터 창에서 보여지게

    [SerializeField] private Slider MusicMasterSlider;              //UI Slider
    [SerializeField] private Slider MusicBGMSlider;                 //UI Slider
    [SerializeField] private Slider MusicSFXSlider;                 //UI Slider

    //슬라이더 Minvalue을 0.001

    private void Awake()
    {
        MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);                  //UI Slider의 값이 변경 되었을 경우 SetMasterVolume 함수를 호출 한다.
        MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);                        //UI Slider의 값이 변경 되었을 경우 SetBGMVolume 함수를 호출 한다.
        MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);                        //UI Slider의 값이 변경 되었을 경우 SetSFXVolume 함수를 호출 한다.
    }


    private void Start()
    {
        Debug.Log("사운드 데이터 로드 시작");

        if (DataManager.Instance != null)
        {
            SetVolume(AudioType.Master);
            SetVolume(AudioType.BGM);
            SetVolume(AudioType.SFX);
            MusicMasterSlider.value = DataManager.Instance.gameData.masterVolume;
            MusicBGMSlider.value = DataManager.Instance.gameData.bgmVolume;
            MusicSFXSlider.value = DataManager.Instance.gameData.sfxVolume;

            Debug.Log("사운드 데이터 로드 완료");
        }
    }

    public void SetVolume(AudioType type)
    {
        float volume = 0;
        string groupName = "Master";

        switch(type)
        {
            case AudioType.Master:
                volume = DataManager.Instance.gameData.masterVolume;
                groupName = "Master";
                break;
            case AudioType.BGM:
                volume = DataManager.Instance.gameData.bgmVolume;
                groupName = "BGM";
                break;
            case AudioType.SFX:
                volume = DataManager.Instance.gameData.sfxVolume;
                groupName = "SFX";
                break;
        }

        if (volume == 0)
        {
            audioMixer.SetFloat(groupName, 0);
            return;
        }
        audioMixer.SetFloat(groupName, Mathf.Log10(volume / 10) * 20);                //볼륨에서의 0 ~ 1 <- Mathf.Log10(volume) * 20

        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlayShootSound("SoundCheck_" + groupName);
        }
    }

    public void SetMasterVolume(float volume)
    {
        if(DataManager.Instance != null)
        {
            DataManager.Instance.SetVolume(AudioType.Master, volume);
            SetVolume(AudioType.Master);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SetVolume(AudioType.BGM, volume);
            SetVolume(AudioType.BGM);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SetVolume(AudioType.SFX, volume);
            SetVolume(AudioType.SFX);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayShootSound("ButtonSelected");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayShootSound("ButtonSelected");
        }
    }
}
