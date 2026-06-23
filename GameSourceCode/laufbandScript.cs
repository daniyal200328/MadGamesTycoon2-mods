using UnityEngine;

public class laufbandScript : MonoBehaviour
{
	public MeshRenderer myRenderer;

	private GameObject main_;

	private mainScript mS_;

	private objectScript oS_;

	private int aktuellesMaterial = -1;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!myRenderer)
		{
			myRenderer = base.gameObject.GetComponent<MeshRenderer>();
		}
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!oS_)
		{
			oS_ = base.transform.root.gameObject.GetComponent<objectScript>();
			if (mS_.multiplayer && !oS_)
			{
				Object.Destroy(this);
			}
		}
	}

	private void Update()
	{
		if (!oS_)
		{
			FindScripts();
		}
		else
		{
			if (!oS_.enabled || oS_.picked || !myRenderer.isVisible || !oS_)
			{
				return;
			}
			if (oS_.inUse)
			{
				if (aktuellesMaterial != 0)
				{
					myRenderer.material = mS_.specialMaterials[8];
					aktuellesMaterial = 0;
				}
			}
			else if (aktuellesMaterial != 1)
			{
				myRenderer.material = mS_.specialMaterials[9];
				aktuellesMaterial = 1;
			}
		}
	}
}
