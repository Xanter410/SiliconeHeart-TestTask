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
        private readonly Dictionary<Building, GameObject> _activeBuildingsMap = new();
        
        private const string saveKey = "BuildingsData";

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

            if (isNeedToSave)
            {
                SaveBuildings();
            }
        }

        public void RevomeFromBuidingMaps(Building building)
        {
            _ = _activeBuildingsMap.Remove(building);

            ServiceLocator.Current.Get<GridService>().FreeCells(building);

            SaveBuildings();
        }

        private void SaveBuildings()
        {
            List<Building> saveBuildings = new();

            foreach (Building building in _activeBuildingsMap.Keys)
            {
                saveBuildings.Add(building);
            }

            ServiceLocator.Current.Get<IStorage>().Save(saveKey, saveBuildings);
        }

        private void TryLoadBuildings()
        {
            ServiceLocator.Current.Get<IStorage>().Load(saveKey, (List<Building> saveBuildings) =>
            {
                foreach (Building saveBuilding in saveBuildings)
                {
                    BuildingData buildingData = ServiceLocator.Current.Get<IBuildingDataService>().GetBuildingDataByID(saveBuilding.DataId);

                    Vector3 spawnPos = new(saveBuilding.PositionX, saveBuilding.PositionY, 0);

                    GameObject newBuilding = Object.Instantiate(
                        buildingData.Prefab,
                        spawnPos,
                        Quaternion.identity
                    );

                    AddToBuildingMaps(saveBuilding, newBuilding, false);

                    GridService gridService = ServiceLocator.Current.Get<GridService>();

                    Vector2Int gridPos = gridService.WorldToGridPosition(spawnPos);
                    gridService.OccupyCells(gridPos, buildingData.Size, saveBuilding);
                }
            });
        }
    }
}
