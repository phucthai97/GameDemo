using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IBuildingState
{
    PreviewSystem previewSystem;
    TouchableObject touchableObject;
    PlacementChecker placementChecker;
    Vector3Int lastGridPosition;
    PlacementSystem placementSystem;

    public MovingState(PreviewSystem previewSystem,
                        PlacementChecker placementChecker,
                        PlacementSystem placementSystem,
                        TouchableObject touchableObject)
    {
        this.previewSystem = previewSystem;
        this.touchableObject = touchableObject;
        this.placementChecker = placementChecker;
        Debug.Log($"Moving state start");
        this.placementSystem = placementSystem;
        touchableObject.SetActiveEditIndicator(true);
        placementChecker.IsThisCurrentTouchalbeObj(touchableObject);
        if (touchableObject.floorPlacement)
            placementChecker.SetActiveGridVisualization("floor");
        else
            placementChecker.SetActiveGridVisualization("wall");
    }

    public void EndState()
    {
        //Stop showing placement review
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {

    }

    public void UpdateState(Vector3Int gridPosition)
    {
        if (touchableObject != null)
        {
            //Moving EIndicator
            touchableObject.MovingEditIndicator();
            if (touchableObject.mouseIsPressed && lastGridPosition != gridPosition)
            {
                int indexPrefabs = touchableObject.indexPrefabs;
                lastGridPosition = gridPosition;

                Vector3 rawPos = new Vector3();
                int typeOfPlacement = 0;

                if (touchableObject.floorPlacement)
                {
                    //Get rawPos
                    rawPos = new Vector3(gridPosition.x,
                                                touchableObject.placementChecker.lastPosition.y + 1.4f,
                                                gridPosition.z);
                    typeOfPlacement = 0;
                }
                else
                {
                    if (placementSystem.layerType == PlacementSystem.LayerType.Wall1)
                    {
                        rawPos = new Vector3(gridPosition.x,
                                            gridPosition.y,
                                            touchableObject.placementChecker.lastPosition.z - 1.4f);
                        typeOfPlacement = 1;
                    }
                    // else if (placementSystem.layerType == PlacementSystem.LayerType.Wall2)
                    // {
                    //     rawPos = new Vector3(touchableObject.placementChecker.lastPosition.x,
                    //                         gridPosition.y,
                    //                         gridPosition.z);
                    //      numberTypeOfPlacement = 2;
                    // }
                }

                //Then align positon of object
                Vector3 alignPos = placementChecker.ObjectAlignment(rawPos,
                                                                    touchableObject.currentSize,
                                                                    typeOfPlacement);

                //Debug.Log($"alignPos is {alignPos}");

                //Set position for moving object
                touchableObject.gameObject.transform.position = alignPos;

                //Check validity  for placement
                bool validity = placementChecker.CheckPlacementValidityMod(gridPosition,
                                                                        touchableObject.currentSize,
                                                                        indexPrefabs,
                                                                        touchableObject.typeOfPlacement);

                touchableObject.currentGridPos = gridPosition;

                previewSystem.UpdateGridIndicatorMod(gridPosition,
                                                touchableObject.currentSize,
                                                validity, typeOfPlacement);
            }
        }
    }
}
