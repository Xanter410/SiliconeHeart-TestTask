using UnityEngine;

namespace SiliconeHeart.Building
{
    public class BuildingSystem : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private GameObject _buildingGhostPrefab;
        [SerializeField] private GameObject _tilemapBuildingGrid;

        [SerializeField] private DeletingMarker _deletingMarker;

        private BuildingContainer _buidlingContainer;
        private BuildingStateMachine _buildingStateMachine;

        public void Initialize()
        {
            _buidlingContainer = new BuildingContainer();

            _buildingStateMachine = new BuildingStateMachine(
                _buidlingContainer,
                _buildingGhostPrefab,
                _tilemapBuildingGrid,
                _deletingMarker
                );
        }

        private void Start()
        {
            _buildingStateMachine.Start();
        }

        private void Update()
        {
            _buildingStateMachine.Update();
        }

        private void FixedUpdate()
        {
            _buildingStateMachine.FixedUpdate();
        }
    }
}