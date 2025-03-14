using SiliconeHeart.Grid;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _invalidMaterial;

    public bool IsValid => _isValid;
    private bool _isValid;

    private BuildingData _currentBuildingData;
    private SpriteRenderer _renderer;

    public void Initialize(BuildingData data)
    {
        _renderer = GetComponent<SpriteRenderer>();

        _currentBuildingData = data;

        _renderer.sprite = _currentBuildingData.BuildingGhostSprite;

        InputHandler inputHandler = ServiceLocator.Current.Get<InputHandler>();

        inputHandler.mouseMoved += UpdatePosition;

        UpdatePosition(inputHandler.GetMousePosition());
    }

    private void OnDestroy()
    {
        InputHandler inputHandler = ServiceLocator.Current.Get<InputHandler>();

        inputHandler.mouseMoved -= UpdatePosition;
    }

    private void UpdatePosition(Vector2 move)
    {
        var gridService = ServiceLocator.Current.Get<GridService>();

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(move);

        Vector2Int gridPos = gridService.WorldToGridPosition(worldPos);

        Vector2 NewPosition = new Vector2(
            Mathf.RoundToInt(gridPos.x * gridService.GridCellSize),
            Mathf.RoundToInt(gridPos.y * gridService.GridCellSize)
            );

        if ((Vector2)transform.position != NewPosition)
        {
            transform.position = NewPosition;

            _isValid = gridService.IsAreaFree(gridPos, _currentBuildingData.Size);

            SetValidity();
        }        
    }

    public void SetValidity()
    {
        _renderer.material = _isValid ? _validMaterial : _invalidMaterial;
    }
}