using UnityEngine;

public class Menu_Init : MonoBehaviour
{
	public GameObject myMenu;

	public GameObject myParent;

	private void OnEnable()
	{
		if ((bool)myMenu)
		{
			myMenu.transform.SetParent(base.transform);
			myMenu.transform.SetAsFirstSibling();
		}
	}

	private void OnDisable()
	{
		_ = (bool)myMenu;
	}
}
