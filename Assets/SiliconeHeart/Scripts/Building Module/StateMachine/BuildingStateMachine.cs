using SiliconeHeart.Data;
using UnityEngine;
using Utils.StateMachine;

namespace SiliconeHeart.Building
{
    public class BuildingStateMachine : StateMachine
    {
        public readonly IState IdleState;
        public readonly IState ConstructState;
        public readonly IState DeconstructState;

        public BuildingData currentBuildingData;

        public BuildingStateMachine(
            BuildingContainer buidlingContainer,
            GameObject buildingGhostPrefab,
            GameObject tilemapBuildingGrid,
            DeletingMarker deletingMarker
            )
        {
            IdleState = new IdleState(1, this);

            ConstructState = new ConstructState(
                2,
                this,
                buidlingContainer,
                buildingGhostPrefab,
                tilemapBuildingGrid
                );

            DeconstructState = new DeconstructState(
                3, 
                this,
                buidlingContainer,
                deletingMarker
                );
        }

        public void Start()
        {
            Initialize(IdleState);
        }

        public void Update()
        {
            UpdateState();
        } 

        public void FixedUpdate()
        {
            FixedUpdateState();
        }
    }
}
