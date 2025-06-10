using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizeData
{
    public List<TypeData> customizeData = new List<TypeData>
    {
        new TypeData(ClothesType.Hat, 0),
        new TypeData(ClothesType.Hair, 0),
        new TypeData(ClothesType.Accessorises, 0),
        new TypeData(ClothesType.Face, 0),
        new TypeData(ClothesType.Clothes, 0),
        new TypeData(ClothesType.Pants, 0),
        new TypeData(ClothesType.Shoes, 0 ),
    };

    public void GetRandomData(CharacterMeshDB meshDB)
    {
        customizeData = new List<TypeData>
        {
            GetTypeData(ClothesType.Hat, meshDB),
            GetTypeData(ClothesType.Hair, meshDB),
            GetTypeData(ClothesType.Accessorises, meshDB),
            GetTypeData(ClothesType.Face, meshDB),
            GetTypeData(ClothesType.Clothes, meshDB),
            GetTypeData(ClothesType.Pants, meshDB),
            GetTypeData(ClothesType.Shoes, meshDB)
        };
    }

    private TypeData GetTypeData(ClothesType type, CharacterMeshDB meshDB)
    {
        int index = Random.Range(0, meshDB.meshList[type].Count);
        return new TypeData(type, index);
    }
}

[System.Serializable]
public class TypeData
{
    public ClothesType type;
    public int index;

    public TypeData(ClothesType type, int index)
    {
        this.type = type;
        this.index = index;
    }
}
