using UnityEngine;

public class DeletingMarker : MonoBehaviour
{
    [SerializeField] private Material _deletedMaterial;

    private GridSystem _gridSystem;
    private InputHandler _inputHandler;

    private Building _currentBuilding;
    private SpriteRenderer _currentSpriteBuilding;
    private Material _baseMaterial;

    public void Initialize(GridSystem gridSystem, InputHandler inputHandler)
    {
        _gridSystem = gridSystem;

        _inputHandler = inputHandler;
    }

    private void OnDestroy()
    {
        _inputHandler.mouseMoved -= CheckPosition;
    }

    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            _inputHandler.mouseMoved += CheckPosition;
        }
        else
        {
            _inputHandler.mouseMoved -= CheckPosition;
        }
    }

    private void CheckPosition(Vector2 move)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(move);

        Vector2Int gridPos = _gridSystem.WorldToGridPosition(worldPos);

        Building findedBuilding = _gridSystem.GetBuildingAtGridPosition(gridPos);

        if (findedBuilding != null && findedBuilding != _currentBuilding)
        {
            MarkBuilding(findedBuilding);
        }

        if (findedBuilding == null)
        {
            UnmarkOldBuilding();
        }
    }

    private void MarkBuilding(Building findedBuilding)
    {
        UnmarkOldBuilding();

        _currentBuilding = findedBuilding;
        _currentSpriteBuilding = _currentBuilding.GetComponent<SpriteRenderer>();

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
