using System.Collections.Generic;
using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Save;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Building
{
    public class BuildingContainer
    {
        private Dictionary<Building, GameObject> _activeBuildingsMap = new Dictionary<Building, GameObject>();
        
        private const string saveKey = "BuildingsData";

        BuildingSystem _buildingSystem;

        public BuildingContainer()
        {
            TryLoadBuildings();
        }
        public GameObject GetGameObjectByBuilding(Building building)
        {
            return _activeBuildingsMap[building];
        }

        public void AddToBuildingMaps(Building buildingModel, GameObject buildingViewInstance, bool isNeedToSave)
        {
            _activeBuildingsMap.Add(buildingModel, buildingViewInstance);

            int newBuildingIndex = _activeBuildingsMap.Count;

            if (isNeedToSave)
            {
                SaveBuildings();
            }
        }

        public void RevomeFromBuidingMaps(Building building)
        {
            _activeBuildingsMap.Remove(building);

            ServiceLocator.Current.Get<GridService>().FreeCells(building);

            SaveBuildings();
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

        private void TryLoadBuildings()
        {
            ServiceLocator.Current.Get<IStorage>().Load(saveKey, (List<Building> saveBuildings) =>
            {
                foreach (var saveBuilding in saveBuildings)
                {
                    BuildingData buildingData = ServiceLocator.Current.Get<BuildingDataService>().GetBuildingDataByID(saveBuilding.DataId);

                    Vector3 spawnPos = new Vector3(saveBuilding.PositionX, saveBuilding.PositionY, 0);

                    GameObject newBuilding = Object.Instantiate(
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
    }
}
