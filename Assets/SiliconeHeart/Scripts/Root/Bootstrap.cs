using System.Collections.Generic;
using SiliconeHeart.Building;
using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using SiliconeHeart.Save;
using SiliconeHeart.UI;
using UnityEngine;
using Utils.ServiceLocator;

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

        ServiceLocator.Current.Register<BuildingDataService>(new BuildingDataService());

        ServiceLocator.Current.Register<IStorage>(new JsonToFileStorageService());

        ServiceLocator.Current.Register<GridService>(new GridService());

        ServiceLocator.Current.Register<InputHandler>(new InputHandler());

        _uiService.Initialize();
        ServiceLocator.Current.Register<UIGameplayService>(_uiService);
    }

    private void InitializeSystems()
    {
        _buildingSystem.Initialize();
    }
}