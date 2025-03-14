using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SiliconeHeart.Input
{
    public class InputHandler : IInput
    {
        public event Action<Vector2> MouseMoved;
        public event Action LeftClick;

        private readonly InputActionAssetMap _inputAction; 

        public InputHandler()
        {
            _inputAction = new InputActionAssetMap();

            SetupInputActions();

            _inputAction.Enable();
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

        private void HandleMove(Vector2 move)
        {
            if (IsPointerOverUI())
            {
                return;
            }

            MouseMoved?.Invoke(move);
        }

        private void HandleLeftClick()
        {
            if (IsPointerOverUI())
            {
                return;
            }

            LeftClick?.Invoke();
        }

        private bool IsPointerOverUI()
        {
            PointerEventData eventData = new(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}