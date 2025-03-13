using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField]
    public float GridCellSize { get; } = 1f;

    private Dictionary<Vector2Int, Building> _occupiedCells = new Dictionary<Vector2Int, Building>();

    public void Initialize()
    {
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / GridCellSize);
        int y = Mathf.RoundToInt(worldPosition.y / GridCellSize);
        return new Vector2Int(x, y);
    }

    public Building GetBuildingAtGridPosition(Vector2Int gridPosition)
    {
        if (_occupiedCells.ContainsKey(gridPosition))
        {
            return _occupiedCells[gridPosition];
        }
        return null;
    }

    public bool IsAreaFree(Vector2Int gridPosition)
    {
        if (_occupiedCells.ContainsKey(gridPosition))
        {
            return false;
        }
        return true;
    }

    public bool IsAreaFree(Vector2Int gridPosition, BuildingData buildingData)
    {
        Vector2Int size = buildingData.Size;

        int StartXPoisition = -Mathf.RoundToInt(size.x / 2);
        int StartYPoisition = -Mathf.RoundToInt(size.y / 2);

        int EndXPoisition = size.x + StartXPoisition;
        int EndYPoisition = size.y + StartYPoisition;

        for (int x = StartXPoisition; x < EndXPoisition; x++)
        {
            for (int y = StartYPoisition; y < EndYPoisition; y++)
            {
                Vector2Int checkPos = gridPosition + new Vector2Int(x, y);
                if (_occupiedCells.ContainsKey(checkPos))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void OccupyCells(Vector2Int gridPosition, BuildingData buildingData, Building buildingInstance)
    {
        Vector2Int size = buildingData.Size;

        int StartXPoisition = -Mathf.RoundToInt(size.x / 2);
        int StartYPoisition = -Mathf.RoundToInt(size.y / 2);

        int EndXPoisition = size.x + StartXPoisition;
        int EndYPoisition = size.y + StartYPoisition;

        for (int x = StartXPoisition; x < EndXPoisition; x++)
        {
            for (int y = StartYPoisition; y < EndYPoisition; y++)
            {
                Vector2Int cellPos = gridPosition + new Vector2Int(x, y);
                _occupiedCells[cellPos] = buildingInstance;
            }
        }
    }

    public void FreeCells(Building building)
    {
        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var cell in _occupiedCells)
        {
            if (cell.Value == building)
            {
                keysToRemove.Add(cell.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            _occupiedCells.Remove(key);
        }
    }

    // Для дебага (опционально)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (var cell in _occupiedCells)
        {
            Vector3 pos = new Vector3(
                cell.Key.x * GridCellSize,
                cell.Key.y * GridCellSize,
                0
            );
            Gizmos.DrawWireCube(pos, Vector3.one * GridCellSize);
        }
    }
}
