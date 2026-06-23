using UnityEngine;

public class Item_FanGenre : MonoBehaviour
{
	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
