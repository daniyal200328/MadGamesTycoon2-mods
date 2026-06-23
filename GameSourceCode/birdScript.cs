using UnityEngine;

public class birdScript : MonoBehaviour
{
	public mainScript mS_;

	public GameObject main_;

	public Animation myAnim;

	private float updateTimer;

	public float speed = 4f;

	private float targetRotY;

	private float flughoehe = 5f;

	public bool resetPossible = true;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
	}

	private void Update()
	{
		base.transform.Translate(Vector3.forward * mS_.GetDeltaTime() * speed);
		float y = Mathf.LerpAngle(base.transform.eulerAngles.y, targetRotY, 0.05f);
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, y, base.transform.eulerAngles.z);
		float y2 = Mathf.Lerp(base.transform.position.y, flughoehe, 0.05f);
		base.transform.position = new Vector3(base.transform.position.x, y2, base.transform.position.z);
		if (base.transform.position.x < -40f || base.transform.position.x > 110f || base.transform.position.z < -40f || base.transform.position.z > 110f)
		{
			if (resetPossible)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Floor");
				if (array.Length != 0)
				{
					resetPossible = false;
					int num = Random.Range(0, array.Length);
					if ((bool)array[num])
					{
						Vector3 eulerAngles = base.gameObject.transform.eulerAngles;
						base.gameObject.transform.LookAt(array[num].transform);
						base.gameObject.transform.eulerAngles = new Vector3(0f, base.gameObject.transform.eulerAngles.y, 0f);
						targetRotY = base.gameObject.transform.eulerAngles.y;
						base.gameObject.transform.eulerAngles = eulerAngles;
						flughoehe = Random.Range(5f, 8f);
					}
				}
			}
		}
		else
		{
			resetPossible = true;
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 0.2f))
		{
			updateTimer = 0f;
			if ((bool)myAnim)
			{
				myAnim["BirdFly"].speed = mS_.GetGameSpeed();
			}
		}
	}
}
