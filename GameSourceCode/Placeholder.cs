using UnityEngine;

public class Placeholder : MonoBehaviour
{
	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
