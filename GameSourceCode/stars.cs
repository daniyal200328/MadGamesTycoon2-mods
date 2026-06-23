using UnityEngine;
using UnityEngine.UI;

public class stars : MonoBehaviour
{
	public Color[] myColors;

	public GameObject[] myObjects;

	public int amount;

	private void Start()
	{
	}

	private void Update()
	{
		for (int i = 0; i < myObjects.Length; i++)
		{
			if (amount > i)
			{
				myObjects[i].GetComponent<Image>().color = myColors[0];
			}
			else
			{
				myObjects[i].GetComponent<Image>().color = myColors[1];
			}
		}
	}
}
