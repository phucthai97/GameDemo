using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //Input manager by input device: mouse, keyboard
    [SerializeField] private InputManager inputManager;

    //For see virtual grid on scence when it's active
    [SerializeField] private GameObject gridVisualization;

    //For determined coordinate
    [SerializeField] private Grid grid;

    //Sphere
    [SerializeField] private GameObject Sphere;

    //For storage objects database
    [SerializeField] internal ObjectsDataBaseSO database;

    //For preview object on the virtual grid before placing object
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private PlacementChecker placementChecker;
    [SerializeField] private PlacementUIControl placementUIControl;

    //For storage that object was placed in the scene
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] Vector2Int xGrid = new Vector2Int(-5, 4);
    [SerializeField] Vector2Int yGrid = new Vector2Int(-4, 5);

    //For storage 2 kind of GridData
    public GridData floorData, furnitureData;

    //For detect last position;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    //Declare interface
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        gridVisualization.SetActive(false);
        placementUIControl.TurnOffEditButtonObject();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        //Set active tools
        gridVisualization.SetActive(true);
        placementUIControl.TurnOnEditBtnObject(ID);
        buildingState = new PlacementState(ID,
                                        grid,
                                        preview,
                                        database,
                                        floorData,
                                        furnitureData,
                                        objectPlacer,
                                        xGrid,
                                        yGrid,
                                        placementChecker);

        //Passing Place_Structure into On Clicked delegate
        inputManager.OnClicked += PlaceStructure;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void StartMoving(TouchableObject touchableObject, int indexPrefabs)
    {
        StopPlacement();
        //Set active tools
        gridVisualization.SetActive(true);
        placementUIControl.TurnOnEditBtnObject(database.objectsData[indexPrefabs].ID);
        buildingState = new MovingState(preview,
                                        database,
                                        floorData,
                                        furnitureData,
                                        objectPlacer,
                                        placementChecker,
                                        touchableObject,
                                        indexPrefabs);
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        placementChecker.RemoveFurnitureObject();
    }

    public void StartRemovingFloor()
    {
        StopPlacement();
        //Set de-active tools
        gridVisualization.SetActive(true);
        placementUIControl.TurnOnRemovingFloor();
        buildingState = new RemovingFloor(grid, preview, floorData, furnitureData, objectPlacer);
        //Passing Place_Structure into On Clicked delegate
        inputManager.OnClicked += PlaceStructure;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void RotateObject()
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            gridVisualization.SetActive(true);
            objectPlacer.currentTouchableObj.RotateObject();
        }
    }

    private void PlaceStructure()
    {
        //If pointer of mouse click on UI area -> do nothing!
        if (inputManager.IsPointerOverUI())
            return;

        buildingState.OnAction(GetGridCurrentPos());
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        //Set De-active tools
        gridVisualization.SetActive(false);
        placementUIControl.SetColorBtnRemoveFloor(false);
        //Deactive indicator,...
        buildingState.EndState();

        //Getting out the Place_Structure function from delegate On Clicked
        inputManager.OnClicked -= PlaceStructure;
        //Getting out the Stop_Placement function from delegate On Exit
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        // if (lastDetectedPosition != GetGridCurrentPos())
        // {
        buildingState.UpdateState(GetGridCurrentPos());
        //}
    }

    private Vector3Int GetGridCurrentPos()
    {
        //Get hit on place by position of mouse
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        Sphere.transform.position = mousePosition;
        //Get current grid position
        return new Vector3Int((int)Mathf.Floor(mousePosition.x)
                            , 0
                            , (int)Mathf.Ceil(mousePosition.z));
    }
}
