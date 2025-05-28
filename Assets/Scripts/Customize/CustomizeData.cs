using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizeData
{
    public TypeData[] customizeData = new TypeData[7]
    {
        new TypeData(ClothesType.Hat, 0),
        new TypeData(ClothesType.Hair, 0),
        new TypeData(ClothesType.Accessorises, 0),
        new TypeData(ClothesType.Face, 0),
        new TypeData(ClothesType.Clothes, 0),
        new TypeData(ClothesType.Pants, 0),
        new TypeData(ClothesType.Shoes, 0 ),
    };
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
