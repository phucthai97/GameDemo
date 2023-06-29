using System;
using UnityEngine;
using System.Collections.Generic;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;
    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;
    GameObject currentPrefab;
    [SerializeField] GameObject lastObjectChoose;
    [SerializeField] List<Material> lastListMaterials;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.gameObject.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    // public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    // {
    //     currentPrefab = prefab;
    //     previewObject = Instantiate(prefab);
    //     PreparePreview(previewObject);
    //     PrepareCursor(size);
    //     cellIndicator.SetActive(true);
    // }

    // public void StartShowingGridIndicator(Vector3 position, Vector2Int size)
    // {
    //     cellIndicator.SetActive(true);
    //     MoveGridIndicator(position, size);
    // }

    public void UpdateGridIndicator(Vector3 position, Vector2Int size, bool validity)
    {
        cellIndicator.SetActive(true);
        //Move Grid Indicator
        MoveGridIndicator(position, size);
        //Apply feedback to Grid Indicator
        Vector3Int gridPosition = new Vector3Int((int)position.x, 0 , (int)position.z);
        ApplyFeedbackToGridIndicator(validity);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicator.transform.position = new Vector3();
            //cellIndicator.GetComponentInChildren<Renderer>().material.mainTextureScale = size;

        }
    }

    // private void PreparePreview(GameObject previewObject)
    // {
    //     Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
    //     foreach (Renderer renderer in renderers)
    //     {
    //         Material[] materials = renderer.materials;
    //         for (int i = 0; i < materials.Length; i++)
    //         {
    //             materials[i] = previewMaterialInstance;
    //         }
    //         renderer.materials = materials;
    //     }
    // }

    // private void SaveLastMaterials(GameObject previewObject)
    // {
    //     lastListMaterials = new List<Material>();
    //     Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
    //     foreach (Renderer renderer in renderers)
    //     {
    //         lastListMaterials.AddRange(renderer.materials);
    //     }
    // }

    // private void LoadLastMaterials(GameObject previewObject)
    // {
    //     Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
    //     int n = 0;
    //     foreach (Renderer renderer in renderers)
    //     {
    //         Material[] Loadmaterials = new Material[renderer.materials.Length];
    //         for (int i = 0; i < renderer.materials.Length; i++)
    //         {
    //             Loadmaterials[i] = lastListMaterials[n];
    //             n++;
    //         }
    //         renderer.materials = Loadmaterials;
    //     }
    //     lastListMaterials.Clear();
    // }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer.currentTouchableObj != null)
        {
            objectPlacer.currentTouchableObj.editIndicator.SetActive(false);
            Debug.Log($"Set active false currentTouchableObj");
        }

        if (previewObject != null)
            Destroy(previewObject);
    }

    // public void UpdatePosition(Vector3 position, bool validity, Vector2Int size)
    // {
    //     if (previewObject != null)
    //     {
    //         MovePreview(position, size);
    //         ApplyFeedbackToPreview(validity);
    //     }
    //     MoveCursor(position, size);
    //     ApplyFeedbackToCursor(validity);
    // }

    // //For moving object feature
    // public void UpdateCursor(Vector3 position, bool validity, GameObject specifiedObject)
    // {
    //     if (validity)
    //     {
    //         //Debug.Log($"At pos {position} can move with index gameobject {specifiedObject}");
    //         ApplyFeedbackToPreview(validity);
    //         SaveLastMaterials(specifiedObject);
    //         PreparePreview(specifiedObject);
    //         lastObjectChoose = specifiedObject;
    //     }
    // }

    // public void PreUpdateCursor(bool validity)
    // {
    //     if (lastObjectChoose != null && validity)
    //     {
    //         LoadLastMaterials(lastObjectChoose);
    //         lastObjectChoose = null;
    //     }
    // }

    private void MoveGridIndicator(Vector3 position, Vector2Int size)
    {
        cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
        position = new Vector3(position.x, 0.21f, position.z + (size.y - 1));
        //position = new Vector3(position.x - 1, 0.21f, position.z);
        cellIndicator.transform.position = position;
    }

    // private void MovePreview(Vector3 position, Vector2Int size)
    // {
    //     previewObject.transform.position = new Vector3(position.x
    //                                                 , currentPrefab.transform.position.y
    //                                                 , position.z);
    // }

    // private void ApplyFeedbackToPreview(bool validity)
    // {
    //     Color c = validity ? Color.green : Color.red;
    //     cellIndicatorRenderer.material.color = c;
    //     c.a = 0.4f;
    //     previewMaterialInstance.color = c;
    // }

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
