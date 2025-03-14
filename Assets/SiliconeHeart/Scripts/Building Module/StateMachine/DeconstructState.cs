using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using SiliconeHeart.UI;
using UnityEngine;
using Utils.ServiceLocator;
using Utils.StateMachine;

namespace SiliconeHeart.Building
{
    public class DeconstructState : IState
    {
        public int ID { get; }

        private readonly BuildingStateMachine _buildingStateMachine;
        private readonly BuildingContainer _buidlingContainer;

        private readonly DeletingMarker _deletingMarker;

        public DeconstructState(
            int id, 
            BuildingStateMachine buildingStateMachine,
            BuildingContainer buidlingContainer,
            DeletingMarker deletingMarker)
        {
            ID = id;
            _buildingStateMachine = buildingStateMachine;
            _buidlingContainer = buidlingContainer;
            _deletingMarker = deletingMarker;

            _deletingMarker.Initialize(_buidlingContainer);
        }

        public void Enter()
        {
            ServiceLocator.Current.Get<IInput>().LeftClick += HandleAction;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked += OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked += OnDeleteButtonClicked;

            _deletingMarker.SetEnabled(true);
        }
        public void Exit()
        {
            ServiceLocator.Current.Get<IInput>().LeftClick -= HandleAction;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked -= OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked -= OnDeleteButtonClicked;
            
            _deletingMarker.SetEnabled(false);
        }
        public void FixedUpdate()
        {

        }
        public void Update()
        {

        }
        private void OnPlaceButtonClicked(BuildingData data)
        {
            _buildingStateMachine.currentBuildingData = data;

            _buildingStateMachine.TransitionTo(_buildingStateMachine.ConstructState);
        }

        private void OnDeleteButtonClicked()
        {
            _buildingStateMachine.TransitionTo(_buildingStateMachine.IdleState);
        }

        private void HandleAction()
        {
            TryDeleteBuilding();
        }

        private void TryDeleteBuilding()
        {
            IInput inputHandler = ServiceLocator.Current.Get<IInput>();

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputHandler.GetMousePosition());

            GridService gridService = ServiceLocator.Current.Get<GridService>();

            Vector2Int gridPositionUnderMouse = gridService.WorldToGridPosition(mouseWorldPos);

            if (!gridService.IsAreaFree(gridPositionUnderMouse))
            {
                _ = gridService.TryGetObjectAtGridPosition(gridPositionUnderMouse, out object building);

                GameObject findedBuilding = _buidlingContainer.GetGameObjectByBuilding((Building)building);

                _buidlingContainer.RevomeFromBuidingMaps((Building)building);
                
                Object.Destroy(findedBuilding);
            }
        }
    }
}
