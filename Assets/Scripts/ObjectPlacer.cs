using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] public List<GameObject> placedGameObjects = new();
    [SerializeField] public TouchableObject currentTouchableObj;
    [SerializeField] public int currentIndexPlacedObjects = -1;
    

    public List<GameObject> PlacedGameObjects { get => placedGameObjects; set => placedGameObjects = value; }

    public int PlaceObject(GameObject prefab,
                            Vector2Int size,
                            int selectedIndexPrefabs,
                            Vector3 position)
    {
        GameObject instObject = Instantiate(prefab);
        PlacedGameObjects.Add(instObject);

        UpdateCurrentTouchableObj(instObject.GetComponent<TouchableObject>(),
                                selectedIndexPrefabs,
                                PlacedGameObjects.Count - 1,
                                position,
                                size);

        //Set transform for object
        instObject.transform.position = position;
        instObject.transform.SetParent(gameObject.transform);
        return PlacedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (PlacedGameObjects.Count <= gameObjectIndex
        || PlacedGameObjects[gameObjectIndex] == null)
            return;

        Destroy(PlacedGameObjects[gameObjectIndex]);
        PlacedGameObjects[gameObjectIndex] = null;
    }

    public void UpdateCurrentTouchableObj(TouchableObject argCurrentTouchableObj,
                                        int argSelectedIndexPrefabs,
                                        int argCurrentIndexPlacedObj,
                                        Vector3 position,
                                        Vector2Int size)
    {
        //Assign currentTouchableObj
        currentTouchableObj = argCurrentTouchableObj;

        //Re-set current index of placed object
        currentIndexPlacedObjects = argCurrentIndexPlacedObj;
        //Set index prefabs
        currentTouchableObj.SetParas(argSelectedIndexPrefabs, size);


        //Update preview grid indicator
        PreviewSystem previewSystem = FindObjectOfType<PreviewSystem>();
        previewSystem.UpdateGridIndicator(position, size, true);

        //Active edit indicator
        currentTouchableObj.TurnONOFFIndicator(true);
    }

}
