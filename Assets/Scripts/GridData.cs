using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectsIndex, int typeOfPlacement)
    {
        List<Vector3Int> positionToOccupy = CalculatePositionsMod(gridPosition, objectSize, typeOfPlacement);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectsIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                Debug.Log($"Debug: Dictionary already contains this cell position {pos}");
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
            Debug.Log($"Occupied is {pos}");
        }
    }

    public bool CanPlaceObjectAtMod(Vector3Int gridPosition, Vector2Int objectSize, int typeOfPlacement)
    {
        //Debug.Log($"CanPlaceObjectAt is {gridPosition}");
        List<Vector3Int> positionOccupy = CalculatePositionsMod(gridPosition, objectSize, typeOfPlacement);
        
        foreach (var pos in positionOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int z = 0; z < objectSize.y; z++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, z));
            }
        }
        return returnVal;
    }

    private List<Vector3Int> CalculatePositionsMod(Vector3Int gridPosition, Vector2Int objectSize, int typeOfPlacement)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int z = 0; z < objectSize.y; z++)
            {
                if(typeOfPlacement == 0)
                    returnVal.Add(gridPosition + new Vector3Int(x, 0, z));
                else if(typeOfPlacement == 1)
                    returnVal.Add(gridPosition + new Vector3Int(x, z, 0));
            }
        }

        // foreach(var a in returnVal)
        // {
        //     Debug.Log($"returnVal is {a}");
        // }
        return returnVal;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlaceObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlaceObjectIndex { get; private set; }

    //Create constructor
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectsIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlaceObjectIndex = placedObjectsIndex;
    }
}
