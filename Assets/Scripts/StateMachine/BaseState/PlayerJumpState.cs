using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootGravityState
{
	public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { IsRootState = true; }

	public override void EnterState()
	{
		InitializeSubState();
		Debug.Log("Enter State Jump");
		HandleJamp();
	}
	public override void UpdateState()
	{
		HandleGravity();
		CheckSwithStates();
	}
	public override void ExitState()
	{
		Debug.Log("Exit State Jump");
	}
	public override void InitializeSubState()
	{
		if (!Ctx.IsMoventPressed && !Ctx.IsRunPressed)
		{
			SetSubState(Factory.Idle());
		}
		else if (Ctx.IsMoventPressed && !Ctx.IsRunPressed)
		{
			SetSubState(Factory.Walk());
		}
		else
		{
			SetSubState(Factory.Run());
		}
	}
	public override void CheckSwithStates()
	{
		if (Ctx.CharacterController.isGrounded)
		{
			SwitchState(Factory.Grounded());
		}
	}

	void HandleJamp()
	{
		Ctx.IsJumping = true;

		Ctx.CurrentMovementY = Ctx.InitialJumpVelocity;
		Ctx.ApplieMovementY = Ctx.InitialJumpVelocity;
	}

	public void HandleGravity()
	{
		bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpingPressed;
		float fallMultiplier = 2.0f;

		if (isFalling)
		{
			float previsouYVelocity = Ctx.CurrentMovementY;
			Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallMultiplier * Time.deltaTime);
			Ctx.ApplieMovementY = Mathf.Max((previsouYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
		}
		else
		{
			float previsouYVelocity = Ctx.CurrentMovementY;
			Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
			Ctx.ApplieMovementY = (previsouYVelocity + Ctx.CurrentMovementY) * .5f;
		}
	}

}
