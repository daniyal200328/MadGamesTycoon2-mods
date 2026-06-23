using UnityEngine;

public class animatePop : MonoBehaviour
{
	private RectTransform rT_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!rT_)
		{
			rT_ = GetComponent<RectTransform>();
		}
	}

	public void Init()
	{
		FindScripts();
		rT_.anchoredPosition = new Vector2(0f, 38f);
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void Update()
	{
		if (base.gameObject.transform.localScale.x > 0f)
		{
			base.gameObject.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
		}
	}
}
