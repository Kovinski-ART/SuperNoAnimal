using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootGravityState
{
	public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { IsRootState = true; }

	public override void EnterState()
	{
		InitializeSubState();
		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, true);
			Ctx.Animator.SetBool(Ctx.AnimIDGrounded, false);
		}
	}
	public override void UpdateState()
	{
		CheckSwithStates();
		HandleGravity();
	}

	public override void ExitState()
	{
		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetBool(Ctx.AnimIDFreeFall, false);
		}
	}

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
