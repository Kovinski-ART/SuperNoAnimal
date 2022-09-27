using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] Inventory inventory;
	[SerializeField] EquipmentPanel equipmentPanel;

	private void Awake()
	{
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
					Debug.Log($"AddItem equipmentPanel {previousItem.ItemName}");
					inventory.AddItem(previousItem);
				}
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
			inventory.AddItem(item);
		}
	}
}
