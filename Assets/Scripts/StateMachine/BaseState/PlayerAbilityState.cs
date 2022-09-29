using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerAbilityState : PlayerBaseState
{
	
	private float _activeTime;

	RaycastHit m_Hit;
	private bool m_HitDetect;
	private float m_MaxDistance = .5f;
	public PlayerAbilityState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

	public override void EnterState()
	{
		Ctx.ApplieMovementX = 0;
		Ctx.ApplieMovementZ = 0;
		
		if (Ctx.HasAnimator)
		{
			Ctx.Animator.SetBool(Ctx.AnimIDAbility, true);
		}
		Ctx.StateAbility = AbilityState.Active;
		_activeTime = Ctx.ability1.activeTime;
	}
	
	public override void UpdateState()
	{
		
		m_HitDetect = Physics.BoxCast(Ctx.transform.position + new Vector3(0, 1, 0), new Vector3(1, 0.55f, 0.4f), Ctx.transform.forward, out m_Hit, Ctx.transform.rotation,m_MaxDistance);
		if (m_HitDetect)
		{
			Debug.Log(m_Hit.collider.name);
			PushRigidBodies(m_Hit);
		}
		switch (Ctx.StateAbility)
		{
			case AbilityState.Active:
				Ctx.ability1.Activate(Ctx.CharacterController);
				if (_activeTime > 0)
				{

					_activeTime -= Time.deltaTime;
				}
				else
				{
					Ctx.StateAbility = AbilityState.Cooldown;
					Ctx.CooldownTimeAbility = Ctx.ability1.cooldownTime;
				}
				break;
			default:
				break;
		}
		CheckSwithStates();

	}
	
	private void PushRigidBodies(RaycastHit hit)
	{
		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;
		
		// Calculate push direction from move direction, horizontal motion only
		Vector3 pushDir = new Vector3(hit.point.x, 0, hit.point.z); //Ctx.transform.forward;

		// Apply the push and take strength into account
		body.AddForce(pushDir * 1.5f, ForceMode.Impulse);
	}
	
	
	public override void ExitState()
	{
		Ctx.HeadColider.enabled = false;
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
		if (Ctx.StateAbility == AbilityState.Cooldown)
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
