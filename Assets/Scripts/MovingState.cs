using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IBuildingState
{
    ObjectPlacer objectPlacer;
    GridData floorData, furnitureData;
    PreviewSystem previewSystem;
    TouchableObject touchableObject;
    int indexPrefabs;
    PlacementChecker placementChecker;
    ObjectsDataBaseSO database;
    Vector3Int lastGridPosition;

    public MovingState(PreviewSystem previewSystem,
                        ObjectsDataBaseSO database,
                        GridData floorData,
                        GridData furnitureData,
                        ObjectPlacer objectPlacer,
                        PlacementChecker placementChecker,
                        TouchableObject touchableObject,
                        int indexPrefabs)
    {
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.touchableObject = touchableObject;
        this.placementChecker = placementChecker;
        this.indexPrefabs = indexPrefabs;

        touchableObject.editIndicator.SetActive(true);
        placementChecker.IsThisCurrentTouchalbeObj(touchableObject, indexPrefabs);
    }

    public void EndState()
    {
        //Stop showing placement review
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        Debug.Log($"OnAction {gridPosition}");
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            int indexPrefabs = objectPlacer.currentTouchableObj.indexPrefabs;
            objectPlacer.currentTouchableObj.EditIndicator();
            if (objectPlacer.currentTouchableObj.mouseIsPressed && lastGridPosition != gridPosition)
            {
                lastGridPosition = gridPosition;
                Vector3 newPos = new Vector3(gridPosition.x,
                                            objectPlacer.currentTouchableObj.placementChecker.lastPosition.y + 1.4f,
                                            gridPosition.z);
                objectPlacer.currentTouchableObj.gameObject.transform.position = newPos;
                bool validity = placementChecker.CheckPlacementValidity(gridPosition, indexPrefabs);
                previewSystem.UpdateGridIndicator(gridPosition,
                                                database.objectsData[indexPrefabs].Size
                                                , validity);
            }
        }
    }
}
