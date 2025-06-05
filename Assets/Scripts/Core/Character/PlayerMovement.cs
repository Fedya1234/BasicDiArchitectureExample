using System;
using InputManager;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Character
{
  public class PlayerMovement : MonoBehaviour
  {
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    [SerializeField] private InputController _inputController;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _rotationSpeed = 10f;
    
    private Vector3 _moveDirection;
    private Vector3 _prevPosition;
    private Camera _mainCamera;

    private void Awake()
    {
      _mainCamera = Camera.main;
      _prevPosition = transform.position;
    }

    private void Update()
    {
      DoMovement();
    }

    private void FixedUpdate()
    {
      MoveThePlayer(_moveDirection);
    }

    private Vector3 ToMainHeroDirection()
    {
      var cameraForward = _mainCamera.transform.forward;
      var cameraRight = _mainCamera.transform.right;

      cameraForward.y = 0f;
      cameraRight.y = 0f;

      return cameraForward.normalized * _inputController.Move.y + cameraRight.normalized * _inputController.Move.x;
    }

    private void MoveThePlayer(Vector3 desiredDirection)
    {
      Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
      movement = movement.normalized * (_agent.speed * Time.fixedDeltaTime);

      _agent.Move(movement);
    }

    private void DoMovement()
    {
      _moveDirection = ToMainHeroDirection();

      var speed = GetSpeedByFrame();
      var magnitude = speed.magnitude;

      AnimateCharacter(magnitude);

      if (magnitude < 0.01f)
        return;
      
      var direction = new Vector3(speed.x, 0f, speed.z);
      transform.forward = Vector3.RotateTowards(transform.forward, direction, _rotationSpeed * Time.deltaTime, 0f);
    }


    private void AnimateCharacter(float speedMagnitude)
    {
      _animator.SetFloat(SpeedHash, speedMagnitude / Time.deltaTime, 0.3f, Time.deltaTime);
    }

    private Vector3 GetSpeedByFrame()
    {
      var speed = transform.position - _prevPosition;
      _prevPosition = transform.position;
      return speed;
    }
  }
}