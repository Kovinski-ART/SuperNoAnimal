using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState()
	{
		Ctx.ApplieMovementX = 0;
		Ctx.ApplieMovementZ = 0;
	}
	public override void UpdateState()
	{
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
}
