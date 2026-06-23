using UnityEngine;

public class textChat : MonoBehaviour
{
	private float timer;

	private void Start()
	{
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 30f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
