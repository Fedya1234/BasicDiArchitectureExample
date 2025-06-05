using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputManager
{
  public class PointerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
  {
    public event Action EventPointerDown;
    public event Action EventPointerUp;
    public bool IsPressed { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
      IsPressed = true;
      EventPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      IsPressed = false;
      EventPointerUp?.Invoke();
    }
  }
}