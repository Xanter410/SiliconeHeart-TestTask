using System.Collections.Generic;
using UnityEngine;

public enum BuildingMode
{
    None,
    Placing,
    Deleting
}

public class BuildingManager : MonoBehaviour
{
    public BuildingMode CurrentMode => _currentMode;

    [Header("References")]
    [SerializeField] private GameObject _buildingGhostPrefab;
    [SerializeField] private LayerMask _buildingLayerMask;
    [SerializeField] private GameObject _tilemapBuildingGrid;

    [SerializeField] private DeletingMarker _deletingMarker;

    private GridSystem _gridSystem;
    private InputHandler _inputHandler;
    private SaveManager _saveManager;
    private BuildingDataService _buildingDataService;

    private BuildingData _currentBuildingData;
    private GameObject _currentGhostGameObject;
    private BuildingGhost _currentGhost;

    private BuildingMode _currentMode = BuildingMode.None;
    private List<Building> _activeBuildings = new List<Building>();

    public void Initialize(GridSystem gridSystem, InputHandler inputHandler, BuildingDataService buildingDataService, SaveManager saveManager)
    {
        _gridSystem = gridSystem;
        _inputHandler = inputHandler;
        _buildingDataService = buildingDataService;
        _saveManager = saveManager;

        _tilemapBuildingGrid.SetActive(false);

        _deletingMarker.Initialize(_gridSystem, _inputHandler);
    }

    public void TogglePlaceMode(BuildingData data)
    {
        if (_currentMode == BuildingMode.Placing)
        {
            SetMode(BuildingMode.None);
            Destroy(_currentGhostGameObject);
            _currentBuildingData = null;
        }
        else
        {
            SetMode(BuildingMode.Placing);
            _currentBuildingData = data;
            CreateGhost();
        }
    }

    public void ToggleDeleteMode()
    {
        if (_currentMode == BuildingMode.Deleting)
        {
            SetMode(BuildingMode.None);
        }
        else
        {
            SetMode(BuildingMode.Deleting);
        }
    }

    public void HandleAction()
    {
        if (_currentMode == BuildingMode.Placing)
        {
            TryPlaceBuilding();
        }
        else if (_currentMode == BuildingMode.Deleting)
        {
            TryDeleteBuilding();
        }
    }
    private void SetMode(BuildingMode mode)
    {
        _currentMode = mode;

        if (mode != BuildingMode.Placing && _currentGhostGameObject != null)
        {
            Destroy(_currentGhostGameObject);
            _tilemapBuildingGrid.SetActive(false);
        }

        if (mode == BuildingMode.Placing)
        {
            _tilemapBuildingGrid.SetActive(true);
        }

        if (mode == BuildingMode.Deleting)
        {
            _deletingMarker.SetEnabled(true);
        }
        else
        {
            _deletingMarker.SetEnabled(false);
        }
    }

    private void CreateGhost()
    {
        _currentGhostGameObject = Instantiate(_buildingGhostPrefab);
        _currentGhost = _currentGhostGameObject.GetComponent<BuildingGhost>();
        _currentGhost.Initialize(_currentBuildingData, _gridSystem, _inputHandler);
    }

    private void TryPlaceBuilding()
    {
        if (!_currentGhost.IsValid)
            return;

        GameObject newBuilding = Instantiate(
            _currentBuildingData.Prefab,
            _currentGhostGameObject.transform.position,
            Quaternion.identity
        );

        Building buildingComponent = newBuilding.GetComponent<Building>();
        buildingComponent.Initialize(_currentBuildingData);

        Vector2Int gridPos = _gridSystem.WorldToGridPosition(_currentGhostGameObject.transform.position);

        _gridSystem.OccupyCells(gridPos, _currentBuildingData, buildingComponent);
        _activeBuildings.Add(buildingComponent);

        _saveManager.SaveBuildings(_activeBuildings);
        SetMode(BuildingMode.None);
    }

    private void TryDeleteBuilding()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_inputHandler.GetMousePosition());

        Vector2Int gridPositionUnderMouse = _gridSystem.WorldToGridPosition(mouseWorldPos);

        if (!_gridSystem.IsAreaFree(gridPositionUnderMouse))
        {
            Building building = _gridSystem.GetBuildingAtGridPosition(gridPositionUnderMouse);

            _gridSystem.FreeCells(building);
            _activeBuildings.Remove(building);
            Destroy(building.gameObject);
            _saveManager.SaveBuildings(_activeBuildings);
        }
    }

    public void SpawnSavedBuildings(List<BuildingSaveData> savedData)
    {
        foreach (var data in savedData)
        {
            BuildingData buildingData = _buildingDataService.GetBuildingDataByID(data.BuildingID);

            Vector3 spawnPos = data.Position;

            GameObject newBuilding = Instantiate(
                buildingData.Prefab,
                spawnPos,
                Quaternion.identity
            );

            Building buildingComponent = newBuilding.GetComponent<Building>();
            buildingComponent.Initialize(buildingData);

            Vector2Int gridPos = _gridSystem.WorldToGridPosition(spawnPos);
            _gridSystem.OccupyCells(gridPos, buildingData, buildingComponent);

            _activeBuildings.Add(buildingComponent);
        }
    }
}
