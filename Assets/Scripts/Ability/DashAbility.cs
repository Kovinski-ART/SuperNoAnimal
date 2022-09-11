using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "SuperNoAnimal/DashAbility", order = 0)]
public class DashAbility : Ability
{
	public float dashVelocity;
	private float _targetRotation = 0.0f;

	public override void Activate(GameObject parent)
	{
		CharacterController controller = parent.GetComponent<CharacterController>();

		// move the player
		controller.Move(parent.transform.forward * (dashVelocity * Time.deltaTime));
	}

	public override void BeginCooldown(GameObject parent)
	{

	}
}
