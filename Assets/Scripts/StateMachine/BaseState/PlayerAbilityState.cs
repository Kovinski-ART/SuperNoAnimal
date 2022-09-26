using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAbilityState : PlayerBaseState
{
	AbilityState state = AbilityState.ready;
	float cooldownTime;
	float activeTime;

	public PlayerAbilityState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState()
	{
		Ctx.ApplieMovementX = 0;
		Ctx.ApplieMovementZ = 0;

		Debug.Log("Enter State Ability");
		//Ctx.ability1
		state = AbilityState.active;
		Ctx.ability1.Activate(Ctx.CharacterController);
		activeTime = Ctx.ability1.activeTime;

		if (Ctx.HasAnimator)
		{
			Debug.Log("Ability Animator SetBool.true");
			Ctx.Animator.SetBool(Ctx.AnimIDAbility, true);
		}

	}
	public override void UpdateState()
	{
		CheckSwithStates();

		switch (state)
		{
			case AbilityState.active:
				if (activeTime > 0)
				{
					activeTime -= Time.deltaTime;
				}
				else
				{
					state = AbilityState.cooldown;
					cooldownTime = Ctx.ability1.cooldownTime;

					if (Ctx.HasAnimator)
					{
						Debug.Log("Ability Animator SetBool.false");
						Ctx.Animator.SetBool(Ctx.AnimIDAbility, false);
					}
				}
				break;
			case AbilityState.cooldown:
				if (cooldownTime > 0)
				{
					cooldownTime -= Time.deltaTime;
				}
				else
				{
					state = AbilityState.ready;
				}
				break;
			default:
				Debug.Log(state);
				break;
		}


	}
	public override void ExitState()
	{
		Debug.Log("Exit State Ability");
		if (Ctx.HasAnimator)
		{
			Debug.Log("Ability Animator SetBool.false");
			Ctx.Animator.SetBool(Ctx.AnimIDAbility, false);
		}
	}
	public override void InitializeSubState()
	{

	}
	public override void CheckSwithStates()
	{
		if (state == AbilityState.cooldown || state == AbilityState.ready)
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
enum AbilityState
{
	ready,
	active,
	cooldown,
}