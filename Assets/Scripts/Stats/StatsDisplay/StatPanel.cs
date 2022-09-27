using UnityEngine;
using Stat;

public class StatPanel : MonoBehaviour
{
	[SerializeField] StatDisplay[] statDisplays;
	[SerializeField] string[] statNames;

	private CharacterStats[] stats;


	private void OnValidate()
	{
		statDisplays = GetComponentsInChildren<StatDisplay>();
		UpdateStatNames();
	}

	public void SetStats(params CharacterStats[] charStats)
	{
		stats = charStats;

		if (stats.Length > statDisplays.Length)
		{
			Debug.LogError("Not Enough Stats Displays!");
			return;
		}

		for (int i = 0; i < statDisplays.Length; i++)
		{
			statDisplays[i].gameObject.SetActive(i < stats.Length);
		}
	}

	public void UpdateStatValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].ValueText.text = stats[i].Value.ToString();
		}
	}

	public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statDisplays[i].NameText.text = statNames[i];
		}
	}
}
