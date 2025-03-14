using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using SiliconeHeart.UI;
using UnityEngine;
using Utils.ServiceLocator;
using Utils.StateMachine;

namespace SiliconeHeart.Building
{
    public class ConstructState : IState
    {
        public int ID { get; }
        private readonly BuildingStateMachine _buildingStateMachine;
        private readonly BuildingContainer _buidlingContainer;

        private readonly GameObject _buildingGhostPrefab;
        private readonly GameObject _tilemapBuildingGrid;

        private GameObject _currentGhostGameObject;
        private BuildingGhost _currentGhost;

        public ConstructState(
            int id,
            BuildingStateMachine buildingStateMachine,
            BuildingContainer buidlingContainer,
            GameObject buildingGhostPrefab,
            GameObject tilemapBuildingGrid
            )
        {
            ID = id;
            _buildingStateMachine = buildingStateMachine;
            _buidlingContainer = buidlingContainer;

            _buildingGhostPrefab = buildingGhostPrefab;
            _tilemapBuildingGrid = tilemapBuildingGrid;
        }

        public void Enter()
        {
            ServiceLocator.Current.Get<IInput>().LeftClick += HandleAction;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked += OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked += OnDeleteButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().SelectedBuildingChanged += OnSelectedBuildingChanged;

            CreateGhost();

            _tilemapBuildingGrid.SetActive(true);
        }
        public void Exit()
        {
            ServiceLocator.Current.Get<IInput>().LeftClick -= HandleAction;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked -= OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked -= OnDeleteButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().SelectedBuildingChanged -= OnSelectedBuildingChanged;

            _buildingStateMachine.currentBuildingData = null;

            Object.Destroy(_currentGhostGameObject);

            _tilemapBuildingGrid.SetActive(false);
        }
        public void FixedUpdate()
        {

        }
        public void Update()
        {

        }
        private void OnPlaceButtonClicked(BuildingData _)
        {
            _buildingStateMachine.TransitionTo(_buildingStateMachine.IdleState);
        }

        private void OnDeleteButtonClicked()
        {
            _buildingStateMachine.TransitionTo(_buildingStateMachine.DeconstructState);
        }

        private void OnSelectedBuildingChanged(BuildingData data)
        {
            _buildingStateMachine.currentBuildingData = data;
            Object.Destroy(_currentGhostGameObject);
            CreateGhost();
        }

        private void HandleAction()
        {
            TryPlaceBuilding();
        }

        private void CreateGhost()
        {
            _currentGhostGameObject = Object.Instantiate(_buildingGhostPrefab);
            _currentGhost = _currentGhostGameObject.GetComponent<BuildingGhost>();
            _currentGhost.Initialize(_buildingStateMachine.currentBuildingData);
        }

        private void TryPlaceBuilding()
        {
            if (!_currentGhost.IsValid)
            {
                return;
            }

            GameObject newBuilding = Object.Instantiate(
                _buildingStateMachine.currentBuildingData.Prefab,
                _currentGhostGameObject.transform.position,
                Quaternion.identity
            );

            Building buildingModel = new(
                _buildingStateMachine.currentBuildingData.Id,
                newBuilding.transform.position.x,
                newBuilding.transform.position.y);

            _buidlingContainer.AddToBuildingMaps(buildingModel, newBuilding, true);

            UpdateGrid(newBuilding, buildingModel);
        }

        private void UpdateGrid(GameObject newBuilding, Building buildingModel)
        {
            GridService gridService = ServiceLocator.Current.Get<GridService>();

            Vector2Int gridPos = gridService.WorldToGridPosition(newBuilding.transform.position);

            gridService.OccupyCells(gridPos, _buildingStateMachine.currentBuildingData.Size, buildingModel);
        }
    }
}