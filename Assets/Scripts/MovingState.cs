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

        touchableObject.TurnONOFFIndicator(true);
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
            //Moving EIndicator
            objectPlacer.currentTouchableObj.MovingEIndicator();
            if (objectPlacer.currentTouchableObj.mouseIsPressed && lastGridPosition != gridPosition)
            {
                int indexPrefabs = objectPlacer.currentTouchableObj.indexPrefabs;
                lastGridPosition = gridPosition;

                //Get rawPos
                Vector3 rawPos = new Vector3(gridPosition.x,
                                            objectPlacer.currentTouchableObj.placementChecker.lastPosition.y + 1.4f,
                                            gridPosition.z);
                //Then align positon of object
                Vector3 alignPos = placementChecker.ObjectAlignment(rawPos,
                                                                    objectPlacer.currentTouchableObj.currentSize);

                //Set position for moving object
                objectPlacer.currentTouchableObj.gameObject.transform.position = alignPos;

                //Check validity  for placement
                bool validity = placementChecker.CheckPlacementValidity(gridPosition, indexPrefabs);

                objectPlacer.currentTouchableObj.currentGridPos = gridPosition;

                previewSystem.UpdateGridIndicator(gridPosition,
                                                objectPlacer.currentTouchableObj.currentSize,
                                                validity);
            }

            // //Update current gridPosition
            // else if (!objectPlacer.currentTouchableObj.mouseIsPressed && lastGridPosition != gridPosition
            //         && placementChecker.countClicked >= 1
            //         && placementChecker.mode == PlacementChecker.Mode.Moving)
            // {
            //     objectPlacer.currentTouchableObj.currentGridPos = gridPosition;
            //     Debug.Log($"gridPosition updated {gridPosition}");
            // }
        }
    }
}
