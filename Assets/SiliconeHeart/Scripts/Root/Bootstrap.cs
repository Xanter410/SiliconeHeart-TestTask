using SiliconeHeart.Building;
using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using SiliconeHeart.Save;
using SiliconeHeart.UI;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Root
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private UIGameplayService _uiService;
        [SerializeField] private BuildingSystem _buildingSystem;

        private void Awake()
        {
            ValidateDependencies();
            RegisterServices();
            InitializeSystems();
        }

        private void ValidateDependencies()
        {
            if (_uiService == null ||
                _buildingSystem == null
                )
            {
                Debug.LogError("Bootstrap: Missing system dependencies!");
            }
        }

        private void RegisterServices()
        {
            ServiceLocator.Initialize();

            ServiceLocator.Current.Register<IBuildingDataService>(new BuildingDataService());

            ServiceLocator.Current.Register<IStorage>(new JsonToFileStorageService());

            ServiceLocator.Current.Register<GridService>(new GridService());

            ServiceLocator.Current.Register<IInput>(new InputHandler());

            _uiService.Initialize();
            ServiceLocator.Current.Register<IBuildingUICallbacks>(_uiService);
        }

        private void InitializeSystems()
        {
            _buildingSystem.Initialize();
        }
    }
}