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
	
	public new string name = "Ability";
	public float activeTime;
	public float cooldownTime = 1;



	public virtual void Activate(CharacterController character) { }
}

