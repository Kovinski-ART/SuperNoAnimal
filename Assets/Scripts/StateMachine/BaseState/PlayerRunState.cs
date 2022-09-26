using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState() { }
	public override void UpdateState()
	{
		CheckSwithStates();
		Ctx.ApplieMovementX = Ctx.InputMovement.x * Ctx.RunMultiplier;
		Ctx.ApplieMovementZ = Ctx.InputMovement.y * Ctx.RunMultiplier;
	}
	public override void ExitState() { }
	public override void CheckSwithStates()
	{
		if (!Ctx.IsMoventPressed)
		{
			SwitchState(Factory.Idle());
		}
		else if (Ctx.IsMoventPressed && !Ctx.IsRunPressed)
		{
			SwitchState(Factory.Walk());
		}
	}
	public override void InitializeSubState() { }
}
