using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
	public Ability ability;
	float cooldownTime;
	float activeTime;

	private StarterAssetsInputs _input;
	private ThirdPersonController _controller;
	private Animator _animator;
	private bool _hasAnimator;


	// animation IDs
	private int _animIDAbility;


	enum AbilityState
	{
		ready,
		active,
		cooldown
	}
	AbilityState state = AbilityState.ready;

	private void Start()
	{
		_hasAnimator = TryGetComponent(out _animator);
		_input = GetComponent<StarterAssetsInputs>();
		_controller = GetComponent<ThirdPersonController>();

		AssignAnimationIDs();
	}

	private void AssignAnimationIDs()
	{
		_animIDAbility = Animator.StringToHash("Ability");

	}

	void Update()
	{
		_hasAnimator = TryGetComponent(out _animator);

		switch (state)
		{
			case AbilityState.ready:
				if (_input.ability)
				{
					ability.Activate(gameObject);
					state = AbilityState.active;
					activeTime = ability.activeTime;
				}
				break;
			case AbilityState.active:
				if (activeTime > 0)
				{
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDAbility, true);
					}
					activeTime -= Time.deltaTime;
				}
				else
				{
					ability.BeginCooldown(gameObject);
					state = AbilityState.cooldown;
					cooldownTime = ability.cooldownTime;
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDAbility, false);
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
					_input.ability = false;
				}
				break;
		}

	}
}
