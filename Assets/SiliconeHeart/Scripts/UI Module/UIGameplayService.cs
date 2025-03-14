using System;
using SiliconeHeart.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils.ServiceLocator;

namespace SiliconeHeart.UI
{
    public class UIGameplayService : MonoBehaviour, IService
    {
        [Header("Building Selection")]
        [SerializeField] private RectTransform _BuildingButtonsContainer;
        [SerializeField] private BuildingButton _buildingButtonPrefab;

        [Header("Action Buttons")]
        [SerializeField] private Button _placeButton;
        [SerializeField] private Button _deleteButton;

        public event Action<BuildingData> placeButtonClicked;
        public event Action<BuildingData> selectedBuildingChanged;
        public event Action deleteButtonClicked;

        private BuildingButton _selectedBuildingButton;
        private BuildingData _selectedBuildingData;

        public void Initialize()
        {
            SetupBuildingButtons();
            SetupActionButtons();
        }

        private void SetupBuildingButtons()
        {
            BuildingDataService buildingDataService = ServiceLocator.Current.Get<BuildingDataService>();

            foreach (var buildingData in buildingDataService.GetAllBuildingsData())
            {
                BuildingButton newButton = Instantiate(_buildingButtonPrefab, _BuildingButtonsContainer);
                newButton.Initialize(buildingData, OnBuildingSelected);
            }
        }
        private void OnBuildingSelected(BuildingData data, BuildingButton button)
        {
            if (_selectedBuildingButton != null)
            {
                _selectedBuildingButton.SetSelected(false);
            }

            _selectedBuildingButton = button;
            _selectedBuildingData = data;
            _selectedBuildingButton.SetSelected(true);

            selectedBuildingChanged?.Invoke(_selectedBuildingData);
        }

        private void SetupActionButtons()
        {
            _placeButton.onClick.AddListener(PlaceButtonClicked);

            _deleteButton.onClick.AddListener(DeleteButtonClicked);
        }

        private void PlaceButtonClicked()
        {
            if (_selectedBuildingData != null)
            {
                placeButtonClicked?.Invoke(_selectedBuildingData);
            }
        }

        private void DeleteButtonClicked()
        {
            DeselectBuilding();
            deleteButtonClicked?.Invoke();
        }

        private void DeselectBuilding()
        {
            if (_selectedBuildingButton != null)
            {
                _selectedBuildingButton.SetSelected(false);
                _selectedBuildingButton = null;
                _selectedBuildingData = null;
            }
        }
    }
}