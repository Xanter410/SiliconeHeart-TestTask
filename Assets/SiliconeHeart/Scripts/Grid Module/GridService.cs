using System.Collections.Generic;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Grid
{
    public class GridService : IService
    {
        public int GridCellSize { get; private set; }

        private Dictionary<Vector2Int, object> _occupiedCells = new Dictionary<Vector2Int, object>();

        public GridService(int gridCellSize = 1) 
        {
            GridCellSize = gridCellSize;
        }

        public Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / GridCellSize);
            int y = Mathf.RoundToInt(worldPosition.y / GridCellSize);
            return new Vector2Int(x, y);
        }

        public bool TryGetObjectAtGridPosition(Vector2Int gridPosition, out object value)
        {
            if (_occupiedCells.ContainsKey(gridPosition))
            {
                value = _occupiedCells[gridPosition];
                return true;
            }

            value = null;
            return false;
        }

        public bool IsAreaFree(Vector2Int gridPosition)
        {
            if (_occupiedCells.ContainsKey(gridPosition))
            {
                return false;
            }
            return true;
        }

        public bool IsAreaFree(Vector2Int gridPosition, Vector2Int size)
        {
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

        public void OccupyCells(Vector2Int gridPosition, Vector2Int size , object value)
        {
            int StartXPoisition = -Mathf.RoundToInt(size.x / 2);
            int StartYPoisition = -Mathf.RoundToInt(size.y / 2);

            int EndXPoisition = size.x + StartXPoisition;
            int EndYPoisition = size.y + StartYPoisition;

            for (int x = StartXPoisition; x < EndXPoisition; x++)
            {
                for (int y = StartYPoisition; y < EndYPoisition; y++)
                {
                    Vector2Int cellPos = gridPosition + new Vector2Int(x, y);
                    _occupiedCells[cellPos] = value;
                }
            }
        }

        public void FreeCells(object value)
        {
            List<Vector2Int> keysToRemove = new List<Vector2Int>();

            foreach (var cell in _occupiedCells)
            {
                if (cell.Value == value)
                {
                    keysToRemove.Add(cell.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _occupiedCells.Remove(key);
            }
        }
    }
}