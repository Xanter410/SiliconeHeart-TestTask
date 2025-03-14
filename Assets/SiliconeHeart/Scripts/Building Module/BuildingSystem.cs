using System.Collections.Generic;
using SiliconeHeart.Grid;
using SiliconeHeart.Save;
using UnityEngine;

public enum BuildingMode
{
    None,
    Placing,
    Deleting
}

public class BuildingSystem : MonoBehaviour
{
    public BuildingMode CurrentMode => _currentMode;

    [Header("References")]
    [SerializeField] private GameObject _buildingGhostPrefab;
    [SerializeField] private LayerMask _buildingLayerMask;
    [SerializeField] private GameObject _tilemapBuildingGrid;

    [SerializeField] private DeletingMarker _deletingMarker;
    
    private const string saveKey = "BuildingsData";

    private BuildingData _currentBuildingData;
    private GameObject _currentGhostGameObject;
    private BuildingGhost _currentGhost;

    private BuildingMode _currentMode = BuildingMode.None;

    private Dictionary<Building, GameObject> _activeBuildingsMap = new Dictionary<Building, GameObject>();

    public void Initialize()
    {
        _tilemapBuildingGrid.SetActive(false);

        _deletingMarker.Initialize(this);

        TryLoadBuildings();

        ServiceLocator.Current.Get<InputHandler>().leftClick += HandleAction;
    }

    public GameObject GetGameObjectByBuilding(Building building)
    {
        return _activeBuildingsMap[building];
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

    private void HandleAction()
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
        _currentGhost.Initialize(_currentBuildingData);
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
        Building buildingModel = new(
            _currentBuildingData.Id, 
            newBuilding.transform.position.x, 
            newBuilding.transform.position.y);

        AddToBuildingMaps(buildingModel, newBuilding, true);

        var gridService = ServiceLocator.Current.Get<GridService>();

        Vector2Int gridPos = gridService.WorldToGridPosition(newBuilding.transform.position);

        gridService.OccupyCells(gridPos, _currentBuildingData.Size, buildingModel);

        SetMode(BuildingMode.None);
    }

    private void TryDeleteBuilding()
    {
        InputHandler inputHandler = ServiceLocator.Current.Get<InputHandler>();

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputHandler.GetMousePosition());

        var gridService = ServiceLocator.Current.Get<GridService>();

        Vector2Int gridPositionUnderMouse = gridService.WorldToGridPosition(mouseWorldPos);

        if (!gridService.IsAreaFree(gridPositionUnderMouse))
        {
            gridService.TryGetObjectAtGridPosition(gridPositionUnderMouse, out object building);

            RevomeAndDestroyFromBuidingMaps((Building)building);
        }
    }

    private void SaveBuildings()
    {
        List<Building> saveBuildings = new List<Building>();

        foreach (var building in _activeBuildingsMap.Keys)
        {
            saveBuildings.Add(building);
        }

        ServiceLocator.Current.Get<IStorage>().Save(saveKey, saveBuildings);
    }

    public void TryLoadBuildings()
    {
        ServiceLocator.Current.Get<IStorage>().Load(saveKey, (List<Building> saveBuildings) =>
        {
            foreach (var saveBuilding in saveBuildings)
            {
                BuildingData buildingData = ServiceLocator.Current.Get<BuildingDataService>().GetBuildingDataByID(saveBuilding.DataId);

                Vector3 spawnPos = new Vector3(saveBuilding.PositionX, saveBuilding.PositionY, 0);

                GameObject newBuilding = Instantiate(
                    buildingData.Prefab,
                    spawnPos,
                    Quaternion.identity
                );

                AddToBuildingMaps(saveBuilding, newBuilding, false);

                var gridService = ServiceLocator.Current.Get<GridService>();

                Vector2Int gridPos = gridService.WorldToGridPosition(spawnPos);
                gridService.OccupyCells(gridPos, buildingData.Size, saveBuilding);
            }
        });
    }    

    private void AddToBuildingMaps(Building buildingModel, GameObject buildingViewInstance, bool isNeedToSave)
    {
        _activeBuildingsMap.Add(buildingModel, buildingViewInstance);

        int newBuildingIndex = _activeBuildingsMap.Count;

        if (isNeedToSave)
        {
            SaveBuildings();
        }
    }

    private void RevomeAndDestroyFromBuidingMaps(Building building)
    {

        Destroy(_activeBuildingsMap[building]);

        _activeBuildingsMap.Remove(building);

        ServiceLocator.Current.Get<GridService>().FreeCells(building);

        SaveBuildings();
    }
}
