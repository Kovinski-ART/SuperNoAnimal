using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image image;
	[SerializeField] private ItemTooltip tooltip;

	public event Action<Item> OnLeftClickEvent;

	private Item _item;
	public Item Item
	{
		get { return _item; }
		set
		{
			_item = value;
			if (_item == null)
			{
				image.enabled = false;
			}
			else
			{
				image.sprite = _item.Icon;
				image.enabled = true;
			}

		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData != null && eventData.button == PointerEventData.InputButton.Left) //! Событи на нажатие Левой кнопки
		{
			if (Item == null)
				Debug.Log("Нет предмета");

			if (OnLeftClickEvent == null)
				Debug.Log("Никто не подписан на ивент");

			if (Item != null && OnLeftClickEvent != null)
			{
				OnLeftClickEvent(Item);
			}
		}
	}

	protected virtual void OnValidate()
	{
		if (image == null)
			Debug.Log("Not Sprite Slot");

		if (tooltip == null)
			tooltip = FindObjectOfType<ItemTooltip>();

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Item is EquippableItem)
		{
			tooltip.ShowTooltip((EquippableItem)Item);
		}

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip.HideTooltip();
	}
}
