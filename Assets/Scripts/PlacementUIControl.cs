using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUIControl : MonoBehaviour
{
    [SerializeField] GameObject btnDone;
    [SerializeField] GameObject btnRotate;
    [SerializeField] GameObject btnRemoveFur;
    [SerializeField] GameObject btnRemoveFlo;

    public void RunbtnDone()
    {
        //Stop placement
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        placementSystem.StopPlacement();

        //Reset mode moving
        PlacementChecker placementChecker = FindObjectOfType<PlacementChecker>();
        placementChecker.mode = PlacementChecker.Mode.Moving;

        //Re-new the current object for prevent tap first to select that object
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        placementChecker.AddFurniture(objectPlacer);
        placementChecker.RenewCurrentTouchableObject();

        //Reset clickmouse
        placementChecker.countClicked = 0;

        //Reset color of button
        SetColorBtnRemoveFloor(false);

        //Turn Off Edit Button
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
        if (ID < 10000)
        {
            btnRemoveFur.SetActive(true);
            btnRemoveFlo.SetActive(false);
        }
        else if (ID >= 10000)
        {
            btnRemoveFur.SetActive(false);
            btnRemoveFlo.SetActive(true);
        }

        btnDone.SetActive(true);
        btnRotate.SetActive(true);
    }

    public void TurnOnRemovingFloor()
    {
        btnRemoveFlo.SetActive(true);
        btnDone.SetActive(true);
        SetColorBtnRemoveFloor(true);
    }

    public void SetColorBtnRemoveFloor(bool greenColor)
    {
        Image img = btnRemoveFlo.GetComponent<Image>();
        if (greenColor)
            img.color = new Color32(96, 255, 13, 255);
        else
            img.color = new Color32(255, 255, 255, 255);
    }
}
