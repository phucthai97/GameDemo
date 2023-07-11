using System;
using UnityEngine;
using System.Collections.Generic;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        cellIndicator.gameObject.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void UpdateGridIndicator(Vector3 position, Vector2Int size, bool validity)
    {
        //Debug.Log($"Pos is {position}");
        cellIndicator.SetActive(true);
        //Move Grid Indicator
        MoveGridIndicator(position, size);
        //Apply feedback to Grid Indicator
        ApplyFeedbackToGridIndicator(validity);
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != null)
            objectPlacer.currentTouchableObj.TurnONOFFIndicator(false);

    }

    private void MoveGridIndicator(Vector3 position, Vector2Int size)
    {
        cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        //position = new Vector3(position.x, 0.21f, position.z + (size.y - 1));
        //position = new Vector3(position.x, 0.21f, position.z);
        //cellIndicator.transform.position = position;

        PlacementChecker placementChecker = FindObjectOfType<PlacementChecker>();
        Vector3 posIndicator = placementChecker.ObjectAlignment(position, size);
        cellIndicator.transform.position = new Vector3(posIndicator.x, 0.21f, posIndicator.z);
    }

    private void ApplyFeedbackToGridIndicator(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.4f;
    }
}