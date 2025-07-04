using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    public int playerIndex;
    public bool useRandomData = false;

    [SerializeField] private CustomizeData sampleData;                                            //커스터마이징 임시 데이터

    [SerializeField] private CharacterMeshDB m_CharacterMeshDB;                 //의상 DB

    [SerializeField] private SkinnedMeshRenderer hatRenderer;
    [SerializeField] private SkinnedMeshRenderer hairRenderer;
    [SerializeField] private SkinnedMeshRenderer accessorisesRenderer;
    [SerializeField] private SkinnedMeshRenderer faceRenderer;
    [SerializeField] private SkinnedMeshRenderer clothesRenderer;
    [SerializeField] private SkinnedMeshRenderer pantsRenderer;
    [SerializeField] private SkinnedMeshRenderer shoesRenderer;

    private Dictionary<ClothesType, SkinnedMeshRenderer> renderers;

    private void Awake()
    {
        renderers = new Dictionary<ClothesType, SkinnedMeshRenderer> {
            { ClothesType.Hat, hatRenderer},
            { ClothesType.Hair, hairRenderer},
            { ClothesType.Accessorises, accessorisesRenderer},
            { ClothesType.Face, faceRenderer},
            { ClothesType.Clothes, clothesRenderer},
            { ClothesType.Pants, pantsRenderer},
            { ClothesType.Shoes, shoesRenderer},
            };
    }

    // 게임 시작시로 변경
    void Start()
    {
        if(useRandomData)
        {
            sampleData = new CustomizeData();
            sampleData.GetRandomData(m_CharacterMeshDB);                                //랜덤한 의상 데이터 생성
        }
        else if (DataManager.Instance != null)
        {
            sampleData = DataManager.Instance.gameData.playersCustomData[playerIndex];
        }

        if (sampleData != null)
        {
            for(int i = 0; i < sampleData.customizeData.Count; i++)
            {
                ClothesType type = sampleData.customizeData[i].type;

                if (renderers.ContainsKey(type))
                {
                    renderers[type].sharedMesh = m_CharacterMeshDB.GetMesh(type, sampleData.customizeData[i].index);
                }
            }
        }
    }

    public void ChangeMesh(ClothesType type, Mesh mesh)
    {
        renderers[type].sharedMesh = mesh;
    }

}
