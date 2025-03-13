using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _invalidMaterial;

    public bool IsValid => _isValid;
    private bool _isValid;

    private BuildingData _currentBuildingData;
    private SpriteRenderer _renderer;
    private GridSystem _gridSystem;
    private InputHandler _inputHandler;

    public void Initialize(BuildingData data, GridSystem gridSystem, InputHandler inputHandler)
    {
        _renderer = GetComponent<SpriteRenderer>();

        _currentBuildingData = data;
        _gridSystem = gridSystem;

        _renderer.sprite = _currentBuildingData.BuildingGhostSprite;

        _inputHandler = inputHandler;

        _inputHandler.mouseMoved += UpdatePosition;

        UpdatePosition(_inputHandler.GetMousePosition());
    }

    private void OnDestroy()
    {
        _inputHandler.mouseMoved -= UpdatePosition;
    }

    private void UpdatePosition(Vector2 move)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(move);

        Vector2Int gridPos = _gridSystem.WorldToGridPosition(worldPos);

        Vector2 NewPosition = new Vector2(
            Mathf.RoundToInt(gridPos.x * _gridSystem.GridCellSize),
            Mathf.RoundToInt(gridPos.y * _gridSystem.GridCellSize)
            );

        if ((Vector2)transform.position != NewPosition)
        {
            transform.position = NewPosition;

            _isValid = _gridSystem.IsAreaFree(gridPos, _currentBuildingData);

            SetValidity();
        }        
    }

    public void SetValidity()
    {
        _renderer.material = _isValid ? _validMaterial : _invalidMaterial;
    }
}