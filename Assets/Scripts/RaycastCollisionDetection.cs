using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public enum StatePlayerController : int
{
	Stand = 0,
	Walk = 1,
	Run = 2,
	Fall = 3,
	Fell = 4,
	Jump = 5
}

public delegate void EventChangeState(StatePlayerController SPC);

[RequireComponent(typeof(CharacterController))]
public class RaycastCollisionDetection : MonoBehaviour
{

	private CharacterController controller;
	private Transform _Camera;

	[SerializeField]
	private float SmoothCamRot = 1f;
	[SerializeField]
	private float Angle = 80f;
	private float AngleRot = 0;

	[SerializeField]
	private float forceJump = 10f;
	[SerializeField]
	private float SpendingionJump = 1f;

	[SerializeField]
	private float Speed = 10f;
	[SerializeField]
	private float RunSpeed = 20f;

	private float SelectedSpeed;

	[SerializeField]
	private float g = 9.8f;

	//private bool RealisticGravity = false;
	//private float cof = 1f;

	private Vector3 move;

	[SerializeField]
	private float MaxStamina = 10f;
	[SerializeField]
	private float MinStartRunStamina = 1f;
	[SerializeField]
	private float SpeedSpendingionStamina = 1f;
	[SerializeField]
	private float SpeedRegenerationStamina = 0.35f;
	private float Stamina;

	public event EventChangeState EventCS;

	private float stamina
	{
		set
		{
			Stamina = Mathf.Clamp(value, 0, MaxStamina);

			if (Stamina == 0)
			{
				Run = false;
			}

			/*
			BarFilled.fillAmount = Stamina/MaxStamina;

			if(!Run)
			{
					if(Stamina <= MinStartRunStamina)
					{
							BarFilled.color = ColorBarRegen;
					}
					else
					{
							BarFilled.color = ColorBarNormal;
					}
			}
			*/
		}
		get
		{
			return Stamina;
		}
	}

	private bool Run = false;


	//[SerializeField]
	//private Color ColorBarNormal = Color.green;
	//[SerializeField]
	//private Color ColorBarRegen = Color.grey;

	//[SerializeField]
	//private Image BarFilled;

	private StatePlayerController LastState = StatePlayerController.Fall;

	public Vector3 GetSpeed { get { return move; } }

	private bool LockRotate = true;

	void Start()
	{
		Stamina = MaxStamina;

		move = new Vector3(0, g, 0);

		controller = GetComponent<CharacterController>();

		_Camera = transform.GetChild(0);

		SelectedSpeed = Speed;
	}


	void Update()
	{
		StatePlayerController SPC = StatePlayerController.Walk;

		if (Run)
		{
			SPC = StatePlayerController.Run;

			stamina -= Time.deltaTime * SpeedSpendingionStamina;
		}
		else if (Stamina < MaxStamina)
		{
			stamina += Time.deltaTime * SpeedRegenerationStamina;
		}

		if (controller.isGrounded)
		{
			if (InputGetKey(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D))
			{
				if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > MinStartRunStamina)
				{
					Run = true;

					SelectedSpeed = RunSpeed;
				}
				else if (Input.GetKeyUp(KeyCode.LeftShift))
				{
					Run = false;

					SelectedSpeed = Speed;
				}

				float Hor = Input.GetAxis("Horizontal");
				float Ver = Input.GetAxis("Vertical");

				Vector3 move = transform.TransformVector(new Vector3(Hor, 0, Ver).normalized);
				move *= SelectedSpeed;

				this.move.x = move.x;
				this.move.z = move.z;
			}
			else
			{
				Run = false;

				SelectedSpeed = Speed;

				move = new Vector3(0, g, 0);

				SPC = StatePlayerController.Stand;
			}

			if (LastState == StatePlayerController.Fall || LastState == StatePlayerController.Jump)
			{
				LastState = StatePlayerController.Fell;
			}

			if (Input.GetKeyDown(KeyCode.Space) && SpendingionStamina(SpendingionJump))
			{
				move.y = forceJump;

				SPC = StatePlayerController.Jump;
			}
			else
			{
				move.y = g;
			}
		}
		else
		{
			Run = false;

			SelectedSpeed = Speed;

			move.y += Time.deltaTime * g;

			if (move.y >= 0)
			{
				SPC = StatePlayerController.Jump;
			}
			else
			{
				SPC = StatePlayerController.Fall;
			}
		}

		if (LockRotate)
		{
			float Y = -Input.GetAxis("Mouse Y");
			float X = Input.GetAxis("Mouse X");

			if (X != 0)
			{
				transform.eulerAngles += new Vector3(0, X * Time.deltaTime * SmoothCamRot, 0);
			}

			if (Y != 0)
			{
				AngleRot = Mathf.Clamp(AngleRot + Y * Time.deltaTime * SmoothCamRot, -Angle, Angle);
				_Camera.localEulerAngles = new Vector3(AngleRot, 0, 0);
			}
		}

		if (SPC != LastState)
		{
			LastState = SPC;

			EventCS(LastState);
		}

		controller.Move(move * Time.deltaTime);
	}



	private bool InputGetKey(params KeyCode[] KC)
	{
		for (int i = 0; i < KC.Length; i++)
		{
			if (Input.GetKey(KC[i]))
			{
				return true;
			}
		}

		return false;
	}


	private bool SpendingionStamina(float GetStam)
	{
		if ((Stamina >= MinStartRunStamina || Run) && (Stamina - GetStam) >= 0)
		{
			stamina -= GetStam;

			return true;
		}

		return false;
	}

	public void LockRotateCam(bool LockRotate)
	{
		this.LockRotate = LockRotate;
	}
}
