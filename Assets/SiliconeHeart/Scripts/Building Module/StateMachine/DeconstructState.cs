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

        private BuildingStateMachine _buildingStateMachine;
        private BuildingContainer _buidlingContainer;

        private DeletingMarker _deletingMarker;

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
            ServiceLocator.Current.Get<InputHandler>().leftClick += HandleAction;
            ServiceLocator.Current.Get<UIGameplayService>().placeButtonClicked += OnPlaceButtonClicked;
            ServiceLocator.Current.Get<UIGameplayService>().deleteButtonClicked += OnDeleteButtonClicked;

            _deletingMarker.SetEnabled(true);
        }
        public void Exit()
        {
            ServiceLocator.Current.Get<InputHandler>().leftClick -= HandleAction;
            ServiceLocator.Current.Get<UIGameplayService>().placeButtonClicked -= OnPlaceButtonClicked;
            ServiceLocator.Current.Get<UIGameplayService>().deleteButtonClicked -= OnDeleteButtonClicked;
            
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
            InputHandler inputHandler = ServiceLocator.Current.Get<InputHandler>();

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputHandler.GetMousePosition());

            var gridService = ServiceLocator.Current.Get<GridService>();

            Vector2Int gridPositionUnderMouse = gridService.WorldToGridPosition(mouseWorldPos);

            if (!gridService.IsAreaFree(gridPositionUnderMouse))
            {
                gridService.TryGetObjectAtGridPosition(gridPositionUnderMouse, out object building);

                GameObject findedBuilding = _buidlingContainer.GetGameObjectByBuilding((Building)building);

                _buidlingContainer.RevomeFromBuidingMaps((Building)building);
                
                Object.Destroy(findedBuilding);
            }
        }
    }
}
