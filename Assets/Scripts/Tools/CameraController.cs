using UnityEngine;

namespace Tools
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 10f;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    private void FixedUpdate()
    {
      if (_target == null)
        return;

      var desiredPosition = _target.position + _offset;
      var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.fixedDeltaTime);
      transform.position = smoothedPosition;
    }
  }
}