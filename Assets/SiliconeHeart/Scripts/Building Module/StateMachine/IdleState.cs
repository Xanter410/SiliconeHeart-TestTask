using SiliconeHeart.Data;
using SiliconeHeart.UI;
using Utils.ServiceLocator;
using Utils.StateMachine;

namespace SiliconeHeart.Building
{
    public class IdleState : IState
    {
        public int ID { get; }

        private BuildingStateMachine _buildingStateMachine;

        public IdleState(int id, BuildingStateMachine buildingStateMachine)
        {
            ID = id;

            _buildingStateMachine = buildingStateMachine;
        }

        public void Enter()
        {
            ServiceLocator.Current.Get<UIGameplayService>().placeButtonClicked += OnPlaceButtonClicked;
            ServiceLocator.Current.Get<UIGameplayService>().deleteButtonClicked += OnDeleteButtonClicked;
        }
        public void Exit()
        {
            ServiceLocator.Current.Get<UIGameplayService>().placeButtonClicked -= OnPlaceButtonClicked;
            ServiceLocator.Current.Get<UIGameplayService>().deleteButtonClicked -= OnDeleteButtonClicked;
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
            _buildingStateMachine.currentBuildingData = null;

            _buildingStateMachine.TransitionTo(_buildingStateMachine.DeconstructState);
        }
    }
}
