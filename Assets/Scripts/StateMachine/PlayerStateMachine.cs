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
	float _moveSpeed = 2f;
	float _runMultiplier = 6.0f;
	float _rotationFactorPerFrame = 5.0f;

	// gravity variables
	float _gravity = -9.8f;
	float _groudedGravity = -0.35f;

	// jumping variables
	float _initialJumpVelocity;
	float _maxJumpHeight = 2.0f;
	float _maxJumpTime = 0.75f;

	// state variable
	[SerializeField] public List<PlayerBaseState> _AbilityState;
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

	public Animator Animator { get { return _animator; } set { _animator = value; } }
	private bool _hasAnimator;
	public bool HasAnimator { get { return _hasAnimator; } }

	//! animation IDS
	private int _animIDSpeed;
	private int _animIDGrounded;
	private int _animIDJump;
	private int _animIDFreeFall;
	private int _animIDMotionSpeed;
	private int _animIDAbility;

	private float _animationBlend;
	public float AnimationBlend { get { return _animationBlend; } set { _animationBlend = value; } }
	public int AnimIDSpeed { get { return _animIDSpeed; } }
	public int AnimIDGrounded { get { return _animIDGrounded; } }
	public int AnimIDJump { get { return _animIDJump; } }
	public int AnimIDFreeFall { get { return _animIDFreeFall; } }
	public int AnimIDMotionSpeed { get { return _animIDMotionSpeed; } }
	public int AnimIDAbility { get { return _animIDAbility; } }


	public bool Ability { get { return _input.ability; } }

	public Ability ability1;


	public AbilityState StateAbility = AbilityState.ready;
	public float CooldownTimeAbility;

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
		AssignAnimationIDs();
	}
	void setupJumpVariables()
	{
		float timeToApex = _maxJumpTime / 2;
		_gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
		_initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
	}

	private void AssignAnimationIDs()
	{
		_animIDSpeed = Animator.StringToHash("Speed");
		_animIDGrounded = Animator.StringToHash("Grounded");
		_animIDJump = Animator.StringToHash("Jump");
		_animIDFreeFall = Animator.StringToHash("FreeFall");
		_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		_animIDAbility = Animator.StringToHash("Ability");
		//_animIDAbility1 = Animator.StringToHash("Ability2");

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
		_hasAnimator = TryGetComponent(out _animator);
		HandleRotation();
		_currentState.UpdateStates();

		//Debug.Log(_currentState.name)
		_controller.Move(_applieMovement * Time.deltaTime);


		switch (StateAbility)
		{
			case AbilityState.cooldown:
				if (CooldownTimeAbility > 0)
				{
					CooldownTimeAbility -= Time.deltaTime;
				}
				else
				{
					CooldownTimeAbility = 0;
					StateAbility = AbilityState.ready;
				}
				break;
			default:
				break;
		}
	}
}
public enum AbilityState
{
	ready,
	active,
	cooldown,
}