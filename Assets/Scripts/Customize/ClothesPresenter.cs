using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClothesPresenter : MonoBehaviour
{
    public int playerIndex;
    public CharacterMeshDB meshDB;
    public List<ClothesView> clothesViews = new List<ClothesView>();
    public RectTransform selectBox;
    public Image clothesTypeImage;
    public List<Sprite> clothesTypes = new List<Sprite>();

    public float duration = 0.5f;

    public CharacterCustomizer customizer;

    public GameObject apply;
    public GameObject cancel;

    private ClothesModel model = new ClothesModel();
    //private Dictionary<ClothesType, ClothesView> views;

    private int currentVIewIndex = 0;
    private RectTransform targetTransform;

    private bool isDisplaying = false;

    [SerializeField]private float rotateDir;

    // Start is called before the first frame update
    void Start()
    {
        InitializeModel();
        StartCoroutine(InitializeViews());

        targetTransform = clothesViews[currentVIewIndex].GetComponent<RectTransform>();
    }

    private void InitializeModel()
    {
        if (DataManager.Instance != null)
        {
            CustomizeData data = DataManager.Instance.gameData.playersCustomData[playerIndex];

            for (int i = 0; i < data.customizeData.Count; i++)
            {
                model.index.Add(data.customizeData[i].type, data.customizeData[i].index);  //현재 플레이어의 커스터마이징 인덱스 저장
            }
        }
    }

    private IEnumerator InitializeViews()
    {
        for (int i = 0; i < clothesViews.Count; i++)
        {
            UpdateView(i);
            yield return null;
        }

        if (playerIndex == 0)
        {
            GameManager.Instance.ChangeState(GameState.Ready);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            selectBox.anchoredPosition = Vector2.Lerp(selectBox.anchoredPosition, targetTransform.anchoredPosition, Time.deltaTime / duration);
        }

        customizer.transform.Rotate(0, rotateDir * 60f * Time.deltaTime, 0);
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (context.control.device == InputDeviceManager.Instance.InputDevices[playerIndex])
            rotateDir = -context.ReadValue<float>();
    }

    //위 아래 조절
    public void ChangeClothesType(InputAction.CallbackContext context)
    {
        if (context.started && context.control.device == InputDeviceManager.Instance.InputDevices[playerIndex])
        {
            float value = context.ReadValue<float>();

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
            clothesTypeImage.sprite = clothesTypes[index];
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

    private void UpdateView(int viewIndex = -1)
    {
        if(viewIndex < 0) viewIndex = currentVIewIndex;

        int index = model.index[clothesViews[viewIndex].clothesType];

        ClothesType type = clothesViews[viewIndex].clothesType;

        ClothesViewData currnet = GetViewData(type, index);
        clothesViews[viewIndex].UpdateItems(currnet);

        ClothesViewData pre = GetViewData(type, (int)Mathf.Repeat(index - 1, meshDB.meshList[type].Count));
        clothesViews[viewIndex].UpdatePre(pre);

        ClothesViewData next = GetViewData(type, (int)Mathf.Repeat(index + 1, meshDB.meshList[type].Count));
        clothesViews[viewIndex].UpdateNext(next);

        //캐릭터 의상 변경
        Mesh mesh = meshDB.meshList[type][index].mesh;

        customizer.ChangeMesh(type, mesh);
    }

    private ClothesViewData GetViewData(ClothesType type, int index)
    {
        Sprite sprite = meshDB.meshList[type][index].sprite;

        bool useble = meshDB.IsAvailable(type, index);
        int unlockStar = meshDB.meshList[type][index].unlockStarCount;

        return new ClothesViewData(sprite, useble, unlockStar);
    }

    public void ApplyCustomize(InputAction.CallbackContext context)
    {
        //메세지 출력중 일 때도 리턴
        if (!context.started || context.control.device != InputDeviceManager.Instance.InputDevices[playerIndex]
            || isDisplaying || GameManager.Instance.CurrentState == GameState.Paused) return;

        foreach(var data in model.index)
        {
            bool available = meshDB.IsAvailable(data.Key, data.Value);

            if(!available)
            {
                StartCoroutine(DisableMessege(cancel));
                Debug.LogWarning("별이 부족합니다.");
                return;
            }
        }

        if (DataManager.Instance != null)
        {
            foreach (var data in model.index)
            {
                DataManager.Instance.UpdateCustomizeData(playerIndex, data.Key, data.Value);
            }
            DataManager.Instance.SaveData();

            StartCoroutine(DisableMessege(apply));
            Debug.Log("의상 정보 저장 완료");
        }
        else
        {
            StartCoroutine(DisableMessege(cancel));
            Debug.LogWarning("DataManager를 찾을 수 없습니다.");
            return;
        }
    }    


    private IEnumerator DisableMessege(GameObject target)
    {
        isDisplaying = true;

        target.SetActive(true);

        yield return new WaitForSeconds(1);

        target.SetActive(false);

        isDisplaying = false;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.Instance != null)
        {
            UIManager.Instance.ShowScreen(ScreenType.Pause);
            //일단 UI띄우는 걸로, 이전 씬으로 돌아가게 할 수도
        }
    }

    public void Back(InputAction.CallbackContext context)
    {
        if(context.started && UIManager.Instance != null)
        {
            UIManager.Instance.ShowScreen(ScreenType.Pause);
            //일단 UI띄우는 걸로, 이전 씬으로 돌아가게 할 수도
        }    
    }
}
