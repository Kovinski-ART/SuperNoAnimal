using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
	#region Variables
	[SerializeField]
	private InputManager m_inputManager = default;
	private Vector3 m_movementDir;
	private float m_inputAmount;

	public float JumpHeight = 2f;
	#endregion

	#region BuiltIn Methods
	private void FixedUpdate()
	{
		UpdateMovementInput();
		UpdatePhysics();


	}
	#endregion

	#region Custom Methods
	private void UpdateMovementInput()
	{
		m_movementDir = Vector3.zero;
		Vector3 forward = m_inputManager.Forward * transform.forward;
		Vector3 sideway = m_inputManager.Sideway * transform.right;
		Vector3 combinedInput = (forward + sideway).normalized;
		m_movementDir = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
		float inputMagnitude = Mathf.Abs(_input.move.x) +
			Mathf.Abs(_input.move.y);
		m_inputAmount = Mathf.Clamp01(inputMagnitude);
	}
	#endregion

	[SerializeField]
	private Rigidbody m_rb = default;
	[SerializeField]
	private CapsuleCollider m_collider = default;
	[SerializeField]
	private float m_offsetFloorY = 0.4f;
	[SerializeField]
	private float m_movementSpeed = 3f;

	private Vector3 m_raycastFloorPos;
	private Vector3 m_combinedRaycast;
	private Vector3 m_gravity;
	private Vector3 m_floorMovement;
	private float m_groundRayLenght;

	private GameObject _mainCamera;

	private float _rotationVelocity;
	public StarterAssetsInputs _input;

	private void Awake()
	{
		// get a reference to our main camera
		if (_mainCamera == null)
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

	private float _targetRotation = 0.0f;
	public float RotationSmoothTime = 0.12f;
	private float _jumpTimeoutDelta;
	private void UpdatePhysics()
	{
		if (_input.move != Vector2.zero)
		{
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
									  _mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
				RotationSmoothTime);

			// rotate to face input direction relative to camera position
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}

		m_groundRayLenght = (m_collider.height * 0.5f) + m_offsetFloorY;

		if (FloorRaycasts(0, 0, m_groundRayLenght).transform == null)
		{
			m_gravity += (Vector3.up * Physics.gravity.y * Time.fixedDeltaTime);
		}

		//m_rb.velocity = (m_movementDir * m_movementSpeed * m_inputAmount) + m_gravity;
		m_rb.velocity = (m_movementDir * m_movementSpeed * m_inputAmount) + m_gravity;

		m_floorMovement = new Vector3(m_rb.position.x, FindFloor().y, m_rb.position.z);

		if (FloorRaycasts(0, 0, m_groundRayLenght).transform != null && m_floorMovement != m_rb.position)
		{
			m_rb.MovePosition(m_floorMovement);
			m_gravity.y = 0;
		}


		// Jump
		if (_input.jump && FloorRaycasts(0, 0, m_groundRayLenght).transform != null)
		{
			//_input.jump = false;
			// the square root of H * -2 * G = how much velocity needed to reach desired height
			//m_gravity.y = 1;
			//m_rb.MovePosition(new Vector3(m_rb.position.x, Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y * Time.fixedDeltaTime), m_rb.position.z));
			m_rb.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse); //+= new Vector3(0, JumpHeight, 0);
		}

	}
	private Vector3 FindFloor()
	{
		float raycastWidth = 0.25f;
		int floorAverage = 1;
		m_combinedRaycast = FloorRaycasts(0, 0, m_groundRayLenght).point;
		floorAverage += (GetFloorAverage(raycastWidth, 0) +
			GetFloorAverage(-raycastWidth, 0) + GetFloorAverage(0, raycastWidth) + GetFloorAverage(0, -raycastWidth));
		return m_combinedRaycast / floorAverage;
	}
	private RaycastHit FloorRaycasts(float t_offsetx, float t_offsetz, float t_raycastLength)
	{
		RaycastHit hit;
		m_raycastFloorPos = transform.TransformPoint(0 + t_offsetx, m_collider.center.y, 0 + t_offsetz);
		Debug.DrawRay(m_raycastFloorPos, Vector3.down * m_groundRayLenght, Color.magenta);
		Physics.Raycast(m_raycastFloorPos, -Vector3.up, out hit, t_raycastLength);
		return hit;
	}
	private int GetFloorAverage(float t_offsetx, float t_offsetz)
	{
		if (FloorRaycasts(t_offsetx, t_offsetz, m_groundRayLenght).transform != null)
		{
			m_combinedRaycast += FloorRaycasts(t_offsetx, t_offsetz, m_groundRayLenght).point;
			return 1;
		}
		else
		{
			return 0;
		}
	}
}
