using UnityEngine;

public class Item_GenreFit : MonoBehaviour
{
	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
