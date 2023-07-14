using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedIndexPrefabs = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDataBaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    Vector2Int xGrid, yGrid;
    Vector3Int lastGridPosition;
    PlacementChecker placementChecker;

    public PlacementState(int iD,
                         Grid grid,
                         PreviewSystem previewSystem,
                         ObjectsDataBaseSO database,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer,
                         Vector2Int xGrid,
                         Vector2Int yGrid,
                         PlacementChecker placementChecker)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.xGrid = xGrid;
        this.yGrid = yGrid;
        this.placementChecker = placementChecker;

        //Get index of Objects Data
        selectedIndexPrefabs = database.objectsData.FindIndex(data => data.ID == ID);

        //if selectedObjectIndex exists 
        if (selectedIndexPrefabs > -1)
        {
            //Check if currentTouchableObj has gameObject or NOT. If yes -> fixed it
            placementChecker.AddFurniture(objectPlacer);

            //Check ID
            if (ID >= 10000)
                placementChecker.mode = PlacementChecker.Mode.Floorplan;
            else
            {
                placementChecker.mode = PlacementChecker.Mode.Moving;
                Debug.Log($"Mode.Moving");
                FindPosAndPlace();
            }
        }
        else
            throw new System.Exception($"No object with in ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        //If ID object belongs to floor
        if (ID >= 10000)
        {
            bool validity = placementChecker.CheckPlacementValidity(gridPosition, selectedIndexPrefabs);
            previewSystem.UpdateGridIndicator(gridPosition, database.objectsData[selectedIndexPrefabs].Size, validity);
            if (validity)
            {
                CreateObjectPlacer(gridPosition);
                placementChecker.AddObjectInDataBase(gridPosition, selectedIndexPrefabs, objectPlacer.currentIndexPlacedObjects);
            }
        }
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            objectPlacer.currentTouchableObj.EditIndicator();
            //int indexPrefabs = objectPlacer.currentTouchableObj.indexPrefabs;
            // if (objectPlacer.currentTouchableObj.mouseIsPressed && lastGridPosition != gridPosition)
            // {
            //     lastGridPosition = gridPosition;
            //     Vector3 newPos = new Vector3(gridPosition.x,
            //                                 objectPlacer.currentTouchableObj.placementChecker.lastPosition.y + 1.4f,
            //                                 gridPosition.z);
            //     objectPlacer.currentTouchableObj.gameObject.transform.position = newPos;
            //     bool validity = placementChecker.CheckPlacementValidity(gridPosition, indexPrefabs);
            //     previewSystem.UpdateGridIndicator(gridPosition,
            //                                     database.objectsData[indexPrefabs].Size
            //                                     , validity);
            // }
        }
    }

    void FindPosAndPlace()
    {
        Vector3Int gridPosition = new Vector3Int();
        bool validity = false;
        for (int i = xGrid.x; i <= xGrid.y; i++)
        {
            for (int j = yGrid.x; j <= yGrid.y; j++)
            {
                if (placementChecker.CheckPlacementValidity(new Vector3Int(i, 0, j), selectedIndexPrefabs))
                {
                    gridPosition = new Vector3Int(i, 0, j);
                    validity = true;
                    break;
                }
            }
            if (validity)
                break;
        }

        if (validity)
            CreateObjectPlacer(gridPosition);
        else
            Debug.Log($"There is no space for placement!");
    }

    private void CreateObjectPlacer(Vector3Int gridPosition)
    {
        objectPlacer.currentIndexPlacedObjects = objectPlacer.PlaceObject(database.objectsData[selectedIndexPrefabs].Prefab,
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs,
                                                            gridPosition);
    }
}
