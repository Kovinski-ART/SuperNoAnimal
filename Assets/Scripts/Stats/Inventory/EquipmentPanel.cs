using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
	[SerializeField] Transform equipmentSlotParent;
	[SerializeField] EquipmentSlot[] equipmentSlots;

	public event Action<Item> OnItemLeftClickEvent;

	private void Start()
	{
		for (int i = 0; i < equipmentSlots.Length; i++)
		{
			equipmentSlots[i].OnLeftClickEvent += OnItemLeftClickEvent;
		}
	}

	private void OnValidate()
	{
		equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
	}

	public bool AddItem(EquippableItem item, out EquippableItem previousItem)
	{
		for (int i = 0; i < equipmentSlots.Length; i++)
		{
			if (equipmentSlots[i].EquipmentType == item.EquipmentType)
			{
				previousItem = (EquippableItem)equipmentSlots[i].Item;
				equipmentSlots[i].Item = item;
				return true;
			}
		}
		previousItem = null;
		Debug.Log("Не один item не добавлен так как нет соответсвий EquipmentType");
		return false;
	}
	public bool RemoveItem(EquippableItem item)
	{
		for (int i = 0; i < equipmentSlots.Length; i++)
		{
			if (equipmentSlots[i].Item == item)
			{
				equipmentSlots[i].Item = null;
				return true;
			}
		}
		Debug.Log("Не один item не удалён так как нет соответсвий Item");
		return false;
	}
}
