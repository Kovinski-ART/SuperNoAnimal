using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability : ScriptableObject
{
	public class MyFloatEvent : UnityEvent<float> { }
	public MyFloatEvent OnAbilityUse = new MyFloatEvent();

	[Header("Ability Info")]
	public string title;
	public Sprite icon;
	public float cooldownTime = 1;
	private bool canUse = true;
	public new string name;

	public float activeTime;



	public virtual void Activate(CharacterController character) { }
}

