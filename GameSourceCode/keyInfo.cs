using UnityEngine;
using UnityEngine.UI;

public class keyInfo : MonoBehaviour
{
	public KeyCode[] keys;

	public GameObject[] uiObjects;

	private void Start()
	{
		string text = "";
		for (int i = 0; i < keys.Length; i++)
		{
			text += keys[i];
			if (i < keys.Length - 1)
			{
				text += " & ";
			}
		}
		uiObjects[0].GetComponent<Text>().text = text;
	}
}
