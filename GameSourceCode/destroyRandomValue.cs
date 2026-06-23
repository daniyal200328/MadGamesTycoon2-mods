using UnityEngine;

public class destroyRandomValue : MonoBehaviour
{
	public int rand = 95;

	private void Start()
	{
		if (Random.Range(0, 100) < rand)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
