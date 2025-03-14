using SiliconeHeart.Data;
using SiliconeHeart.Grid;
using SiliconeHeart.Input;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Building
{
    public class BuildingGhost : MonoBehaviour
    {
        [SerializeField] private Material _validMaterial;
        [SerializeField] private Material _invalidMaterial;
        public bool IsValid { get; private set; }

        private BuildingData _currentBuildingData;
        private SpriteRenderer _renderer;

        public void Initialize(BuildingData data)
        {
            _renderer = GetComponent<SpriteRenderer>();

            _currentBuildingData = data;

            _renderer.sprite = _currentBuildingData.BuildingGhostSprite;

            IInput inputHandler = ServiceLocator.Current.Get<IInput>();

            inputHandler.MouseMoved += UpdatePosition;

            UpdatePosition(inputHandler.GetMousePosition());
        }

        private void OnDestroy()
        {
            IInput inputHandler = ServiceLocator.Current.Get<IInput>();

            inputHandler.MouseMoved -= UpdatePosition;
        }

        private void UpdatePosition(Vector2 move)
        {
            GridService gridService = ServiceLocator.Current.Get<GridService>();

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(move);

            Vector2Int gridPos = gridService.WorldToGridPosition(worldPos);

            Vector2 NewPosition = new(
                Mathf.RoundToInt(gridPos.x * gridService.GridCellSize),
                Mathf.RoundToInt(gridPos.y * gridService.GridCellSize)
                );

            if ((Vector2)transform.position != NewPosition)
            {
                transform.position = NewPosition;

                IsValid = gridService.IsAreaFree(gridPos, _currentBuildingData.Size);

                SetValidity();
            }
        }

        public void SetValidity()
        {
            _renderer.material = IsValid ? _validMaterial : _invalidMaterial;
        }
    }
}