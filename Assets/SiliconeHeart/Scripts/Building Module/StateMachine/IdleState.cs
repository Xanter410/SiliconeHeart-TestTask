using SiliconeHeart.Data;
using SiliconeHeart.UI;
using Utils.ServiceLocator;
using Utils.StateMachine;

namespace SiliconeHeart.Building
{
    public class IdleState : IState
    {
        public int ID { get; }

        private readonly BuildingStateMachine _buildingStateMachine;

        public IdleState(int id, BuildingStateMachine buildingStateMachine)
        {
            ID = id;

            _buildingStateMachine = buildingStateMachine;
        }

        public void Enter()
        {
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked += OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked += OnDeleteButtonClicked;
        }
        public void Exit()
        {
            ServiceLocator.Current.Get<IBuildingUICallbacks>().PlaceButtonClicked -= OnPlaceButtonClicked;
            ServiceLocator.Current.Get<IBuildingUICallbacks>().DeleteButtonClicked -= OnDeleteButtonClicked;
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
