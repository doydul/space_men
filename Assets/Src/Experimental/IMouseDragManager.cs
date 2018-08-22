using UnityEngine;
using System;

public interface IMouseDragManager {

    event Action<Vector2> MouseDragged;
}
