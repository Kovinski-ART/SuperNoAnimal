using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState()
	{

		Debug.Log("Enter State Idel");
		Ctx.ApplieMovementX = 0;
		Ctx.ApplieMovementZ = 0;

	}
	public override void UpdateState()
	{
		if (Ctx.AnimationBlend != 0)
		{
			SetAnimation();
		}
		CheckSwithStates();
	}
	public override void ExitState() { }
	public override void CheckSwithStates()
	{
		if (Ctx.IsMoventPressed && Ctx.IsRunPressed)
		{
			SwitchState(Factory.Run());
		}
		else if (Ctx.IsMoventPressed)
		{
			SwitchState(Factory.Walk());
		}
	}
	public override void InitializeSubState() { }

	void SetAnimation()
	{
		Ctx.AnimationBlend = Mathf.Lerp(Ctx.AnimationBlend, 0, Time.deltaTime * 5f);
		if (Ctx.AnimationBlend < 0.01f) Ctx.AnimationBlend = 0;
		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetFloat(Ctx.AnimIDSpeed, Ctx.AnimationBlend);
			Ctx.Animator.SetFloat(Ctx.AnimIDMotionSpeed, 1);
		}
	}
}
