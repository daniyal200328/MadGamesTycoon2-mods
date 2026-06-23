using UnityEngine;
using UnityEngine.UI;

public class setText : MonoBehaviour
{
	private GameObject main_;

	private textScript tS_;

	private settingsScript settings_;

	public int textID = -1;

	public string textArray = "";

	public string c = "";

	private void Start()
	{
	}

	private void OnEnable()
	{
		FindScripts();
		if (textArray.Length > 0 && textID > -1)
		{
			c = tS_.GetText(textID);
		}
		GetComponent<Text>().text = c;
	}

	private void SetText()
	{
		if (textArray.Length > 0 && textID > -1)
		{
			c = tS_.GetText(textID);
		}
		GetComponent<Text>().text = c;
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
	}
}
