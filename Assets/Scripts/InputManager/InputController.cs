using System;
using UnityEngine;

namespace InputManager
{
  [CreateAssetMenu(menuName = "Input/InputController")]
  public class InputController : ScriptableObject
  {
    public event Action EventMovePointerDown;
    public event Action EventMovePointerUp;
    public event Action EventCast;

    private Vector2 _move;

    public Vector2 Move => _move;

    public void OnMove(Vector2 input)
    {
      _move = input;
    }

    public void OnCast()
    {
      EventCast?.Invoke();
    }

    public void OnMovePointerDown()
    {
      EventMovePointerDown?.Invoke();
    }

    public void OnMovePointerUp()
    {
      _move = Vector2.zero;
      EventMovePointerUp?.Invoke();
    }
  }
}