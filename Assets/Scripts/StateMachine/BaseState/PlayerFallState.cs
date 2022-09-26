using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootGravityState
{
	public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { IsRootState = true; }

	public override void EnterState()
	{
		InitializeSubState();
	}
	public override void UpdateState()
	{
		CheckSwithStates();
		HandleGravity();
	}
	public override void ExitState() { }

	public void HandleGravity()
	{
		float previsouYVelocity = Ctx.CurrentMovementY;
		Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
		Ctx.ApplieMovementY = Mathf.Max((previsouYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
	}

	public override void CheckSwithStates()
	{
		if (Ctx.CharacterController.isGrounded)
		{
			SwitchState(Factory.Grounded());
		}
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
}
