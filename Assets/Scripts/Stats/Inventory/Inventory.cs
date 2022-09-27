using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IItemContainer
{
	[SerializeField] List<Item> startingItems;
	[SerializeField] Transform itemsParent;
	[SerializeField] ItemSlot[] itemSlots;

	public event Action<Item> OnItemLeftClickEvent;

	private void Start()
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			itemSlots[i].OnLeftClickEvent += OnItemLeftClickEvent;
		}
		SetStartingItems();
	}

	private void OnValidate()
	{
		if (itemsParent != null)
			itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

		//SetStartingItems();
	}

	private void SetStartingItems()
	{
		int i = 0;
		for (; i < startingItems.Count && i < itemSlots.Length; i++)
		{
			itemSlots[i].Item = Instantiate(startingItems[i]);
		}
		for (; i < itemSlots.Length; i++)
		{
			itemSlots[i].Item = null;
		}
	}


	public bool AddItem(Item item)
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item == null)
			{
				itemSlots[i].Item = item;
				return true;
			}
		}
		return false;
	}

	public bool RemoveItem(Item item)
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item == item)
			{
				itemSlots[i].Item = null;
				return true;
			}
		}
		return false;
	}

	public Item RemoveItem(string itemID)
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			Item item = itemSlots[i].Item;
			if (item != null && item.ID == itemID)
			{
				itemSlots[i].Item = null;
				return item;
			}
		}
		return null;
	}

	public bool IsFull()
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool ContainsItem(Item item)
	{
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item == item)
			{
				return true;
			}
		}
		return false;
	}

	public int ItemCount(string itemID)
	{
		int number = 0;
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item.ID == itemID)
			{
				number++;
			}
		}
		return number;
	}
}