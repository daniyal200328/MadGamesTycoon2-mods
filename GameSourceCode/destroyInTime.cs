using UnityEngine;

public class destroyInTime : MonoBehaviour
{
	public float timeToLife = 3f;

	private float timer;

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= timeToLife)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
