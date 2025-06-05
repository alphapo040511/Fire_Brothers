using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothesViewData
{
    public Sprite image;
    public bool useble;
    public int starCount;
    public ClothesViewData(Sprite sprite, bool useble, int unlockStar)
    {
        image = sprite;
        this.useble = useble;
        starCount = unlockStar;
    }
}

public class ClothesView : MonoBehaviour
{
    public ClothesType clothesType;
    public Image centerItem;
    public Image preItem;
    public Image nextItem;
    public GameObject lockedImage;
    public GameObject lockedPre;
    public GameObject lockedNext;
    public TextMeshProUGUI starText;
    public TextMeshProUGUI starPre;
    public TextMeshProUGUI starNext;

    public void UpdateItems(ClothesViewData data)
    {
        centerItem.sprite = data.image;

        lockedImage.SetActive(!data.useble);
        starText.text = data.starCount.ToString();
    }

    public void UpdatePre(ClothesViewData data)
    {
        preItem.sprite = data.image;

        lockedPre.SetActive(!data.useble);
        starPre.text = data.starCount.ToString();
    }

    public void UpdateNext(ClothesViewData data)
    {
        nextItem.sprite = data.image;

        lockedNext.SetActive(!data.useble);
        starNext.text = data.starCount.ToString();
    }
}
