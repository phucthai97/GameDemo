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
    List<Vector2Int> xGrid = new List<Vector2Int>
    {
        new Vector2Int(-5, 4),
        new Vector2Int(-4, 5),
        new Vector2Int(0, 9)
    };

    List<Vector2Int> yGrid = new List<Vector2Int>
    {
        new Vector2Int(-4, 5),
        new Vector2Int(0, 9),
        new Vector2Int(0, 9)
    };
    Vector3Int lastGridPosition;
    PlacementChecker placementChecker;

    public PlacementState(int iD,
                         PreviewSystem previewSystem,
                         ObjectsDataBaseSO database,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer,
                         PlacementChecker placementChecker)
    {
        ID = iD;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.placementChecker = placementChecker;

        //Get index of Objects Data
        selectedIndexPrefabs = database.objectsData.FindIndex(data => data.ID == ID);
        TouchableObject touchableObject = database.objectsData[selectedIndexPrefabs].Prefab.GetComponent<TouchableObject>();

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
                if (touchableObject.floorPlacement)
                    FindPosAndPlaceAtFloor();
                else
                {
                    FindPosAndPlaceAtWall();
                }
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
            bool validity = placementChecker.CheckPlacementValidity(gridPosition,
                                                                    database.objectsData[selectedIndexPrefabs].Size,
                                                                    selectedIndexPrefabs);
            previewSystem.UpdateGridIndicator(gridPosition, database.objectsData[selectedIndexPrefabs].Size, validity);
            if (validity)
            {
                CreateObjectPlacer(gridPosition);

                //Add object in database
                placementChecker.AddObjectInDataBase(gridPosition,
                                                    database.objectsData[selectedIndexPrefabs].Size,
                                                    selectedIndexPrefabs,
                                                    objectPlacer.currentIndexPlacedObjects);
            }
        }
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            objectPlacer.currentTouchableObj.MovingEIndicator();
            // int indexPrefabs = objectPlacer.currentTouchableObj.indexPrefabs;
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

    void FindPosAndPlaceAtFloor()
    {
        Vector3Int gridPosition = new Vector3Int();
        bool validity = false;
        for (int i = xGrid[0].x; i <= xGrid[0].y; i++)
        {
            for (int j = yGrid[0].x; j <= yGrid[0].y; j++)
            {
                if (placementChecker.CheckPlacementValidity(new Vector3Int(i, 0, j),
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs))
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

    void FindPosAndPlaceAtWall()
    {

    }

    private void CreateObjectPlacer(Vector3Int gridPosition)
    {
        objectPlacer.currentIndexPlacedObjects = objectPlacer.PlaceObject(database.objectsData[selectedIndexPrefabs].Prefab,
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs,
                                                            gridPosition);
    }
}
