using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;                        //Audio 관련 기능을 사용하기 위해 추가


[System.Serializable]                           //직렬화 시켜준다. (유니티 안에서 데이터를 인스팩터 창에서 나타나게 보여주는 기능)
public class Sound
{
    public string name;                         //사운드의 이름
    public AudioClip clip;                      //사운드 클립

    [Range(0f, 1f)]                             //인스팩터 화면에서 0 ~ 1사이 값을 선택할 수 있는 슬라이더로 변환
    public float volume = 1.0f;
    [Range(0.1f, 3f)]                           //인스팩터 화면에서 0 ~ 1사이 값을 선택할 수 있는 슬라이더로 변환
    public float pitch = 1.0f;                  //사운드 피치
    public bool loop;                           //반복 재생 여부
    public AudioMixerGroup mixerGroup;          //오디오 믹서 그룹

    [HideInInspector]                           //인스팩터 창에서 안보이게 해주는 기능
    public AudioSource sources;                 //오디오 소스
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;                    //싱글톤 인스턴스 (static은 전역 변수로 올려서 어디서든 접근할 수 있게 해준다.)
    public List<Sound> sounds = new List<Sound>();          //사운드 리스트 관리 (List 자료구조 관리)
    public AudioMixer audioMixer;
    //오디오 믹서 참조

    private string lastSound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      //Scene이 변경되어도 (이 오브젝트는)파괴되지 않는다.
        }
        else
        {
            Destroy(gameObject);                //이미 다른 오브젝트가 있을경우 파괴한다.    클래스를 전역으로 1개 유지시킬수 있다.
        }

        //사운드를 초기화
        foreach (Sound sound in sounds)
        {
            sound.sources = gameObject.AddComponent<AudioSource>();                 //AddComponent는 Class 컴포넌트를 오브젝트에 붙인다.
            sound.sources.clip = sound.clip;
            sound.sources.volume = sound.volume;
            sound.sources.pitch = sound.pitch;
            sound.sources.loop = sound.loop;
            sound.sources.outputAudioMixerGroup = sound.mixerGroup;                 //List에 있는 사운드 객체를 (생성된) <AudioSource>에 1개씩 값을 전달 한다.
        }

    }

    //사운드를 재생하는 함수
    public void PlaySound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);               //사운드 List에서 name을 찾아서 Sound를 찾는다.

        if(soundToPlay != null)
        {
            soundToPlay.sources.Play();                                             //사운드 재생
        }
        else
        {
            Debug.LogWarning("사운드 " + name + " 을 찾을 수 없다.");                //사운드가 없다는 경고 로그
        }
    }

    public void PlayShootSound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);               //사운드 List에서 name을 찾아서 Sound를 찾는다.

        if (soundToPlay != null)
        {
            soundToPlay.sources.PlayOneShot(soundToPlay.clip);                      //사운드 재생
        }
        else
        {
            Debug.LogWarning("사운드 " + name + " 을 찾을 수 없다.");                //사운드가 없다는 경고 로그
        }
    }

    public void PlayLoopSound(string name)
    {
        Sound soundToLast = sounds.Find(sound => sound.name == lastSound);      //마지막 사운드

        if( soundToLast != null )
        {
            soundToLast.sources.Stop();
        }

        Sound soundToPlay = sounds.Find(sound => sound.name == name);               //사운드 List에서 name을 찾아서 Sound를 찾는다.

        if (soundToPlay != null)
        {
            soundToPlay.sources.Play();                                             //사운드 재생
            lastSound = name;
        }
        else
        {
            Debug.LogWarning("사운드 " + name + " 을 찾을 수 없다.");                //사운드가 없다는 경고 로그
        }
    }
}
