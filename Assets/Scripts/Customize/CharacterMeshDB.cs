using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomizeDatabase", menuName = "Cumtomize/Database")]
public class CharacterMeshDB : ScriptableObject
{
    public List<CharacterMeshData> hatMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> hairMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> accessoriesMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> faceMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> clothesMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> pantsMesh = new List<CharacterMeshData>();
    public List<CharacterMeshData> shoesMesh = new List<CharacterMeshData>();
}
