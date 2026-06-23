using UnityEngine;

public class disableInTime : MonoBehaviour
{
	public float timeToLife = 3f;

	private float timer;

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= timeToLife)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		timer = 0f;
	}
}
