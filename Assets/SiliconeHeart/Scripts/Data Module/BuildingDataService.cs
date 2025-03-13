using System.Collections.Generic;
using UnityEngine;

public class BuildingDataService : MonoBehaviour
{
    [SerializeField] private List<BuildingData> _buildingsData;

    private Dictionary<string, BuildingData> _buildingsDataMap;

    public void Initialize()
    {
        _buildingsDataMap = new Dictionary<string, BuildingData>();

        foreach (var buildingData in _buildingsData)
        {
            _buildingsDataMap[buildingData.ID] = buildingData;
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