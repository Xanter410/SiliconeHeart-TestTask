using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Building
{
    public class DeletingMarker : MonoBehaviour
    {
        [SerializeField] private Material _deletedMaterial;

        private BuildingContainer _buildingContainer;

        private Building _currentBuilding;
        private SpriteRenderer _currentSpriteBuilding;
        private Material _baseMaterial;

        public void Initialize(BuildingContainer buildingContainer)
        {
            _buildingContainer = buildingContainer;
        }

        private void OnDestroy()
        {
            ServiceLocator.Current.Get<InputHandler>().mouseMoved -= CheckPosition;
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                ServiceLocator.Current.Get<InputHandler>().mouseMoved += CheckPosition;
            }
            else
            {
                ServiceLocator.Current.Get<InputHandler>().mouseMoved -= CheckPosition;
            }
        }

        private void CheckPosition(Vector2 move)
        {
            var gridSevice = ServiceLocator.Current.Get<GridService>();

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(move);

            Vector2Int gridPos = gridSevice.WorldToGridPosition(worldPos);

            if (gridSevice.TryGetObjectAtGridPosition(gridPos, out object findedBuilding))
            {
                if ((Building)findedBuilding != _currentBuilding)
                {
                    MarkBuilding((Building)findedBuilding);
                }
            }
            else
            {
                UnmarkOldBuilding();
            }
        }

        private void MarkBuilding(Building findedBuilding)
        {
            UnmarkOldBuilding();

            _currentBuilding = findedBuilding;

            var gameObject = _buildingContainer.GetGameObjectByBuilding(_currentBuilding);

            _currentSpriteBuilding = gameObject.GetComponent<SpriteRenderer>();

            _baseMaterial = _currentSpriteBuilding.material;
            _currentSpriteBuilding.material = _deletedMaterial;
        }

        private void UnmarkOldBuilding()
        {
            if (_currentSpriteBuilding != null)
            {
                _currentSpriteBuilding.material = _baseMaterial;

                _currentBuilding = null;
                _currentSpriteBuilding = null;
                _baseMaterial = null;
            }
        }
    }
}