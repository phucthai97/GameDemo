using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingFloor : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingFloor(Grid grid,
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
        Debug.Log($"OnAction is {gridPosition}");
        GridData selectedData = null;
        if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
            selectedData = floorData;

        if (selectedData == null)
        {
            Debug.Log($"SelectedData RemovingFloor is null");
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
        previewSystem.UpdateGridIndicator(gridPosition,
                                    Vector2Int.one, true);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        
    }
}
