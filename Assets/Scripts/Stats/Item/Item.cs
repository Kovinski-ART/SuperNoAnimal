using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
	[SerializeField] string id;
	public string ID { get { return id; } }
	public string ItemName = "Name Item";
	public Sprite Icon;

	private void OnValidate()
	{
		//string path = AssetDatabase.GetAssetPath(this);
		//id = AssetDatabase.AssetPathToGUID(path);
	}
}
