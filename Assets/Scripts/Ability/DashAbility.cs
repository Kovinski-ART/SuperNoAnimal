using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "SuperNoAnimal/Ability", order = 0)]
public class DashAbility : Ability
{
	public float dashVelocity;

	public override void Activate(CharacterController character)
	{
		Vector3 _applieMovement = character.transform.forward;
		character.Move(character.transform.forward * dashVelocity * Time.fixedDeltaTime);
	}
}
