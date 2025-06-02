using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ClothesPresenter : MonoBehaviour
{
    public int playerIndex;
    public CharacterMeshDB meshDB;
    public List<ClothesView> clothesViews = new List<ClothesView>();
    public RectTransform selectBox;

    public float duration = 0.5f;

    private ClothesModel model = new ClothesModel();
    //private Dictionary<ClothesType, ClothesView> views;

    private int currentVIewIndex = 0;
    private RectTransform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        InitializeModel();
        InitializeViews();

        targetTransform = clothesViews[currentVIewIndex].GetComponent<RectTransform>();

        for(int i = 0; i < clothesViews.Count; i++)
        {
            UpdateView(i);
        }
    }

    private void InitializeModel()
    {
        if (DataManager.Instance != null)
        {
            CustomizeData data = DataManager.Instance.gameData.playersCustomData[playerIndex];

            for (int i = 0; i < data.customizeData.Count; i++)
            {
                model.index.Add(data.customizeData[i].type,data.customizeData[i].index);  //현재 플레이어의 커스터마이징 인덱스 저장
            }
        }
    }

    private void InitializeViews()
    {
        //for (int i = 0; i < clothesViews.Count; i++)
        //{
            //views[clothesViews[i].clothesType] = clothesViews[i];
        //}
    }


    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            selectBox.anchoredPosition = Vector2.Lerp(selectBox.anchoredPosition, targetTransform.anchoredPosition, Time.deltaTime / duration);
        }
    }

    //위 아래 조절
    public void ChangeClothesType(InputAction.CallbackContext context)
    {
        if (context.started && context.control.device == InputDeviceManager.Instance.InputDevices[playerIndex])
        {
            float value = context.ReadValue<float>();

            Debug.Log(value);

            if (value == 0) return;

            int index = currentVIewIndex;

            if (value == 1)
            {
                index = (int)Mathf.Repeat(index + 1, clothesViews.Count);
            }
            else if (value == -1)
            {
                index = (int)Mathf.Repeat(index - 1, clothesViews.Count);
            }

            currentVIewIndex = index;
            targetTransform = clothesViews[currentVIewIndex].GetComponent<RectTransform>();
        }
    }

    //다음 옷으로 변경
    public void NextClothes(InputAction.CallbackContext context)
    {
        if (context.started && context.control.device == InputDeviceManager.Instance.InputDevices[playerIndex])
        {
            int currentIndex = model.index[clothesViews[currentVIewIndex].clothesType];
            model.index[clothesViews[currentVIewIndex].clothesType] = (int)Mathf.Repeat(currentIndex + 1, meshDB.meshList[clothesViews[currentVIewIndex].clothesType].Count);
            UpdateView();
        }
    }

    //이전 옷으로 변경
    public void PreClothes(InputAction.CallbackContext context)
    {
        if (context.started && context.control.device == InputDeviceManager.Instance.InputDevices[playerIndex])
        {
            int currentIndex = model.index[clothesViews[currentVIewIndex].clothesType];
            model.index[clothesViews[currentVIewIndex].clothesType] = (int)Mathf.Repeat(currentIndex - 1, meshDB.meshList[clothesViews[currentVIewIndex].clothesType].Count);
            UpdateView();
        }
    }

    private void UpdateView()
    {
        int index = model.index[clothesViews[currentVIewIndex].clothesType];

        ClothesType type = clothesViews[currentVIewIndex].clothesType;

        Sprite current = meshDB.meshList[type][index].sprite;
        Sprite pre = meshDB.meshList[type][(int)Mathf.Repeat(index - 1, meshDB.meshList[type].Count)].sprite;
        Sprite next = meshDB.meshList[type][(int)Mathf.Repeat(index + 1, meshDB.meshList[type].Count)].sprite;

        clothesViews[currentVIewIndex].UpdateItems(current, pre, next);
    }

    private void UpdateView(int viewIndex)
    {
        int index = model.index[clothesViews[viewIndex].clothesType];

        ClothesType type = clothesViews[viewIndex].clothesType;

        Sprite current = meshDB.meshList[type][index].sprite;
        Sprite pre = meshDB.meshList[type][(int)Mathf.Repeat(index - 1, meshDB.meshList[type].Count)].sprite;
        Sprite next = meshDB.meshList[type][(int)Mathf.Repeat(index + 1, meshDB.meshList[type].Count)].sprite;

        clothesViews[viewIndex].UpdateItems(current, pre, next);
    }
}
