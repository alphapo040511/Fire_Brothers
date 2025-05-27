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

    public Mesh GetMesh(ClothesType type, int index = 0)
    {
        Mesh mesh;

        switch(type)
        {
            case ClothesType.Hat:
                mesh = UsableChecking(hatMesh, index);
                break;
            case ClothesType.Hair:
                mesh = UsableChecking(hairMesh, index);
                break;
            case ClothesType.Accessorises:
                mesh = UsableChecking(accessoriesMesh, index);
                break;
            case ClothesType.Face:
                mesh = UsableChecking(faceMesh, index);
                break;
            case ClothesType.Clothes:
                mesh = UsableChecking(clothesMesh, index);
                break;
            case ClothesType.Pants:
                mesh = UsableChecking(pantsMesh, index);
                break;
            case ClothesType.Shoes:
                mesh = UsableChecking(shoesMesh, index);
                break;
            default:
                mesh = hatMesh[0].mesh;
                break;
        }

        return mesh;
    }
    
    private Mesh UsableChecking(List<CharacterMeshData> meshList, int index)
    {
        if (index > meshList.Count)     //또는 의상이 잠겨있는 경우 추가
        {
            return meshList[0].mesh;
        }

        return meshList[index].mesh;
    }
}
