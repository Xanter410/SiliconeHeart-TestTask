using SiliconeHeart.Data;
using System;
using Utils.ServiceLocator;

namespace SiliconeHeart.UI
{
    public interface IBuildingUICallbacks : IService
    {
        public event Action<BuildingData> PlaceButtonClicked;
        public event Action<BuildingData> SelectedBuildingChanged;
        public event Action DeleteButtonClicked;
    }
}