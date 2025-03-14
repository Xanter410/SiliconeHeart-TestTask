using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils.ServiceLocator;

namespace SiliconeHeart.Input
{
    public class InputHandler : IService
    {
        public event Action<Vector2> mouseMoved;
        public event Action leftClick;

        private InputActionAssetMap _inputAction;

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
                return;

            mouseMoved?.Invoke(move);
        }

        private void HandleLeftClick()
        {
            if (IsPointerOverUI())
                return;

            leftClick?.Invoke();
        }

        private bool IsPointerOverUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}