using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //Input manager by input device: mouse, keyboard
    [SerializeField] private InputManager inputManager;

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

    //For storage 2 kind of GridData
    public GridData floorData, furnitureData;

    //Current gridlayer
    public enum LayerType { Floor, Wall1, Wall2 };
    [SerializeField] public LayerType layerType;

    //Declare interface
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        placementChecker.SetActiveGridVisualization("turnoffall");
        placementUIControl.TurnOffEditButtonObject();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        //Set active tools
        placementUIControl.TurnOnEditBtnObject(ID);
        buildingState = new PlacementState(ID,
                                        preview,
                                        database,
                                        objectPlacer,
                                        placementChecker);

        //Passing Place_Structure into On Clicked delegate
        inputManager.OnClicked += PlaceStructure;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void StartMoving(TouchableObject touchableObject)
    {
        StopPlacement();
        //Set active tools
        //gridVisualization.SetActive(true);
        placementUIControl.TurnOnEditBtnObject(database.objectsData[touchableObject.indexPrefabs].ID);
        buildingState = new MovingState(preview,
                                        placementChecker,
                                        this,
                                        touchableObject);
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        //gridVisualization.SetActive(true);
        placementChecker.RemoveFurnitureObject();
    }

    public void StartRemovingFloor()
    {
        StopPlacement();
        //Set de-active tools
        //gridVisualization.SetActive(true);
        placementUIControl.TurnOnRemovingFloor();
        buildingState = new RemovingFloor(preview, floorData, furnitureData, objectPlacer);
        //Passing Place_Structure into On Clicked delegate
        inputManager.OnClicked += PlaceStructure;
        //Passing Stop_Placement into On Exit delegate
        inputManager.OnExit += StopPlacement;
    }

    public void RotateObject()
    {
        if (objectPlacer.currentTouchableObj != null)
        {
            //gridVisualization.SetActive(true);
            objectPlacer.currentTouchableObj.RotateObject();
        }
    }

    private void PlaceStructure()
    {
        //If pointer of mouse click on UI area -> do nothing!
        if (inputManager.IsPointerOverUI())
            return;

        buildingState.OnAction(GetCurrentGridPos());
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        //Set De-active tools
        placementChecker.SetActiveGridVisualization("turnoffall");
        placementUIControl.SetColorBtnRemoveFloor(false);
        //Deactive indicator,...
        buildingState.EndState();

        //Getting out the Place_Structure function from delegate On Clicked
        inputManager.OnClicked -= PlaceStructure;
        //Getting out the Stop_Placement function from delegate On Exit
        inputManager.OnExit -= StopPlacement;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        // if (lastDetectedPosition != GetGridCurrentPos())
        // {
        buildingState.UpdateState(GetCurrentGridPos());
        //Debug.Log($"GetGridCurrentPos is {GetCurrentGridPos()}");
        //}
    }

    private Vector3Int GetCurrentGridPos()
    {
        //Get hit on place by position of mouse
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        Sphere.transform.position = mousePosition;

        Vector3Int CurrentGridPos = new Vector3Int();

        if (inputManager.numberCurrentLayer == 6)
        {
            layerType = LayerType.Floor;
            CurrentGridPos = new Vector3Int((int)Mathf.Floor(mousePosition.x)
                                            , 0
                                            , (int)Mathf.Ceil(mousePosition.z));
        }
        else if (inputManager.numberCurrentLayer == 7)
        {
            layerType = LayerType.Wall1;
            CurrentGridPos = new Vector3Int((int)Mathf.Ceil(mousePosition.x)
                                            , (int)Mathf.Floor(mousePosition.y)
                                            , 0);
        }
        else if (inputManager.numberCurrentLayer == 8)
        {
            layerType = LayerType.Wall2;
            CurrentGridPos = new Vector3Int(0
                                            , (int)Mathf.Floor(mousePosition.y)
                                            , (int)Mathf.Ceil(mousePosition.z));
        }

        //Get current grid position
        return CurrentGridPos;
    }
}
