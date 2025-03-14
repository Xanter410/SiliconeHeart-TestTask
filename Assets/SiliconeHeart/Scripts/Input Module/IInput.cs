using System;
using UnityEngine;
using Utils.ServiceLocator;

namespace SiliconeHeart.Input
{
    public interface IInput : IService
    {
        public event Action<Vector2> MouseMoved;
        public event Action LeftClick;
        public Vector2 GetMousePosition();
    }
}