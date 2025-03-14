using System.Collections.Generic;
using SiliconeHeart.Grid;
using SiliconeHeart.Save;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private UIGameplaySystem _uiSystem;
    [SerializeField] private BuildingSystem _buildingSystem;

    [SerializeField] private List<BuildingData> _buildingsData;


    private void Awake()
    {
        ValidateDependencies();

        RegisterServices();

        InitializeSystems();
        CreateRelations();
    }

    private void ValidateDependencies()
    {
        if (_uiSystem == null ||
            _buildingSystem == null
            )
        {
            Debug.LogError("Bootstrap: Missing system dependencies!");
        }
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register<BuildingDataService>(new BuildingDataService(_buildingsData));

        ServiceLocator.Current.Register<IStorage>(new JsonToFileStorageService());

        ServiceLocator.Current.Register<GridService>(new GridService());

        ServiceLocator.Current.Register<InputHandler>(new InputHandler());
    }

    private void InitializeSystems()
    {
        _buildingSystem.Initialize();
        _uiSystem.Initialize();
    }

    private void CreateRelations()
    {
        _uiSystem.placeButtonClicked += _buildingSystem.TogglePlaceMode;
        _uiSystem.deleteButtonClicked += _buildingSystem.ToggleDeleteMode;
    }
}