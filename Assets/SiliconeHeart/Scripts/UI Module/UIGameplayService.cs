using System;
using SiliconeHeart.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils.ServiceLocator;

namespace SiliconeHeart.UI
{
    public class UIGameplayService : MonoBehaviour, IBuildingUICallbacks
    {
        [Header("Building Selection")]
        [SerializeField] private RectTransform _BuildingButtonsContainer;
        [SerializeField] private BuildingButton _buildingButtonPrefab;

        [Header("Action Buttons")]
        [SerializeField] private Button _placeButton;
        [SerializeField] private Button _deleteButton;

        public event Action<BuildingData> PlaceButtonClicked;
        public event Action<BuildingData> SelectedBuildingChanged;
        public event Action DeleteButtonClicked;

        private BuildingButton _selectedBuildingButton;
        private BuildingData _selectedBuildingData;

        public void Initialize()
        {
            SetupBuildingButtons();
            SetupActionButtons();
        }

        private void SetupBuildingButtons()
        {
            IBuildingDataService buildingDataService = ServiceLocator.Current.Get<IBuildingDataService>();

            foreach (BuildingData buildingData in buildingDataService.GetAllBuildingsData())
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

            SelectedBuildingChanged?.Invoke(_selectedBuildingData);
        }

        private void SetupActionButtons()
        {
            _placeButton.onClick.AddListener(OnPlaceButtonClicked);

            _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        }

        private void OnPlaceButtonClicked()
        {
            if (_selectedBuildingData != null)
            {
                PlaceButtonClicked?.Invoke(_selectedBuildingData);
            }
        }

        private void OnDeleteButtonClicked()
        {
            DeselectBuilding();
            DeleteButtonClicked?.Invoke();
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