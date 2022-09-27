using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemChest : MonoBehaviour
{
	//private Controls _input;
	[SerializeField] Item item;
	[SerializeField] Inventory inventory;
	private bool isEmpty;
	//[SerializeField] KeyCode itemPickupKeycode = KeyCode.E;

	private bool isRange;

	private void Awake()
	{
		//_input = new Controls();
	}

	private void Update()
	{
		Keyboard kb = InputSystem.GetDevice<Keyboard>();

		if (isRange && kb.eKey.wasPressedThisFrame)
		{

			if (!isEmpty)
			{
				if (inventory.AddItem(Instantiate(item)))
				{
					Debug.Log("Chest item Up");
				}
				isEmpty = true;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		isRange = true;
	}

	private void OnTriggerExit(Collider other)
	{
		isRange = false;
	}
}
