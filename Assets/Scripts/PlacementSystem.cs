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

    //For storage objects database
    [SerializeField] internal ObjectsDataBaseSO database;

    //For preview object on the virtual grid before placing object
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private PlacementChecker placementChecker;

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
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
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

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer);
        //Passing Place_Structure into On Clicked delegate
        inputManager.OnClicked += PlaceStructure;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
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
        //Setting state-of-arrangement is false
        gridVisualization.SetActive(false);

        //Deactive indicator,...
        buildingState.EndState();

        //Getting out the Place_Structure function from delegate On Clicked
        inputManager.OnClicked -= PlaceStructure;
        //Getting out the Stop_Placement function from delegate On Exit
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    public void StartMoving(TouchableObject touchableObject, int indexPrefabs)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new MovingState(preview, 
                                        database, 
                                        floorData, 
                                        furnitureData, 
                                        objectPlacer, 
                                        placementChecker, 
                                        touchableObject, 
                                        indexPrefabs);
        //Passing Place_Structure into On Clicked delegate
        //inputManager.OnClicked += MovingObject;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    private void MovingObject()
    {
        //If pointer of mouse click on UI area -> do nothing!
        if (inputManager.IsPointerOverUI())
            return;

        buildingState.OnAction(GetGridCurrentPos());
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        // if (lastDetectedPosition != GetGridCurrentPos())
        // {
        buildingState.UpdateState(GetGridCurrentPos());
        //lastDetectedPosition = GetGridCurrentPos();
        //}
    }

    private Vector3Int GetGridCurrentPos()
    {
        //Get hit on place by position of mouse
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //Get current grid position
        return new Vector3Int((int)Mathf.Floor(mousePosition.x)
                            , 0
                            , (int)Mathf.Ceil(mousePosition.z));
    }
}
