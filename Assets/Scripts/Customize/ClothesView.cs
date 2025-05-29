using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothesView : MonoBehaviour
{
    public ClothesType clothesType;
    public Image centerItem;
    public Image preItem;
    public Image nextItem;

    public void UpdateItems(Sprite currnet, Sprite pre, Sprite next)
    {
        centerItem.sprite = currnet;
        preItem.sprite = pre;
        nextItem.sprite = next;
    }
}
