using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> mouseMoved;
    public event Action leftClick;

    private InputActionAssetMap _inputAction;
    private bool _isPointerOverUI;

    public void Initialize()
    {
        _inputAction = new InputActionAssetMap();

        SetupInputActions();

        _inputAction.Enable();
    }

    private void Update()
    {
        _isPointerOverUI = IsPointerOverUI();
    }

    public Vector2 GetMousePosition()
    {
        return _inputAction.Mouse.Position.ReadValue<Vector2>();
    }

    private void SetupInputActions()
    {
        _inputAction.Mouse.Position.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());

        _inputAction.Mouse.LeftClick.started += _ => HandleLeftClick();
    }
    private void OnDestroy()
    {
        _inputAction.Mouse.Position.performed -= ctx => HandleMove(ctx.ReadValue<Vector2>());

        _inputAction.Mouse.LeftClick.started -= _ => HandleLeftClick();
    }

    private void HandleMove(Vector2 move)
    {
        if (_isPointerOverUI)
            return;

        mouseMoved?.Invoke(move);
    }

    private void HandleLeftClick()
    {
        if (_isPointerOverUI)
            return;

        leftClick?.Invoke();
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
