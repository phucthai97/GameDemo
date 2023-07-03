using System;
using UnityEngine;
using System.Collections.Generic;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;
    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        cellIndicator.gameObject.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void UpdateGridIndicator(Vector3 position, Vector2Int size, bool validity)
    {
        cellIndicator.SetActive(true);
        //Move Grid Indicator
        MoveGridIndicator(position, size);
        //Apply feedback to Grid Indicator
        Vector3Int gridPosition = new Vector3Int((int)position.x, 0, (int)position.z);
        ApplyFeedbackToGridIndicator(validity);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicator.transform.position = new Vector3();
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != null)
            objectPlacer.currentTouchableObj.TurnONOFFIndicator(false);

        if (previewObject != null)
            Destroy(previewObject);
    }

    private void MoveGridIndicator(Vector3 position, Vector2Int size)
    {
        cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        position = new Vector3(position.x, 0.21f, position.z + (size.y - 1));
        //position = new Vector3(position.x - 1, 0.21f, position.z);
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedbackToGridIndicator(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.4f;
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToGridIndicator(false);
    }
}