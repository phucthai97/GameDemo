using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    [SerializeField] public GameObject editIndicator;
    public bool mouseIsPressed = false;
    [SerializeField] public int indexPrefabs;
    public PlacementChecker placementChecker;

    // Start is called before the first frame update
    void Start()
    {
        placementChecker = FindObjectOfType<PlacementChecker>();
        SetIndicator();
    }

    public void SetIndexPrefabs(int indexPrefabs)
    {
        this.indexPrefabs = indexPrefabs;
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        placementChecker = FindObjectOfType<PlacementChecker>();
        placementChecker.maxHeightIndicator = placementSystem.database.objectsData[indexPrefabs].maxHeight;
    }

    private void OnMouseDown()
    {
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        placementSystem.StartMoving(this, indexPrefabs);
        mouseIsPressed = true;
        placementChecker.HandleMouseDownPlacement(gameObject);

    }

    private void OnMouseUp()
    {
        mouseIsPressed = false;
        placementChecker.HandleMouseUpPlacement(gameObject, indexPrefabs);
    }

    private void SetIndicator()
    {
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();

        // If ID belongs to furniture
        if (placementSystem.database.objectsData[indexPrefabs].ID < 10000)
        {
            int index = placementSystem.database.dataPrefabs.FindIndex(data => data.Name == "editindicator");
            editIndicator = Instantiate(placementSystem.database.dataPrefabs[index].Prefab);
            editIndicator.transform.position = new Vector3(
                                            transform.position.x
                                            , placementChecker.maxHeightIndicator + gameObject.transform.position.y
                                            , transform.position.z);
            editIndicator.transform.SetParent(gameObject.transform);
        }
    }

    public void EditIndicator()
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
}
