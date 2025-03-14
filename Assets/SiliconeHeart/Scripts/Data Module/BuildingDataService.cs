using System.Collections.Generic;
using Utils.ServiceLocator;

public class BuildingDataService : IService
{
    private Dictionary<string, BuildingData> _buildingsDataMap;
    private List<BuildingData> _buildingsData;
    public BuildingDataService(List<BuildingData> buildingsData)
    {
        _buildingsData = buildingsData;

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