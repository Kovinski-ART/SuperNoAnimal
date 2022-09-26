using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState() { }
	public override void UpdateState()
	{
		CheckSwithStates();
		Ctx.ApplieMovementX = Ctx.InputMovement.x * Ctx.MoveSpeed;
		Ctx.ApplieMovementZ = Ctx.InputMovement.y * Ctx.MoveSpeed;
	}
	public override void ExitState() { }
	public override void CheckSwithStates()
	{
		if (!Ctx.IsMoventPressed)
		{
			SwitchState(Factory.Idle());
		}
		else if (Ctx.IsMoventPressed && Ctx.IsRunPressed)
		{
			SwitchState(Factory.Run());
		}
	}
	public override void InitializeSubState() { }
}
