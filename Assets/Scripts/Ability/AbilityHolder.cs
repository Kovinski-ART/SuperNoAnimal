using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
	public List<Ability> ability;
	float cooldownTime;
	float activeTime;

	private StarterAssetsInputs _input;
	private ThirdPersonController _controller;
	private Animator _animator;
	private bool _hasAnimator;


	// animation IDs
	private int _animIDAbility;
	private int _animIDAbility1;

	private bool intput2;


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
		_animIDAbility1 = Animator.StringToHash("Ability2");

	}

	void Update()
	{
		_hasAnimator = TryGetComponent(out _animator);

		AbilityHolderActive(0, _animIDAbility, _input.ability, out _input.ability); //Dash
		AbilityHolderActive(1, _animIDAbility1, _input.ability1, out _input.ability1); //Base Attack

	}

	private void AbilityHolderActive(int iAbility, int animID, bool intput, out bool ints)
	{
		ints = intput;

		switch (ability[iAbility].state)
		{
			case Ability.AbilityState.ready:
				if (intput)
				{
					ability[iAbility].Activate(gameObject);
					ability[iAbility].state = Ability.AbilityState.active;
					activeTime = ability[iAbility].activeTime;
				}
				break;
			case Ability.AbilityState.active:
				if (activeTime > 0)
				{
					if (_hasAnimator)
					{
						_animator.SetBool(animID, true);
					}
					activeTime -= Time.deltaTime;
				}
				else
				{
					ability[iAbility].BeginCooldown(gameObject);
					ability[iAbility].state = Ability.AbilityState.cooldown;
					cooldownTime = ability[iAbility].cooldownTime;
					if (_hasAnimator)
					{
						_animator.SetBool(animID, false);
					}
				}
				break;
			case Ability.AbilityState.cooldown:
				if (cooldownTime > 0)
				{
					cooldownTime -= Time.deltaTime;
				}
				else
				{
					ability[iAbility].state = Ability.AbilityState.ready;
					ints = false;
				}
				break;
		}
	}
}
