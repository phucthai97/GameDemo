using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementUIControl : MonoBehaviour
{
    [SerializeField] GameObject btnDone;
    [SerializeField] GameObject btnRotate;
    [SerializeField] GameObject btnRemoveFur;
    [SerializeField] GameObject btnRemoveFlo;

    // [SerializeField] GameObject btnRemoveFloor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RunbtnDone()
    {
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        placementSystem.StopPlacement();
        TurnOffEditButtonObject();
    }

    public void TurnOffEditButtonObject()
    {
        btnDone.SetActive(false);
        btnRotate.SetActive(false);
        btnRemoveFur.SetActive(false);
        btnRemoveFlo.SetActive(false);
    }

    public void TurnOnEditBtnObject(int ID)
    {
        if(ID < 10000)
            btnRemoveFur.SetActive(true);
        else if(ID >= 10000)
            btnRemoveFlo.SetActive(true);

        btnDone.SetActive(true);
        btnRotate.SetActive(true);
    }

}
