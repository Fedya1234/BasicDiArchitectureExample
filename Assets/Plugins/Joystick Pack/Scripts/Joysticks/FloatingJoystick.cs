using System;
//using EventsManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public event Action EventOnPointerDown;
    public event Action EventOnPointerUp;
    
    private bool _isFirstTime = true;
    private Vector2 _startPosition;
    
    // protected override void Start()
    // {
    //     base.Start();
    //     background.gameObject.SetActive(false);
    // }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _startPosition = background.anchoredPosition;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        //background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        EventOnPointerDown?.Invoke();

        if (_isFirstTime == false)
            return;
        
        _isFirstTime = false;
        //EventManager.Broadcast(new EventStartControl());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.anchoredPosition = _startPosition;
        //background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        EventOnPointerUp?.Invoke();
    }
}