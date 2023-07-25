using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] public int numberCurrentLayer;

    //Declare On Clicked & On Exit is function delegate
    public event Action OnClicked, OnExit;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Callback function
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            //Callback function
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 touchPos = Input.mousePosition;
        //mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(touchPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
            GameObject hitObject = hit.collider.gameObject;
            numberCurrentLayer = hitObject.layer;
        }
        return lastPosition;
    }
}
