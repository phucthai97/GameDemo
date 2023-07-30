using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedIndexPrefabs = -1;
    int ID;
    PreviewSystem previewSystem;
    ObjectsDataBaseSO database;
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
        new Vector2Int(-4, 5)
    };
    PlacementChecker placementChecker;

    public PlacementState(int iD,
                         PreviewSystem previewSystem,
                         ObjectsDataBaseSO database,
                         ObjectPlacer objectPlacer,
                         PlacementChecker placementChecker)
    {
        ID = iD;
        this.previewSystem = previewSystem;
        this.database = database;
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
            {
                placementChecker.mode = PlacementChecker.Mode.Floorplan;
                placementChecker.SetActiveGridVisualization("floor");
            }
            else
            {
                placementChecker.mode = PlacementChecker.Mode.Moving;
                if (touchableObject.floorPlacement)
                    FindPosAndPlaceAtFloor();
                else
                    FindPosAndPlaceAtWall();
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
            bool validity = placementChecker.CheckPlacementValidityMod(gridPosition,
                                                                    database.objectsData[selectedIndexPrefabs].Size,
                                                                    selectedIndexPrefabs,
                                                                    0);
            previewSystem.UpdateGridIndicator(gridPosition, database.objectsData[selectedIndexPrefabs].Size, validity);
            if (validity)
            {
                CreateObjectPlacer(gridPosition, 0);

                //Add object in database
                placementChecker.AddObjectInDataBase(gridPosition,
                                                    database.objectsData[selectedIndexPrefabs].Size,
                                                    selectedIndexPrefabs,
                                                    objectPlacer.currentIndexPlacedObjects,
                                                    0);
            }
        }
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            objectPlacer.currentTouchableObj.MovingEditIndicator();
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
        placementChecker.SetActiveGridVisualization("floor");
        Vector3Int gridPosition = new Vector3Int();
        bool validity = false;
        for (int i = xGrid[0].x; i <= xGrid[0].y; i++)
        {
            for (int j = yGrid[0].x; j <= yGrid[0].y; j++)
            {
                if (placementChecker.CheckPlacementValidityMod(new Vector3Int(i, 0, j),
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs, 
                                                            0))
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
            CreateObjectPlacer(gridPosition, 0);
        else
            Debug.Log($"There is no space for placement!");
    }

    void FindPosAndPlaceAtWall()
    {
        placementChecker.SetActiveGridVisualization("wall");
        Vector3Int gridPosition = new Vector3Int();
        bool validity = false;

        //numberTypeOfPlacement with (0: Floor, 1: Wall1, 2: Wall2)
        int typeOfPlacement = 1;
        for (int i = xGrid[typeOfPlacement].x; i <= xGrid[typeOfPlacement].y; i++)
        {
            for (int j = yGrid[typeOfPlacement].x; j <= yGrid[typeOfPlacement].y; j++)
            {
                if (placementChecker.CheckPlacementValidityMod(new Vector3Int(i, j, 0),
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs, 
                                                            typeOfPlacement))
                {
                    gridPosition = new Vector3Int(i, j, 0);
                    validity = true;
                    break;
                }
            }
            if (validity)
                break;
        }

        //If can't find cell for place wall1 -> Continue find cell at wall2
        if (!validity)
        {
            typeOfPlacement = 2;
            // for (int i = xGrid[numberTypeOfPlacement].x; i <= xGrid[numberTypeOfPlacement].y; i++)
            // {
            //     for (int j = yGrid[numberTypeOfPlacement].x; j <= yGrid[numberTypeOfPlacement].y; j++)
            //     {
            //         Debug.Log($"!validity with pos {new Vector3Int(0, i, j)}");
            //         if (placementChecker.CheckPlacementValidity(new Vector3Int(0, i, j),
            //                                                     database.objectsData[selectedIndexPrefabs].Size,
            //                                                     selectedIndexPrefabs))
            //         {
            //             gridPosition = new Vector3Int(0, i, j);
            //             validity = true;
            //             break;
            //         }
            //     }
            //     if (validity)
            //         break;
            // }
        }

        if (validity)
            CreateObjectPlacer(gridPosition, typeOfPlacement);
        else
            Debug.Log($"There is no space for placement!");
    }

    //typeOfPlacement with (0: Floor, 1: Wall1, 2: Wall2)
    private void CreateObjectPlacer(Vector3Int gridPosition, int numberTypeOfPlacement)
    {
        //Debug.Log($"GridPos is {gridPosition}");
        objectPlacer.currentIndexPlacedObjects = objectPlacer.PlaceObject(database.objectsData[selectedIndexPrefabs].Prefab,
                                                            database.objectsData[selectedIndexPrefabs].Size,
                                                            selectedIndexPrefabs,
                                                            gridPosition,
                                                            numberTypeOfPlacement);
    }
}
