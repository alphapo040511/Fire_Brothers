using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizeData
{
    public TypeData[] customizeData = new TypeData[7];
}

[System.Serializable]
public class TypeData
{
    public ClothesType type;
    public int index;
}
