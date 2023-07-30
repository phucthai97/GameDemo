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
                            Vector3Int gridPosition,
                            int typeOfPlacement)
    {
        //Create GameObject
        GameObject instObject = Instantiate(prefab);
        PlacedGameObjects.Add(instObject);
        TouchableObject touchableObject = instObject.GetComponent<TouchableObject>();

        UpdateCurrentTouchableObj(touchableObject,
                                typeOfPlacement,
                                selectedIndexPrefabs,
                                PlacedGameObjects.Count - 1,
                                gridPosition,
                                size);


        //Create position for new object follow type GridPlacement
        Vector3 newPosObject = gridPosition;

        if (typeOfPlacement == 0)
            newPosObject.y = touchableObject.constantPos.y;
        else if(typeOfPlacement == 1)
            newPosObject.z = touchableObject.constantPos.z;
        else if(typeOfPlacement == 2)
            newPosObject.x = touchableObject.constantPos.x;
        
        //Set transform for object
        PlacementChecker placementChecker = FindObjectOfType<PlacementChecker>();
        instObject.transform.position = placementChecker.ObjectAlignment(newPosObject, size, typeOfPlacement);
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
                                        int typeOfPlacement,
                                        int argSelectedIndexPrefabs,
                                        int argCurrentIndexPlacedObj,
                                        Vector3Int gridPosition,
                                        Vector2Int size)
    {
        //Assign currentTouchableObj
        currentTouchableObj = argCurrentTouchableObj;

        //Re-set current index of placed object
        currentIndexPlacedObjects = argCurrentIndexPlacedObj;
        //Set index prefabs
        currentTouchableObj.SetParas(argSelectedIndexPrefabs, typeOfPlacement, size, gridPosition);

        //Update preview grid indicator
        PreviewSystem previewSystem = FindObjectOfType<PreviewSystem>();
        previewSystem.UpdateGridIndicatorMod(gridPosition, size, true, typeOfPlacement);

        //Active edit indicator
        currentTouchableObj.SetActiveEditIndicator(true);
    }

}
