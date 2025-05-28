using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomizeDatabase", menuName = "Cumtomize/Database")]
public class CharacterMeshDB : ScriptableObject
{
    [SerializeField] private List<CharacterMeshData> hatMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> hairMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> accessoriesMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> faceMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> clothesMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> pantsMesh = new List<CharacterMeshData>();
    [SerializeField] private List<CharacterMeshData> shoesMesh = new List<CharacterMeshData>();

    private Dictionary<ClothesType, List<CharacterMeshData>> meshList;

    private void OnEnable()
    {
        meshList = new Dictionary<ClothesType, List<CharacterMeshData>> {
            { ClothesType.Hat, hatMesh},
            { ClothesType.Hair, hairMesh},
            { ClothesType.Accessorises, accessoriesMesh},
            { ClothesType.Face, faceMesh},
            { ClothesType.Clothes, clothesMesh},
            { ClothesType.Pants, pantsMesh},
            { ClothesType.Shoes, shoesMesh},
            };
    }

    public Mesh GetMesh(ClothesType type, int index = 0)
    {
        Mesh mesh = new Mesh();

        if (meshList[type].Count > 0)       //mesh = meshList[type][index] 해금 조건 달성 시
        {
            mesh = meshList[type][index].mesh;
        }

        return mesh;
    }
}
