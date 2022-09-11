using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ability", menuName = "SuperNoAnimal/Ability", order = 0)]
public class Ability : ScriptableObject
{
	public new string name;
	public float cooldownTime;
	public float activeTime;

	public virtual void Activate(GameObject parent) { }
	public virtual void BeginCooldown(GameObject parent) { }
}
