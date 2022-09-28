using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAbilityState : PlayerBaseState
{
	AbilityState state = AbilityState.ready;
	float activeTime;

	public PlayerAbilityState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState()
	{
		Ctx.ApplieMovementX = 0;
		Ctx.ApplieMovementZ = 0;

		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetBool(Ctx.AnimIDAbility, true);
		}
		Ctx.StateAbility = AbilityState.active;
		activeTime = Ctx.ability1.activeTime;
	}
	public override void UpdateState()
	{


		switch (Ctx.StateAbility)
		{
			case AbilityState.active:
				Ctx.ability1.Activate(Ctx.CharacterController);
				if (activeTime > 0)
				{

					activeTime -= Time.deltaTime;
				}
				else
				{
					Ctx.StateAbility = AbilityState.cooldown;
					Ctx.CooldownTimeAbility = Ctx.ability1.cooldownTime;
				}
				break;
			default:
				break;
		}
		CheckSwithStates();

	}
	public override void ExitState()
	{
		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetBool(Ctx.AnimIDAbility, false);
		}
	}
	public override void InitializeSubState()
	{

	}
	public override void CheckSwithStates()
	{
		if (Ctx.StateAbility == AbilityState.cooldown)
		{
			if (!Ctx.IsMoventPressed && !Ctx.IsRunPressed)
			{
				SwitchState(Factory.Idle());
			}
			else if (Ctx.IsMoventPressed && !Ctx.IsRunPressed)
			{
				SwitchState(Factory.Walk());
			}
			else
			{
				SwitchState(Factory.Run());
			}

		}
	}

}
