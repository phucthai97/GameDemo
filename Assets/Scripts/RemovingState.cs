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

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // previewSystem.UpdatePosition(gridPosition, 
        //                             CheckIfSelectionIsValid(gridPosition), 
        //                             new Vector2Int(1, 1));
    }
}
