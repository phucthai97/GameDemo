using System;
using UnityEngine;
using System.Collections.Generic;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    private Renderer cellValidIndicatorRenderer;
    [SerializeField]
    private List<Vector3> multiPosValidIndicator = new List<Vector3>
    {
        new Vector3(0,0.21f,0),
        new Vector3(0,0,5f)
    };

    private void Start()
    {
        cellIndicator.gameObject.SetActive(false);
        cellValidIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void UpdateGridIndicator(Vector3 position, Vector2Int size, bool validity)
    {

        cellIndicator.SetActive(true);
        //Move Grid Indicator
        MoveGridIndicator(position, size);
        //Apply feedback to Grid Indicator
        ApplyFeedbackToGridIndicator(validity);
    }

    public void UpdateGridIndicatorMod(Vector3 position, Vector2Int size, bool validity, int typeOfPlacement)
    {

        cellIndicator.SetActive(true);
        //Move Grid Indicator
        MoveGridIndicatorMod(position, size, typeOfPlacement);
        //Apply feedback to Grid Indicator
        ApplyFeedbackToGridIndicator(validity);
    }

    private void MoveGridIndicator(Vector3 position, Vector2Int size)
    {
        cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        PlacementChecker placementChecker = FindObjectOfType<PlacementChecker>();
        Vector3 posIndicator = placementChecker.ObjectAlignment(position, size, 0);

        cellIndicator.transform.position = new Vector3(posIndicator.x, 0.21f, posIndicator.z);
    }

    private void MoveGridIndicatorMod(Vector3 position, Vector2Int size, int typeOfPlacement)
    {
        cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        PlacementChecker placementChecker = FindObjectOfType<PlacementChecker>();
        Vector3 posIndicator = placementChecker.ObjectAlignment(position, size, typeOfPlacement);

        if (typeOfPlacement == 0)
        {
            Quaternion desiredRotation = Quaternion.Euler(0, 0, 0);
            cellIndicator.transform.rotation = desiredRotation;
            cellIndicator.transform.position = new Vector3(posIndicator.x, 0.21f, posIndicator.z);
        }
        else if (typeOfPlacement == 1)
        {
            Quaternion desiredRotation = Quaternion.Euler(90f, 0, 0);
            cellIndicator.transform.rotation = desiredRotation;
            cellIndicator.transform.position = new Vector3(posIndicator.x, posIndicator.y, 5f);
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != null)
            objectPlacer.currentTouchableObj.SetActiveEditIndicator(false);

    }

    private void ApplyFeedbackToGridIndicator(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        cellValidIndicatorRenderer.material.color = c;
        c.a = 0.4f;
    }
}