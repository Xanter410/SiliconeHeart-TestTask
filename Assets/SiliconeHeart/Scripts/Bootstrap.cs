using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private BuildingDataService _buildingDataService;
    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private SaveManager _saveManager;


    private void Awake()
    {
        ValidateDependencies();
        InitializeSystems();
        CreateRelations();
    }

    private void ValidateDependencies()
    {
        if (_uiManager == null ||
            _buildingManager == null ||
            _inputHandler == null ||
            _buildingDataService == null ||
            _gridSystem == null ||
            _saveManager == null
            )
        {
            Debug.LogError("Bootstrap: Missing system dependencies!");
        }
    }

    private void InitializeSystems()
    {
        //// 1. Загрузка данных
        var savedData = _saveManager.LoadData();


        _buildingDataService.Initialize();

        //// 2. Инициализация сетки с учётом сохранённых данных
        _gridSystem.Initialize();


        _inputHandler.Initialize();

        //// 3. Настройка системы строительства
        _buildingManager.Initialize(_gridSystem, _inputHandler, _buildingDataService, _saveManager);
        _buildingManager.SpawnSavedBuildings(savedData);

        // 4. Инициализация UI
        _uiManager.Initialize(_buildingDataService);
    }

    private void CreateRelations()
    {
        _uiManager.placeButtonClicked += _buildingManager.TogglePlaceMode;
        _uiManager.deleteButtonClicked += _buildingManager.ToggleDeleteMode;

        _inputHandler.leftClick += _buildingManager.HandleAction;
    }
}
