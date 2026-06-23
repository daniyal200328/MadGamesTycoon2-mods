using UnityEngine;

public class newsTimer : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public settingsScript settings_;

	public float aliveTimer;

	private RectTransform myRect_;

	private void Start()
	{
		FindScripts();
	}

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

	public void UpdateMe(bool showAllNews)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (!showAllNews)
		{
			if (base.transform.parent.childCount > 3 && base.transform.parent.childCount - base.transform.GetSiblingIndex() > 3 && base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
		else if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		if (!(mS_.gameSpeed <= 0f))
		{
			aliveTimer += Time.deltaTime;
		}
		if (aliveTimer > settings_.newsTime)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
