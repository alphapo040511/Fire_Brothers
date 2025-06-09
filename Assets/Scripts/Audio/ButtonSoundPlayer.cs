using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Pressed);
    }

    private void Pressed()
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlayShootSound("ButtonPressed");
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
