using UnityEngine;

namespace InputManager
{
  public class InputSetter : MonoBehaviour
  {
    [SerializeField] InputController _inputController;
    [SerializeField] private FloatingJoystick _leftJoystick;
    [SerializeField] private PointerButton _castButton;

    private void OnEnable()
    {
      _leftJoystick.EventOnPointerDown += EventLeftJoystickPointerDown;
      _leftJoystick.EventOnPointerUp += EventLeftJoystickPointerUp;
      _castButton.EventPointerDown += OnButtonClicked;
    }

    private void OnDisable()
    {
      _leftJoystick.EventOnPointerDown -= EventLeftJoystickPointerDown;
      _leftJoystick.EventOnPointerUp -= EventLeftJoystickPointerUp;
      _castButton.EventPointerDown -= OnButtonClicked;
    }

    private void OnButtonClicked()
    {
      _inputController.OnCast();
    }

    private void Update()
    {
      _inputController.OnMove(_leftJoystick.Direction);
    }

    private void EventLeftJoystickPointerDown()
    {
      _inputController.OnMovePointerDown();
    }

    private void EventLeftJoystickPointerUp()
    {
      _inputController.OnMovePointerUp();
    }
  }
}