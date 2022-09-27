using System.Text;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
	public TMP_Text ItemNameText;
	public TMP_Text ItemSlotText;
	public TMP_Text ItemStatsText;

	private StringBuilder sb = new StringBuilder();

	public void ShowTooltip(EquippableItem item)
	{
		ItemNameText.text = item.ItemName;
		ItemSlotText.text = item.EquipmentType.ToString();

		sb.Length = 0;
		AddStat(item.StrengthBonus, "Strength");
		AddStat(item.IntelligenceBonus, "Intelligence");
		AddStat(item.AgilityBonus, "Agility");
		AddStat(item.VitalityBonus, "Vitality");

		AddStat(item.StrengthPercentBonus, "Strength", true);
		AddStat(item.IntelligencePercentBonus, "Intelligence", true);
		AddStat(item.AgilityPercentBonus, "Agility", true);
		AddStat(item.VitalityPercentBonus, "Vitality", true);

		ItemStatsText.text = sb.ToString();

		gameObject.SetActive(true);
	}

	public void HideTooltip()
	{
		gameObject.SetActive(false);
	}

	private void AddStat(float value, string statNames, bool isPercent = false)
	{
		if (value != 0)
		{
			if (sb.Length > 0)
				sb.AppendLine();

			if (value > 0)
				sb.Append("+");

			if (isPercent)
			{
				sb.Append(value * 100);
				sb.Append("% ");
			}
			else
			{
				sb.Append(value);
				sb.Append(" ");
			}
			sb.Append(statNames);
		}
	}
}
