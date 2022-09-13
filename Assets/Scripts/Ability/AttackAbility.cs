using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAbility", menuName = "SuperNoAnimal/AttackAbility", order = 0)]
public class AttackAbility : Ability
{
	public float dashVelocity;
	private float _targetRotation = 0.0f;

	public override void Activate(GameObject parent)
	{
		//CharacterController controller = parent.GetComponent<CharacterController>();

		// move the player
		//controller.Move(parent.transform.forward * (dashVelocity * Time.deltaTime));
	}

	public override void BeginCooldown(GameObject parent)
	{

	}
}
