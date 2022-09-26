using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
	StarterAssetsInputs _input;
	Animator _animator;
	CharacterController _controller;

	Vector3 _currentMovement;
	Vector3 _applieMovement;
	bool _isMovementPresset;
	bool _isJumping = false;

	// constants
	float _moveSpeed = 5f;
	float _runMultiplier = 8.0f;
	float _rotationFactorPerFrame = 15.0f;

	// gravity variables
	float _gravity = -9.8f;
	float _groudedGravity = -0.35f;

	// jumping variables
	float _initialJumpVelocity;
	float _maxJumpHeight = 2.0f;
	float _maxJumpTime = 0.75f;

	// state variable
	PlayerBaseState _currentState;
	PlayerStateFactory _states;

	// GETters and SETters
	public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
	public CharacterController CharacterController { get { return _controller; } }
	public bool IsJumpingPressed { get { return _input.jump; } }
	public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
	public bool IsMoventPressed { get { return _input.move.x != 0 || _input.move.y != 0; } }
	public bool IsRunPressed { get { return _input.sprint; } }

	public float Gravity { get { return _gravity; } }
	public float GroudedGravity { get { return _groudedGravity; } }
	public float InitialJumpVelocity { get { return _initialJumpVelocity; } set { _initialJumpVelocity = value; } }
	public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
	public float ApplieMovementY { get { return _applieMovement.y; } set { _applieMovement.y = value; } }
	public float ApplieMovementX { get { return _applieMovement.x; } set { _applieMovement.x = value; } }
	public float ApplieMovementZ { get { return _applieMovement.z; } set { _applieMovement.z = value; } }

	public Vector2 InputMovement { get { return _input.move.normalized; } }
	public float RunMultiplier { get { return _runMultiplier; } }
	public float MoveSpeed { get { return _moveSpeed; } }



	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_controller.detectCollisions = true;

		_animator = GetComponent<Animator>();
		_input = GetComponent<StarterAssetsInputs>();

		// setup state
		_states = new PlayerStateFactory(this);
		_currentState = _states.Grounded();
		_currentState.EnterState();

		setupJumpVariables();
	}
	void setupJumpVariables()
	{
		float timeToApex = _maxJumpTime / 2;
		_gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
		_initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
	}
	void HandleRotation()
	{
		Vector3 positionToLookAt;

		positionToLookAt.x = _input.move.x;
		positionToLookAt.y = 0;
		positionToLookAt.z = _input.move.y;
		Quaternion currentRotation = transform.rotation;

		if (IsMoventPressed)
		{
			Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
			transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
		}
	}

	void Update()
	{
		HandleRotation();
		_currentState.UpdateStates();
		_controller.Move(_applieMovement * Time.deltaTime);
	}
}
