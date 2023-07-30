using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    [SerializeField] public GameObject editIndicator;
    public bool mouseIsPressed = false;
    [SerializeField] public int indexPrefabs;
    public PlacementChecker placementChecker;
    [SerializeField] public Vector2Int currentSize;
    [SerializeField] public Vector3Int currentGridPos;
    [SerializeField] public bool floorPlacement;
    [SerializeField] public int typeOfPlacement;
    [SerializeField] public Vector3 constantPos;

    // Start is called before the first frame update
    void Start()
    {
        placementChecker = FindObjectOfType<PlacementChecker>();
        SetIndicator();
    }

    public void SetParas(int indexPrefabs, int typeOfPlacement, Vector2Int size, Vector3Int gridPosition)
    {
        this.indexPrefabs = indexPrefabs;
        currentSize = size;
        currentGridPos = gridPosition;
        this.typeOfPlacement = typeOfPlacement;

        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        placementChecker = FindObjectOfType<PlacementChecker>();
        placementChecker.maxHeightIndicator = placementSystem.database.objectsData[indexPrefabs].maxHeight;
    }

    private void OnMouseDown()
    {
        placementChecker.CheckAndSetCountClicked(this);
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();

        if (placementChecker.mode == PlacementChecker.Mode.Moving)
        {
            placementChecker.HandleMouseDownPlacement(this);
            placementSystem.StartMoving(this);
            //First Clicked for choose, Second clicked for moving
            if (placementChecker.countClicked >= 1)
            {
                mouseIsPressed = true;
            }
        }
    }

    private void OnMouseUp()
    {
        //First Clicked for choose, Second clicked for moving
        if (placementChecker.countClicked >= 1 && placementChecker.mode == PlacementChecker.Mode.Moving)
        {
            mouseIsPressed = false;
            placementChecker.HandleMouseUpPlacement(this, indexPrefabs);
        }
    }

    private void SetIndicator()
    {
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();

        // If ID belongs to furniture -> It's mean ID < 10000
        if (placementSystem.database.objectsData[indexPrefabs].ID < 10000)
        {
            int index = placementSystem.database.dataPrefabs.FindIndex(data => data.Name == "editindicator");
            editIndicator = Instantiate(placementSystem.database.dataPrefabs[index].Prefab);

            Vector3 rawPos = gameObject.transform.position;
            if (typeOfPlacement == 0)
                editIndicator.transform.position = new Vector3(rawPos.x
                                                            , gameObject.transform.position.y + placementChecker.maxHeightIndicator
                                                            , rawPos.z);
            else if (typeOfPlacement == 1)
                editIndicator.transform.position = new Vector3(rawPos.x
                                                            , gameObject.transform.position.y + placementChecker.maxHeightIndicator
                                                            , 4.9f);
            else if (typeOfPlacement == 2)
                editIndicator.transform.position = new Vector3(4f
                                                            , rawPos.y
                                                            , rawPos.z);

            //Set transform for edit indicator
            editIndicator.transform.SetParent(gameObject.transform);
        }
    }

    public void MovingEditIndicator()
    {
        if (editIndicator != null)
        {
            if (editIndicator.activeSelf)
            {
                // Move indicator object up and down follow movingUp variable
                if (placementChecker.movingUp)
                    editIndicator.transform.Translate(Vector3.up * 2f * Time.deltaTime);
                else
                    editIndicator.transform.Translate(Vector3.down * 2f * Time.deltaTime);

                // Check if the object has reached its maximum or minimum height, 
                // and change the direction of motion
                if (editIndicator.transform.localPosition.y >= placementChecker.maxHeightIndicator)
                    placementChecker.movingUp = false;
                else if (editIndicator.transform.localPosition.y <= placementChecker.maxHeightIndicator - 0.3f)
                    placementChecker.movingUp = true;
            }
        }
    }

    public void SetActiveEditIndicator(bool val)
    {
        if (editIndicator != null)
            editIndicator.SetActive(val);
    }

    public void RotateObject()
    {
        gameObject.transform.Rotate(Vector3.up, 90f);
        //Update preview grid indicator

        //Set currentSize when the object rotates
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        Vector2Int Size = placementSystem.database.objectsData[indexPrefabs].Size;
        if (gameObject.transform.localRotation.y == 0
        || gameObject.transform.localRotation.y == 1
        || gameObject.transform.localRotation.y == -1)
            currentSize = Size;
        else
            currentSize = new Vector2Int(Size.y, Size.x);

        PreviewSystem previewSystem = FindObjectOfType<PreviewSystem>();
        previewSystem.UpdateGridIndicator(currentGridPos, currentSize, true);
    }
}
