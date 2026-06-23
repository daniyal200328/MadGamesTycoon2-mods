using UnityEngine;
using UnityEngine.UI;

public class setFont : MonoBehaviour
{
	private GameObject main_;

	private settingsScript settings_;

	private mainScript mS_;

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		Text component = GetComponent<Text>();
		if ((bool)component)
		{
			if (settings_.language == 3 || settings_.language == 10 || settings_.language == 19)
			{
				component.fontStyle = FontStyle.Normal;
			}
			if (settings_.language == 19)
			{
				component.font = mS_.fonts[1];
			}
		}
	}
}
