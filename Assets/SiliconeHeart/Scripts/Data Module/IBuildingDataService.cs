using System.Collections.Generic;
using Utils.ServiceLocator;

namespace SiliconeHeart.Data
{
    public interface IBuildingDataService : IService
    {
        public BuildingData GetBuildingDataByID(string id);
        public List<BuildingData> GetAllBuildingsData();
    }
}