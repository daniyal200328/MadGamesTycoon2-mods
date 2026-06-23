using System.Collections;
using UnityEngine;

public class arbeitsmarkt : MonoBehaviour
{
	public const int perk_player = 0;

	public const int perk_starDesigner = 1;

	public const int perk_noPause = 2;

	public const int perk_noBugs = 3;

	public const int perk_loyal = 4;

	public const int perk_talent = 5;

	public const int perk_glueck = 6;

	public const int perk_sport = 7;

	public const int perk_sauber = 8;

	public const int perk_naturfreund = 9;

	public const int perk_krank = 10;

	public const int perk_frieren = 11;

	public const int perk_bescheiden = 12;

	public const int perk_klo = 13;

	public const int perk_fuehrung = 14;

	public const int perk_allrounder = 15;

	public const int perk_unordentlich = 16;

	public const int perk_menschenfreund = 17;

	public const int perk_gierig = 18;

	public const int perk_immunschwach = 19;

	public const int perk_unbelastbar = 20;

	public const int perk_unkonzentriert = 21;

	public const int perk_untalentiert = 22;

	public const int perk_pixelArtist = 23;

	public const int perk_portSpecialist = 24;

	public const int perk_serienDesigner = 25;

	public const int perk_engineExperte = 26;

	public const int perk_noCritic = 27;

	public const int perk_arbeitstier = 28;

	public const int perk_effizient = 29;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

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
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public charArbeitsmarkt CreateArbeitsmarktItem()
	{
		charArbeitsmarkt component = Object.Instantiate(uiPrefabs[0]).GetComponent<charArbeitsmarkt>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.guiMain_ = guiMain_;
		return component;
	}

	public void ArbeitsmarktUpdaten(bool dontDelete)
	{
		if (mS_.multiplayer && mS_.mpCalls_.isClient)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
		if (!dontDelete)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				charArbeitsmarkt component = array[i].GetComponent<charArbeitsmarkt>();
				if ((bool)component)
				{
					component.wochenAmArbeitsmarkt++;
					if (component.wochenAmArbeitsmarkt > 12 && Random.Range(0, component.wochenAmArbeitsmarkt * 3) > Random.Range(0, 100))
					{
						StartCoroutine(Remove(component));
					}
				}
			}
		}
		if (mS_.globalEvent == 3)
		{
			return;
		}
		int num = array.Length;
		int num2 = 30;
		int num3 = 0;
		if (mS_.settings_sandbox)
		{
			switch (mS_.sandbox_arbeitsmarkt)
			{
			case 0:
				num2 = 30;
				num3 = 0;
				break;
			case 1:
				num2 = 50;
				num3 = 3;
				break;
			case 2:
				num2 = 80;
				num3 = 6;
				break;
			case 3:
				num2 = 100;
				num3 = 9;
				break;
			case 4:
				num2 = 150;
				num3 = 14;
				break;
			}
		}
		if (num >= num2)
		{
			return;
		}
		if (!mS_.multiplayer)
		{
			for (int j = 0; j < 3 + num3; j++)
			{
				if (Random.Range(0, 100) > 50 || dontDelete)
				{
					charArbeitsmarkt charArbeitsmarkt2 = CreateArbeitsmarktItem();
					if ((bool)charArbeitsmarkt2)
					{
						charArbeitsmarkt2.Create(null);
						num++;
					}
				}
				if (num >= num2)
				{
					break;
				}
			}
			return;
		}
		for (int k = 0; k < 7; k++)
		{
			if (Random.Range(0, 100) > 50 || dontDelete)
			{
				charArbeitsmarkt charArbeitsmarkt3 = CreateArbeitsmarktItem();
				if ((bool)charArbeitsmarkt3)
				{
					charArbeitsmarkt3.Create(null);
					num++;
				}
			}
			if (num >= num2)
			{
				break;
			}
		}
	}

	private IEnumerator Remove(charArbeitsmarkt script_)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if ((bool)script_)
		{
			script_.RemoveFromArbeitsmarkt(eingestellt: false);
		}
	}
}
