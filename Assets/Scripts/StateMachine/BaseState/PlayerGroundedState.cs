using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootGravityState
{
	public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { IsRootState = true; }

	public override void EnterState()
	{
		InitializeSubState();
		Debug.Log("Enter State Grounded");

		Ctx.CurrentMovementY = Ctx.GroudedGravity;
		Ctx.ApplieMovementY = Ctx.GroudedGravity;
	}
	public override void UpdateState()
	{
		CheckSwithStates();
	}
	public override void ExitState()
	{
		Debug.Log("Exit State Grounded");
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
		if (Ctx.Ability)
		{
			SwitchState(Factory.Ability());
		}

		if (Ctx.IsJumpingPressed)
		{
			SwitchState(Factory.Jump());
		}
		else if (!Ctx.CharacterController.isGrounded)
		{
			SwitchState(Factory.Fall());
		}
	}

	public void HandleGravity()
	{

	}
}
