using System.Collections;
using System.Collections.Generic;
using Stat;
using UnityEngine;


public class Character : MonoBehaviour
{
	public CharacterStats Strength;
	public CharacterStats Agility;
	public CharacterStats Intelligence;
	public CharacterStats Vitality;

	[SerializeField] Inventory inventory;
	[SerializeField] EquipmentPanel equipmentPanel;
	[SerializeField] StatPanel statPanel;

	private void Awake()
	{
		statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
		statPanel.UpdateStatValues();

		inventory.OnItemLeftClickEvent += EquipFromInventory;
		equipmentPanel.OnItemLeftClickEvent += UnequipFromInventory;
	}

	private void EquipFromInventory(Item item)
	{
		Debug.Log($"Выполнение события экипирования {item.ItemName}");
		if (item is EquippableItem)
		{
			Equip((EquippableItem)item);
		}
	}

	private void UnequipFromInventory(Item item)
	{
		Debug.Log($"Выполнение события удаления {item.ItemName}");
		if (item is EquippableItem)
		{
			Unequip((EquippableItem)item);
		}
	}

	public void Equip(EquippableItem item)
	{
		if (inventory.RemoveItem(item))
		{
			EquippableItem previousItem;
			if (equipmentPanel.AddItem(item, out previousItem))
			{
				if (previousItem != null)
				{
					inventory.AddItem(previousItem);
					previousItem.Unequip(this);
					statPanel.UpdateStatValues();
				}
				item.Equip(this);
				statPanel.UpdateStatValues();
			}
			else
			{
				Debug.Log($"AddItem inventory {item.ItemName}");
				inventory.AddItem(item);
			}
		}
	}

	public void Unequip(EquippableItem item)
	{
		if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
		{
			item.Unequip(this);
			statPanel.UpdateStatValues();
			inventory.AddItem(item);
		}
	}
}

