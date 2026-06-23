using UnityEngine;

public class lockDestroy : MonoBehaviour
{
	public int unlockSlot = -1;

	public bool sonstigeForschung;

	public bool gameplayFeatures;

	private GameObject main_;

	private mainScript mS_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private gameplayFeatures gF_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (unlockSlot == -1)
		{
			return;
		}
		if (sonstigeForschung)
		{
			if (forschungSonstiges_.IsErforscht(unlockSlot))
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (gameplayFeatures)
		{
			if (gF_.IsErforscht(unlockSlot))
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (unlock_.Get(unlockSlot))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
