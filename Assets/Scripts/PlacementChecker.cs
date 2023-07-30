using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thay vì lưu các thông số cho mỗi Touchable Object thì ta sẽ tạo ra Placement Checker
//để lưu Touchable Object hiện tại -> Giảm biến lưu trữ
//placement như cầu nối tạo ra các method đáp ứng dùng cho các trường hợp trong placement system
public class PlacementChecker : MonoBehaviour
{
    PlacementSystem placementSystem;
    PreviewSystem previewSystem;
    ObjectPlacer objectPlacer;
    [SerializeField] public Vector3 lastPosition;
    [SerializeField] public Vector3Int lastGridPos;
    [SerializeField] public bool movingUp = true;
    [SerializeField] public float maxHeightIndicator;

    //First Clicked for choose with countClicked = 0, Second clicked for moving with countClicked = 1
    [SerializeField] public int countClicked = 0;
    public enum Mode { Moving, Floorplan }
    [SerializeField] public Mode mode;

    //For see virtual grid on scence when it's active
    [SerializeField] public GameObject gridVisualizationFloor;
    [SerializeField] public GameObject gridVisualizationWall1;
    [SerializeField] public GameObject gridVisualizationWall2;

    // Start is called before the first frame update
    void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
        previewSystem = FindObjectOfType<PreviewSystem>();
        objectPlacer = FindObjectOfType<ObjectPlacer>();
    }

    public void HandleMouseDownPlacement(TouchableObject touchableObject)
    {
        lastPosition = touchableObject.transform.position;
        lastGridPos = touchableObject.currentGridPos;
    }

    public void HandleMouseUpPlacement(TouchableObject touchableObject, int selectedObjectIndex)
    {
        Vector3Int gridPosition = touchableObject.currentGridPos;

        //If Mouse up at valid position
        if (CheckPlacementValidityMod(gridPosition, touchableObject.currentSize, selectedObjectIndex, touchableObject.typeOfPlacement))
        {
            if (touchableObject.floorPlacement)
            {
                touchableObject.transform.position = new Vector3(touchableObject.transform.position.x,
                                                                lastPosition.y,
                                                                touchableObject.transform.position.z);
            }
            else
            {
                if (placementSystem.layerType == PlacementSystem.LayerType.Wall1)
                {
                    Debug.Log($"Okay");
                    touchableObject.transform.position = new Vector3(touchableObject.transform.position.x,
                                                                    touchableObject.transform.position.y,
                                                                    lastPosition.z);
                }
                else if (placementSystem.layerType == PlacementSystem.LayerType.Wall2)
                {
                    touchableObject.transform.position = new Vector3(lastPosition.x,
                                                                    touchableObject.transform.position.y,
                                                                    touchableObject.transform.position.z);
                }
            }
        }

        //If Mouse up at Not valid position -> turn back the lastPosition
        else
        {
            touchableObject.transform.position = lastPosition;
            touchableObject.currentGridPos = lastGridPos;

            previewSystem.UpdateGridIndicator(lastGridPos,
                                            touchableObject.currentSize,
                                            true);
        }
    }

    public void IsThisCurrentTouchalbeObj(TouchableObject touchableObject)
    {
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != touchableObject)
        {
            //Add old-currentTouchObj into data base
            AddFurniture(objectPlacer);

            RemoveObjectInDataDase(touchableObject.currentGridPos, touchableObject.indexPrefabs);

            //Add new current touchable object
            PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
            int currentIndexPlacedObjects = objectPlacer.placedGameObjects.IndexOf(touchableObject.gameObject);

            //Update new-current touchable object
            objectPlacer.UpdateCurrentTouchableObj(touchableObject,
                                                touchableObject.typeOfPlacement,
                                                touchableObject.indexPrefabs,
                                                currentIndexPlacedObjects,
                                                touchableObject.currentGridPos,
                                                touchableObject.currentSize);
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
            int indexPrefabs = objectPlacer.currentIndexPlacedObjects;
            if (indexPrefabs == -1)
                return;

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
                AddObjectInDataBase(objectPlacer.currentTouchableObj.currentGridPos,
                                    objectPlacer.currentTouchableObj.currentSize,
                                    lastIndexPrefabs,
                                    objectPlacer.currentIndexPlacedObjects,
                                    objectPlacer.currentTouchableObj.typeOfPlacement);

                //Turn of moving of edit indicator
                objectPlacer.currentTouchableObj.SetActiveEditIndicator(false);
                //Set null for currentTouchableObj
                objectPlacer.currentTouchableObj = null;
            }
        }
    }

    public void AddObjectInDataBase(Vector3Int gridPosition, Vector2Int currentSize, int indexPrefabs, int currentIndex, int typeOfPlacement)
    {
        //Classify object foor/furniture
        GridData selectedData = placementSystem.database.objectsData[indexPrefabs].ID < 10000 ?
                                placementSystem.furnitureData :
                                placementSystem.floorData;


        //Add object into placement database
        selectedData.AddObjectAt(gridPosition,
                                currentSize,
                                placementSystem.database.objectsData[indexPrefabs].ID,
                                currentIndex,
                                typeOfPlacement);
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

    public bool CheckPlacementValidityMod(Vector3Int gridPosition, Vector2Int currentSize, int selectedObjectIndex, int typeOfPlacement)
    {
        GridData selectedData = placementSystem.database.objectsData[selectedObjectIndex].ID < 10000 ?
                                placementSystem.furnitureData :
                                placementSystem.floorData;

        //Check this positon can place object or Not?
        return selectedData.CanPlaceObjectAtMod(gridPosition, currentSize, typeOfPlacement);
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

    public void RenewCurrentTouchableObject()
    {
        objectPlacer.currentTouchableObj = null;
        objectPlacer.currentIndexPlacedObjects = -1;
    }

    //We have the following offset calculation formula:
    // -> (size.x/2)
    // -> (size.y/2) - 1
    //Then we apply it to the below method
    public Vector3 ObjectAlignment(Vector3 position, Vector2 size, int numberTypeOfPlacement)
    {
        Vector3 posOffSet = new Vector3();
        if (numberTypeOfPlacement == 0)
        {
            posOffSet = new Vector3(size.x / 2,
                                    0,
                                    (size.y / 2) - 1);
        }
        else if (numberTypeOfPlacement == 1)
        {
            posOffSet = new Vector3((size.x / 2) - 1,
                                    size.y / 2,
                                    0);
        }

        //Debug.Log($"ObjectAlignment is {position} with numberTypeOfPlacement is {numberTypeOfPlacement} with posOffset is {posOffSet}");
        return position + posOffSet;
    }

    public void SetActiveGridVisualization(string key)
    {
        if(key == "turnoffall")
        {
            gridVisualizationFloor.SetActive(false);
            gridVisualizationWall1.SetActive(false);
            gridVisualizationWall2.SetActive(false);
        }
        else if(key == "floor")
        {
            gridVisualizationFloor.SetActive(true);
            gridVisualizationWall1.SetActive(false);
            gridVisualizationWall2.SetActive(false);
        }
        else if(key == "wall")
        {
            gridVisualizationFloor.SetActive(false);
            gridVisualizationWall1.SetActive(true);
            gridVisualizationWall2.SetActive(true);
        }
        else if(key == "turnonall")
        {
            gridVisualizationFloor.SetActive(true);
            gridVisualizationWall1.SetActive(true);
            gridVisualizationWall2.SetActive(true);
        }
        else
            Debug.LogError($"key not valid");
    }

    // public Vector3Int GetTurnGridPos(Vector3 position, Vector2 size)
    // {
    //     Vector3 posOffset = new Vector3(size.x / 2,
    //                                     0,
    //                                     (size.y / 2) - 1);
    //     Vector3Int result = new Vector3Int((int)Mathf.Ceil(position.x - posOffset.x),
    //                                     0,
    //                                     (int)Mathf.Ceil(position.z - posOffset.y));
    //     return result;
    // }
}
