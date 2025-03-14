using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Data
{
    public class BuildingDataService : IService
    {
        private Dictionary<string, BuildingData> _buildingsDataMap;
        private List<BuildingData> _buildingsData;
        public BuildingDataService()
        {
            _buildingsData = Resources.LoadAll<BuildingData>("BuildingData/").ToList();

            _buildingsDataMap = new Dictionary<string, BuildingData>();

            foreach (var buildingData in _buildingsData)
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