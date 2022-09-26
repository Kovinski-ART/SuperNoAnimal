using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	StarterAssetsInputs _input;
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
	float _groudedGravity = -.05f;

	// jumping variables
	float _initialJumpVelocity;
	float _maxJumpHeight = 2.0f;
	float _maxJumpTime = 0.75f;




	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_controller.detectCollisions = true;

		_input = GetComponent<StarterAssetsInputs>();

		setupJumpVariables();
	}
	void setupJumpVariables()
	{
		float timeToApex = _maxJumpTime / 2;
		_gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
		_initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
	}
	void InputMove()
	{
		_isMovementPresset = _input.move.x != 0 || _input.move.y != 0;
		Vector2 inputnormals = _input.move.normalized;
		if (!_input.sprint)
		{
			_currentMovement.x = inputnormals.x * _moveSpeed;
			_currentMovement.z = inputnormals.y * _moveSpeed;
		}
		else
		{
			_currentMovement.x = inputnormals.x * _runMultiplier;
			_currentMovement.z = inputnormals.y * _runMultiplier;
		}

	}

	private void Update()
	{
		InputMove();
		HandleRotation();

		//_controller.SimpleMove(currentMovement.normalized * moveSpeed);
		_applieMovement.x = _currentMovement.x;
		_applieMovement.z = _currentMovement.z;


		_controller.Move(_applieMovement * Time.deltaTime);


		HandleGravity();
		HandleJamp();
	}

	void HandleJamp()
	{
		if (!_isJumping && _controller.isGrounded && _input.jump)
		{
			_isJumping = true;

			_currentMovement.y = _initialJumpVelocity;
			_applieMovement.y = _initialJumpVelocity;
		}
		else if (_isJumping && _controller.isGrounded && !_input.jump)
		{
			_isJumping = false;
		}
	}

	void HandleRotation()
	{
		Vector3 positionToLookAt;

		positionToLookAt.x = _input.move.x;
		positionToLookAt.y = 0;
		positionToLookAt.z = _input.move.y;
		Quaternion currentRotation = transform.rotation;

		if (_isMovementPresset)
		{
			Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
			transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
		}
	}
	void HandleGravity()
	{
		bool isFalling = _currentMovement.y <= 0.0f || !_input.jump;
		float fallMultiplier = 2.0f;

		if (_controller.isGrounded)
		{
			_currentMovement.y = _groudedGravity;
			_applieMovement.y = _groudedGravity;
		}
		else if (isFalling)
		{
			float previsouYVelocity = _currentMovement.y;
			_currentMovement.y = _currentMovement.y + (_gravity * fallMultiplier * Time.deltaTime);
			_applieMovement.y = Mathf.Max((previsouYVelocity + _currentMovement.y) * .5f, -20.0f);
		}
		else
		{
			float previsouYVelocity = _currentMovement.y;
			_currentMovement.y = _currentMovement.y + (_gravity * Time.deltaTime);
			_applieMovement.y = (previsouYVelocity + _currentMovement.y) * .5f;
		}
	}
}
