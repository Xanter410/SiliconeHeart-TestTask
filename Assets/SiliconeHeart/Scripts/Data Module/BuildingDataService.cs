using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SiliconeHeart.Data
{
    public class BuildingDataService : IBuildingDataService
    {
        private readonly Dictionary<string, BuildingData> _buildingsDataMap;
        private readonly List<BuildingData> _buildingsData;
        public BuildingDataService() 
        {
            _buildingsData = Resources.LoadAll<BuildingData>("BuildingData/").ToList();

            _buildingsDataMap = new Dictionary<string, BuildingData>();

            foreach (BuildingData buildingData in _buildingsData)
            {
                _buildingsDataMap[buildingData.Id] = buildingData;
            }
        }

        public BuildingData GetBuildingDataByID(string id)
        {
            if (_buildingsDataMap.ContainsKey(id))
            {
                return _buildingsDataMap[id];
            }

            return null;
        }

        public List<BuildingData> GetAllBuildingsData()
        {
            return _buildingsData;
        }
    }
}