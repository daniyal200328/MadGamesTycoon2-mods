using UnityEngine;
using UnityEngine.UI;

public class maschieneScript : MonoBehaviour
{
	private GameObject main_;

	private objectScript oS_;

	public mainScript mS_;

	private Camera myCamera;

	private GUI_Main guiMain;

	public sfxScript sfx_;

	public mapScript mapS_;

	public textScript tS_;

	private games games_;

	public Animation[] myAnimation;

	public MeshRenderer bahn;

	public GameObject[] disketten1976;

	public GameObject[] disketten1985;

	public GameObject[] disketten1995;

	public GameObject uiMaschiene;

	private GameObject myUI;

	private GameObject uiIconMain;

	private GameObject uiWorkProgress;

	private RectTransform myUI_RectTransform;

	private Image uiWorkProgress_Image;

	private float invisibleTimer;

	private float updateProgressTimer;

	private float updateTimer;

	private float updateDisketteTimer;

	private void Start()
	{
		FindScripts();
		InitUI();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!guiMain)
		{
			guiMain = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = GameObject.FindWithTag("Main").GetComponent<textScript>();
		}
		if (!oS_)
		{
			oS_ = GetComponent<objectScript>();
		}
	}

	private void OnDestroy()
	{
		if ((bool)myUI)
		{
			Object.Destroy(myUI);
		}
	}

	private void InitUI()
	{
		myUI = Object.Instantiate(uiMaschiene, new Vector3(99999f, 99999f, 0f), Quaternion.identity);
		myUI.transform.SetParent(mS_.guiPops.transform);
		myUI.transform.SetSiblingIndex(0);
		myUI_RectTransform = myUI.GetComponent<RectTransform>();
		uiIconMain = myUI.transform.Find("IconMain").gameObject;
		uiWorkProgress = uiIconMain.transform.Find("WorkProgress").gameObject;
		uiWorkProgress_Image = uiWorkProgress.GetComponent<Image>();
	}

	private void UpdateUI(bool show)
	{
		if (guiMain.menuOpen || oS_.picked || !show)
		{
			if (myUI.activeSelf)
			{
				myUI.SetActive(value: false);
			}
			return;
		}
		Vector3 position = base.gameObject.transform.position;
		position.y += 1f;
		if (!myUI.activeSelf)
		{
			invisibleTimer += Time.deltaTime;
			if (invisibleTimer < 0.1f)
			{
				return;
			}
			invisibleTimer = 0f;
		}
		Vector2 vector = myCamera.WorldToScreenPoint(position);
		if (vector.x >= 0f && vector.x <= (float)Screen.width && vector.y >= 0f && vector.y <= (float)Screen.height)
		{
			if (!myUI.activeSelf)
			{
				myUI.SetActive(value: true);
			}
			vector = new Vector2(vector.x, vector.y - (float)Screen.height);
			myUI_RectTransform.anchoredPosition = guiMain.GetAnchoredPosition(vector);
			updateProgressTimer += Time.deltaTime;
			if (updateProgressTimer >= 0.04f)
			{
				updateProgressTimer = 0f;
				uiWorkProgress_Image.fillAmount = oS_.maschieneTimer;
			}
		}
		else if (myUI.activeSelf)
		{
			myUI.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!oS_ || !oS_.enabled)
		{
			return;
		}
		UpdateDisketten();
		bool flag = UpdateMaschine();
		if (!flag)
		{
			oS_.maschieneTimer = 0f;
		}
		UpdateUI(flag);
		updateTimer += Time.deltaTime;
		if (updateTimer < 0.2f)
		{
			return;
		}
		updateTimer = 0f;
		if (flag)
		{
			if (!myAnimation[0].isPlaying)
			{
				myAnimation[0].Play();
				myAnimation[1].Play();
			}
			bahn.material = mS_.specialMaterials[2];
			myAnimation[0]["maschine1"].speed = mS_.GetGameSpeed();
			myAnimation[1]["maschineLight"].speed = mS_.GetGameSpeed();
		}
		else
		{
			bahn.material = mS_.specialMaterials[5];
			myAnimation[0]["maschine1"].speed = 0f;
			myAnimation[1]["maschineLight"].speed = 0f;
		}
	}

	private bool UpdateMaschine()
	{
		if (!oS_.isMaschine)
		{
			return false;
		}
		if (oS_.picked)
		{
			return false;
		}
		int num = Mathf.RoundToInt(oS_.gameObject.transform.position.x);
		int num2 = Mathf.RoundToInt(oS_.gameObject.transform.position.z);
		if (!mapS_.IsInMapLimit(num, num2))
		{
			return false;
		}
		roomScript roomScript2 = mapS_.mapRoomScript[num, num2];
		if ((bool)roomScript2 && (bool)roomScript2.taskGameObject && roomScript2.taskID != -1)
		{
			int num3 = oS_.qualitaet * 5000;
			if (mS_.settings_sandbox && mS_.sandbox_maschineSpeed > 1f)
			{
				num3 = Mathf.RoundToInt((float)num3 * mS_.sandbox_maschineSpeed);
			}
			taskUnterstuetzen taskUnterstuetzen2 = roomScript2.GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2)
			{
				roomScript2 = taskUnterstuetzen2.rS_;
				if (!roomScript2)
				{
					return false;
				}
			}
			taskProduction taskProduction2 = roomScript2.GetTaskProduction();
			if ((bool)taskProduction2)
			{
				if (games_.GetFreeLagerplatz() <= 0)
				{
					return false;
				}
				if (taskProduction2.WaitForAutomatic())
				{
					return false;
				}
				oS_.maschieneTimer += mS_.GetDeltaTime() * 0.3f;
				if (oS_.maschieneTimer > 1f)
				{
					oS_.maschieneTimer = 0f;
					taskProduction2.Work(num3, base.transform.position);
				}
				return true;
			}
			taskContractWork taskContractWork2 = roomScript2.GetTaskContractWork();
			if ((bool)taskContractWork2)
			{
				oS_.maschieneTimer += mS_.GetDeltaTime() * 0.3f;
				if (oS_.maschieneTimer > 1f)
				{
					oS_.maschieneTimer = 0f;
					taskContractWork2.Work(num3);
				}
				return true;
			}
		}
		return false;
	}

	private void UpdateDisketten()
	{
		updateDisketteTimer += Time.deltaTime;
		if (updateDisketteTimer < 1f)
		{
			return;
		}
		updateDisketteTimer = 0f;
		if (mS_.year < 1985)
		{
			for (int i = 0; i < disketten1976.Length; i++)
			{
				if ((bool)disketten1976[i] && !disketten1976[i].activeSelf)
				{
					disketten1976[i].SetActive(value: true);
				}
			}
			for (int j = 0; j < disketten1985.Length; j++)
			{
				if ((bool)disketten1985[j] && disketten1985[j].activeSelf)
				{
					disketten1985[j].SetActive(value: false);
				}
			}
			for (int k = 0; k < disketten1995.Length; k++)
			{
				if ((bool)disketten1995[k] && disketten1995[k].activeSelf)
				{
					disketten1995[k].SetActive(value: false);
				}
			}
		}
		else if (mS_.year >= 1985 && mS_.year < 1995)
		{
			for (int l = 0; l < disketten1976.Length; l++)
			{
				if ((bool)disketten1976[l])
				{
					Object.Destroy(disketten1976[l]);
					disketten1976[l] = null;
				}
			}
			for (int m = 0; m < disketten1985.Length; m++)
			{
				if ((bool)disketten1985[m] && !disketten1985[m].activeSelf)
				{
					disketten1985[m].SetActive(value: true);
				}
			}
			for (int n = 0; n < disketten1995.Length; n++)
			{
				if ((bool)disketten1995[n] && disketten1995[n].activeSelf)
				{
					disketten1995[n].SetActive(value: false);
				}
			}
		}
		else
		{
			if (mS_.year < 1995)
			{
				return;
			}
			for (int num = 0; num < disketten1976.Length; num++)
			{
				if ((bool)disketten1976[num])
				{
					Object.Destroy(disketten1976[num]);
					disketten1976[num] = null;
				}
			}
			for (int num2 = 0; num2 < disketten1985.Length; num2++)
			{
				if ((bool)disketten1985[num2])
				{
					Object.Destroy(disketten1985[num2]);
					disketten1985[num2] = null;
				}
			}
			for (int num3 = 0; num3 < disketten1995.Length; num3++)
			{
				if ((bool)disketten1995[num3] && !disketten1995[num3].activeSelf)
				{
					disketten1995[num3].SetActive(value: true);
				}
			}
		}
	}
}
