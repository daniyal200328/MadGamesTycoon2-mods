using UnityEngine;
using UnityEngine.UI;

public class moneyPop : MonoBehaviour
{
	public Camera camera_;

	public Vector3 myPosition;

	public float myTimer;

	public settingsScript settings_;

	private RectTransform rT_;

	private Animation anim_;

	public Text text_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!rT_)
		{
			rT_ = base.gameObject.GetComponent<RectTransform>();
		}
		if (!anim_)
		{
			anim_ = base.gameObject.transform.GetChild(0).GetComponent<Animation>();
		}
		if (!text_)
		{
			text_ = base.gameObject.transform.GetChild(0).GetComponent<Text>();
		}
	}

	public void Init(Vector3 v)
	{
		FindScripts();
		rT_.anchoredPosition = v;
		anim_.enabled = true;
		anim_.Play();
	}

	private void Update()
	{
		if (base.transform.position.x >= 99998f)
		{
			return;
		}
		myTimer += Time.deltaTime;
		if (myTimer > 3f)
		{
			base.transform.position = new Vector3(99999f, 99999f, 0f);
			anim_.enabled = false;
			text_.text = "";
		}
		else if ((bool)settings_)
		{
			Vector2 vector = camera_.WorldToScreenPoint(myPosition);
			if (vector.x >= 0f && vector.x <= (float)Screen.width && vector.y >= 0f && vector.y <= (float)Screen.height)
			{
				vector = new Vector2(vector.x, vector.y - (float)Screen.height);
				vector /= settings_.uiScale;
				rT_.anchoredPosition = vector;
			}
			else
			{
				base.transform.position = new Vector3(99999f, 99999f, 0f);
				anim_.enabled = false;
				text_.text = "";
			}
		}
	}
}
