using UnityEngine;

public class destroyRandom : MonoBehaviour
{
	private void Start()
	{
		if (Random.Range(0, 100) > 50)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
