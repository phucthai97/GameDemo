using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thay vì lưu các thông số cho mỗi Touchable Object thì ta sẽ tạo ra Placement Checker
//để lưu Touchable Object hiện tại -> Giảm biến lưu trữ
public class PlacementChecker : MonoBehaviour
{
    PlacementSystem placementSystem;
    PreviewSystem previewSystem;
    ObjectPlacer objectPlacer;
    [SerializeField] public Vector3 lastPosition;
    [SerializeField] public bool movingUp = true;
    [SerializeField] public float maxHeightIndicator;

    //First Clicked for choose with countClicked = 0, Second clicked for moving with countClicked = 1
    [SerializeField] public int countClicked = 0;
    public enum Mode { Moving, Floorplan }
    [SerializeField] public Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
        previewSystem = FindObjectOfType<PreviewSystem>();
        objectPlacer = FindObjectOfType<ObjectPlacer>();
    }

    public void HandleMouseDownPlacement(GameObject currentObject)
    {
        lastPosition = currentObject.transform.position;
        // currentObject.transform.position = new Vector3(currentObject.transform.position.x,
        //                                             currentObject.transform.position.y + 1.4f,
        //                                             currentObject.transform.position.z);
    }

    public void HandleMouseUpPlacement(GameObject currentObject, int selectedObjectIndex)
    {
        Vector3Int gridPosition = new Vector3Int((int)currentObject.transform.position.x,
                                                0,
                                                (int)currentObject.transform.position.z);

        //If Mouse up at valid position
        if (CheckPlacementValidity(gridPosition, selectedObjectIndex))
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                        lastPosition.y,
                                                        currentObject.transform.position.z);
        }
        ////If Mouse up at Not valid position
        else
        {
            currentObject.transform.position = lastPosition;
            previewSystem.UpdateGridIndicator(lastPosition,
                                            placementSystem.database.objectsData[selectedObjectIndex].Size, true);
        }
    }

    public void IsThisCurrentTouchalbeObj(TouchableObject touchableObject, int indexPrefabs)
    {
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != touchableObject)
        {
            //Add old-currentTouchObj into data base
            AddFurniture(objectPlacer);

            //Remove grid data at position of pre-CurrentTouchObj
            // Vector3Int gridPosition = new Vector3Int((int)touchableObject.gameObject.transform.position.x
            //                                         , 0
            //                                         , (int)touchableObject.gameObject.transform.position.z);
            Vector3Int gridPosition = GetTurnGridPos(touchableObject.gameObject.transform.position, touchableObject.currentSize);

            //Debug.Log($"new gridPosition is {gridPosition}");
            RemoveObjectInDataDase(gridPosition, indexPrefabs);

            //Add new current touchable object
            PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
            int currentIndexPlacedObjects = objectPlacer.placedGameObjects.IndexOf(touchableObject.gameObject);
            objectPlacer.UpdateCurrentTouchableObj(touchableObject, indexPrefabs, currentIndexPlacedObjects, gridPosition, touchableObject.currentSize);
        }
    }

    public void RemoveFurnitureObject()
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            GridData selectedData = placementSystem.furnitureData;

            //Get gridPosition
            Transform trans = objectPlacer.currentTouchableObj.gameObject.transform;
            Vector3Int gridPosition = new Vector3Int((int)trans.position.x,
                                                    (int)0,
                                                    (int)trans.position.z);


            //Check IndexPrefabs 
            //int gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            int indexPrefabs = objectPlacer.currentIndexPlacedObjects;
            Debug.Log($"Start removing with gridPos {gridPosition} and index {indexPrefabs}");
            if (indexPrefabs == -1)
            {
                Debug.LogError($"IndexPrefab = -1");
                return;
            }

            //selectedData.RemoveObjectAt(gridPosition);

            //Remove Gameobject
            objectPlacer.RemoveObjectAt(indexPrefabs);
        }
    }

    public void AddFurniture(ObjectPlacer objectPlacer)
    {
        if (objectPlacer.currentTouchableObj != null && objectPlacer.currentIndexPlacedObjects != -1)
        {
            //Get last index of prefabs
            int lastIndexPrefabs = objectPlacer.currentTouchableObj.indexPrefabs;

            //If object belongs to furniture
            if (placementSystem.database.objectsData[lastIndexPrefabs].ID < 10000)
            {
                //Get current position of current object
                // Vector3 currentPosObj = objectPlacer.currentTouchableObj.gameObject.transform.position;
                // Vector3Int gridPosition = new Vector3Int((int)currentPosObj.x,
                //                                         0,
                //                                         (int)currentPosObj.z);
                Vector3Int gridPosition = GetTurnGridPos(objectPlacer.currentTouchableObj.gameObject.transform.position,
                                                        objectPlacer.currentTouchableObj.currentSize);

                //Debug.Log($"new currentPos is {gridPosition}");

                AddObjectInDataBase(gridPosition, lastIndexPrefabs, objectPlacer.currentIndexPlacedObjects);

                //Turn of moving of edit indicator
                objectPlacer.currentTouchableObj.TurnONOFFIndicator(false);
                //Set null for currentTouchableObj
                objectPlacer.currentTouchableObj = null;
            }
        }
    }

    public void AddObjectInDataBase(Vector3Int gridPosition, int indexPrefabs, int currentIndex)
    {
        //Classify object foor/furniture
        GridData selectedData = placementSystem.database.objectsData[indexPrefabs].ID < 10000 ?
                                placementSystem.furnitureData :
                                placementSystem.floorData;

        //Add object into placement database
        selectedData.AddObjectAt(gridPosition,
                                placementSystem.database.objectsData[indexPrefabs].Size,
                                placementSystem.database.objectsData[indexPrefabs].ID,
                                currentIndex);
    }

    public Vector2Int GetSizeBaseOnRotate()
    {
        Vector2Int Size = new Vector2Int();
        GameObject firstChild = objectPlacer.currentTouchableObj.gameObject.transform.GetChild(0).gameObject;
        //Debug.Log($"{firstChild.name} Rotation Y-axis is {firstChild.transform.localRotation}");
        // if (firstChild.transform.localRotation.y == 1 || firstChild.transform.localRotation.y == 0)
        //     Debug.Log($"Even {firstChild.transform.localRotation.y}");
        return Size;
    }

    public void RemoveObjectInDataDase(Vector3Int gridPosition, int indexPrefabs)
    {
        //Classify object foor/furniture
        GridData selectedData = placementSystem.database.objectsData[indexPrefabs].ID < 10000 ?
                                placementSystem.furnitureData :
                                placementSystem.floorData;

        //Remove object
        selectedData.RemoveObjectAt(gridPosition);
    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = placementSystem.database.objectsData[selectedObjectIndex].ID < 10000 ?
                                placementSystem.furnitureData :
                                placementSystem.floorData;

        //Check this positon can place object or Not?
        return selectedData.CanPlaceObjectAt(gridPosition, placementSystem.database.objectsData[selectedObjectIndex].Size);
    }

    public void CheckAndSetCountClicked(TouchableObject touchableObject)
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            if (touchableObject == objectPlacer.currentTouchableObj)
                countClicked++;
            else
                countClicked = 0;
        }
    }


    //We have the following offset calculation formula:
    // -> (size.x/2)
    // -> (size.y/2) - 1
    //Then we apply it to the below method
    public Vector3 ObjectAlignment(Vector3 position, Vector2 size)
    {
        Vector3 posOffset = new Vector3(size.x / 2,
                                        0,
                                        (size.y / 2) - 1);

        //Debug.Log($"posOffset is {position + posOffset}");
        return position + posOffset;
    }

    public Vector3Int GetTurnGridPos(Vector3 position, Vector2 size)
    {
        Vector3 posOffset = new Vector3(size.x / 2,
                                        0,
                                        (size.y / 2) - 1);
        Vector3Int result = new Vector3Int((int)Mathf.Ceil(position.x - posOffset.x),
                                        0,
                                        (int)Mathf.Ceil(position.z - posOffset.y));
        return result;
    }

}
