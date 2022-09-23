﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacter : RaycastCollisionDetection
{

	public float Speed = 5f;
	public float JumpHeight = 2f;
	public float GroundDistance = 0.2f;
	public float DashDistance = 5f;
	public LayerMask Ground;

	private Rigidbody _body;
	private Vector3 _inputs = Vector3.zero;
	private bool _isGrounded = true;
	private Transform _groundChecker;
	protected Transform m_Transform;
	private Rigidbody m_Rigidbody;
	public float m_Mass;

	protected bool m_Grounded = true;
	private void Awake()
	{
		m_Transform = transform;
	}
	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.mass = m_Mass;
		m_Rigidbody.isKinematic = true;
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

		_body = GetComponent<Rigidbody>();
		_groundChecker = transform.GetChild(0);
	}

	void Update()
	{
		_isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


		_inputs = Vector3.zero;
		_inputs.x = Input.GetAxis("Horizontal");
		_inputs.z = Input.GetAxis("Vertical");
		if (_inputs != Vector3.zero)
			transform.forward = _inputs;

		if (Input.GetButtonDown("Jump") && _isGrounded)
		{
			_body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
		}
		if (Input.GetButtonDown("Dash"))
		{
			Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
			_body.AddForce(dashVelocity, ForceMode.VelocityChange);
		}
	}


	void FixedUpdate()
	{
		_body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
	}
}
