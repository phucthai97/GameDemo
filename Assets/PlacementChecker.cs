using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thay vì lưu các thông số cho mỗi Touchable Object thì ta sẽ tạo ra Placement Checker
//để lưu Touchable Object hiện tại -> Giảm biến lưu trữ
public class PlacementChecker : MonoBehaviour
{
    PlacementSystem placementSystem;
    PreviewSystem previewSystem;
    [SerializeField] public Vector3 lastPosition;
    [SerializeField] public bool movingUp = true;
    [SerializeField] public bool movingState = false;
    [SerializeField] public float maxHeightIndicator;

    // Start is called before the first frame update
    void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
        previewSystem = FindObjectOfType<PreviewSystem>();
    }

    public void HandleMouseDownPlacement(GameObject currentObject)
    {
        lastPosition = currentObject.transform.position;
        currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                    currentObject.transform.position.y + 1.4f,
                                                    currentObject.transform.position.z);
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
            Vector3Int gridPosition = new Vector3Int((int)touchableObject.gameObject.transform.position.x
                                                    , 0
                                                    , (int)touchableObject.gameObject.transform.position.z);
            RemoveObjectInDataDase(gridPosition, indexPrefabs);

            //Add new current touchable object
            PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
            int currentIndexPlacedObjects = objectPlacer.placedGameObjects.IndexOf(touchableObject.gameObject);
            Vector2Int size = placementSystem.database.objectsData[indexPrefabs].Size;
            objectPlacer.UpdateCurrentTouchableObj(touchableObject, indexPrefabs, currentIndexPlacedObjects, gridPosition, size);
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
                Vector3 curentPosObj = objectPlacer.currentTouchableObj.gameObject.transform.position;
                Vector3Int gridPosition = new Vector3Int((int)curentPosObj.x,
                                                        0,
                                                        (int)curentPosObj.z);

                AddObjectInDataBase(gridPosition, lastIndexPrefabs, objectPlacer.currentIndexPlacedObjects);

                //Turn of moving of edit indicator
                objectPlacer.currentTouchableObj.editIndicator.SetActive(false);
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
}
