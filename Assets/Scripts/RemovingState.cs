using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                        PreviewSystem previewSystem,
                        GridData floorData,
                        GridData furnitureData,
                        ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        if (objectPlacer.currentTouchableObj != null)
        {
            Debug.Log($"Start removing");
            GridData selectedData = furnitureData;

            //Get gridPosition
            Transform trans = objectPlacer.currentTouchableObj.gameObject.transform;
            Vector3Int gridPosition = new Vector3Int((int)trans.position.x,
                                                    0,
                                                    (int)trans.position.z);

            //Check IndexPrefabs 
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            //Remove grid data at position
            selectedData.RemoveObjectAt(gridPosition);

            //Remove Gameobject
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        else
            Debug.Log($"There is no current placed object for removing");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
            selectedData = furnitureData;
        else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
            selectedData = floorData;

        if (selectedData == null)
        {
            //sound
        }
        else
        {
            //remove
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            //Remove grid data at position
            selectedData.RemoveObjectAt(gridPosition);

            //Remove Gameobject
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        //Update preview system such as (indicator, material object)
        // previewSystem.UpdateGridIndicator(gridPosition,
        //                             Vector2Int.one);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // previewSystem.UpdatePosition(gridPosition, 
        //                             CheckIfSelectionIsValid(gridPosition), 
        //                             new Vector2Int(1, 1));
    }
}
