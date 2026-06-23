using UnityEngine;
using UnityEngine.UI;

public class DisableInputSearch : MonoBehaviour
{
	private void OnDisable()
	{
		if ((bool)GetComponent<InputField>())
		{
			GetComponent<InputField>().text = "";
		}
	}
}
