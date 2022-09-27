using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
	Helmet,
	Chest,
	Gloves,
	Boots,
	Weapon1,
	Accessory1,
}

[CreateAssetMenu]
public class EquippableItem : Item
{
	public int StrengthBonus;
	public int AgilityBonus;
	public int IntelligenceBonus;
	public int VitalityBonus;
	[Space]
	public float StrengthPercentBonus;
	public float AgilityPercentBonus;
	public float IntelligencePercentBonus;
	public float VitalityPercentBonus;
	[Space]
	public EquipmentType EquipmentType;

	public void Equip(Character c)
	{
		if (StrengthBonus != 0)
			c.Strength.AddModifier(new Stat.StatModifier(StrengthBonus, Stat.StatModType.Flat, this));
		if (AgilityBonus != 0)
			c.Agility.AddModifier(new Stat.StatModifier(AgilityBonus, Stat.StatModType.Flat, this));
		if (IntelligenceBonus != 0)
			c.Intelligence.AddModifier(new Stat.StatModifier(IntelligenceBonus, Stat.StatModType.Flat, this));
		if (VitalityBonus != 0)
			c.Vitality.AddModifier(new Stat.StatModifier(VitalityBonus, Stat.StatModType.Flat, this));

		if (StrengthPercentBonus != 0)
			c.Strength.AddModifier(new Stat.StatModifier(StrengthPercentBonus, Stat.StatModType.PercentMult, this));
		if (AgilityPercentBonus != 0)
			c.Agility.AddModifier(new Stat.StatModifier(AgilityPercentBonus, Stat.StatModType.PercentMult, this));
		if (IntelligencePercentBonus != 0)
			c.Intelligence.AddModifier(new Stat.StatModifier(IntelligencePercentBonus, Stat.StatModType.PercentMult, this));
		if (VitalityPercentBonus != 0)
			c.Vitality.AddModifier(new Stat.StatModifier(VitalityPercentBonus, Stat.StatModType.PercentMult, this));
	}
	public void Unequip(Character c)
	{
		c.Strength.RemoveAllModifiersFromSource(this);
		c.Agility.RemoveAllModifiersFromSource(this);
		c.Intelligence.RemoveAllModifiersFromSource(this);
		c.Vitality.RemoveAllModifiersFromSource(this);
	}
}
