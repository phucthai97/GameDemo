using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "DataBase", fileName = "New DataBase")]
public class ObjectsDataBaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
    public List<Prefabs> dataPrefabs;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public float maxHeight { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}

[Serializable]
public class Prefabs
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
