using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_BuyInventar : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject camera_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private mapScript mapS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private buildRoomScript buildRoomScript_;

	private roomDataScript rdS_;

	private mainCameraScript mCamS_;

	private sfxScript sfx_;

	private autoInventarScript autoInventar_;

	public int buyInventar;

	public bool[] filter = new bool[100];

	private float timerRightMousebutton;

	private Vector3 lastCameraPosition;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!buildRoomScript_)
		{
			buildRoomScript_ = main_.GetComponent<buildRoomScript>();
		}
		if (!autoInventar_)
		{
			autoInventar_ = main_.GetComponent<autoInventarScript>();
		}
		if (!mCamS_)
		{
			mCamS_ = GameObject.Find("Camera").GetComponent<mainCameraScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!camera_)
		{
			camera_ = GameObject.Find("CamMovement");
		}
	}

	private void OnEnable()
	{
		FindScripts();
		uiObjects[3].GetComponent<Toggle>().isOn = mS_.snapObject;
		uiObjects[5].GetComponent<Toggle>().isOn = mS_.snapRotation;
		for (int i = 0; i < uiObjects[2].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[2].transform.GetChild(i).gameObject);
		}
	}

	private void Update()
	{
		if (mS_.multiplayer && !guiMain_.menuOpen)
		{
			guiMain_.menuOpen = true;
		}
		mS_.snapObject = uiObjects[3].GetComponent<Toggle>().isOn;
		mS_.snapRotation = uiObjects[5].GetComponent<Toggle>().isOn;
		if (Input.GetMouseButtonDown(1))
		{
			lastCameraPosition = camera_.transform.position;
		}
		if (Input.GetMouseButtonUp(1) && timerRightMousebutton < 0.2f && Vector3.Distance(lastCameraPosition, camera_.transform.position) < 0.01f)
		{
			BUTTON_Abwahl();
		}
		if (Input.GetMouseButton(1))
		{
			timerRightMousebutton += Time.deltaTime;
		}
		else
		{
			timerRightMousebutton = 0f;
		}
	}

	public void OpenDropdown()
	{
		FindScripts();
		uiObjects[0].SetActive(value: true);
	}

	public void CloseDropdown()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		uiObjects[0].SetActive(value: false);
	}

	private void CreatePlaceholder()
	{
		Object.Instantiate(uiPrefabs[1]).transform.parent = uiObjects[2].transform;
	}

	private void CreateInventarKaufenButton(int typ)
	{
		GameObject obj = Object.Instantiate(uiPrefabs[0]);
		obj.transform.parent = uiObjects[2].transform;
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		Item_InventarKaufen component = obj.GetComponent<Item_InventarKaufen>();
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.mapS_ = mapS_;
		component.guiMain_ = guiMain_;
		component.sfx_ = sfx_;
		component.typ = typ;
		component.autoInventar_ = autoInventar_;
	}

	private void CreateFilter(string c, int filterArrayID)
	{
		GameObject obj = Object.Instantiate(uiPrefabs[2]);
		obj.transform.parent = uiObjects[2].transform;
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.GetChild(2).GetComponent<Text>().text = c;
		Filter_InventarKaufen component = obj.GetComponent<Filter_InventarKaufen>();
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.mapS_ = mapS_;
		component.guiMain_ = guiMain_;
		component.sfx_ = sfx_;
		component.filterArrayID = filterArrayID;
		if (filter[filterArrayID])
		{
			StartCoroutine(iButton_Click(component));
		}
	}

	public IEnumerator iButton_Click(Filter_InventarKaufen script_)
	{
		yield return new WaitForEndOfFrame();
		script_.BUTTON_Click();
	}

	public void BUTTON_SelectInventar(int room)
	{
		buyInventar = room;
		CloseDropdown();
		uiObjects[1].SetActive(value: true);
		switch (room)
		{
		case 0:
			CreateFilter(tS_.GetText(1877), 0);
			CreateInventarKaufenButton(32);
			CreateFilter(tS_.GetText(1876), 1);
			CreateInventarKaufenButton(25);
			CreateInventarKaufenButton(26);
			CreateInventarKaufenButton(27);
			CreateInventarKaufenButton(161);
			CreateInventarKaufenButton(162);
			CreateInventarKaufenButton(163);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 1:
			CreateFilter(tS_.GetText(1866), 3);
			CreateInventarKaufenButton(1);
			CreateInventarKaufenButton(50);
			CreateInventarKaufenButton(51);
			CreateInventarKaufenButton(52);
			CreateInventarKaufenButton(53);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 2:
			CreateFilter(tS_.GetText(1866), 6);
			CreateInventarKaufenButton(6);
			CreateInventarKaufenButton(56);
			CreateInventarKaufenButton(66);
			CreateInventarKaufenButton(67);
			CreateInventarKaufenButton(68);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 6:
			CreateFilter(tS_.GetText(1866), 9);
			CreateInventarKaufenButton(48);
			CreateInventarKaufenButton(57);
			CreateInventarKaufenButton(58);
			CreateInventarKaufenButton(59);
			CreateInventarKaufenButton(60);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 11:
			CreateFilter(tS_.GetText(1878), 12);
			CreateInventarKaufenButton(10);
			CreateInventarKaufenButton(186);
			CreateInventarKaufenButton(11);
			CreateInventarKaufenButton(187);
			CreateInventarKaufenButton(23);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 12:
			CreateFilter(tS_.GetText(1879), 13);
			CreateInventarKaufenButton(43);
			CreateInventarKaufenButton(164);
			CreateInventarKaufenButton(188);
			CreateInventarKaufenButton(24);
			CreateInventarKaufenButton(37);
			CreateInventarKaufenButton(38);
			CreateInventarKaufenButton(78);
			CreateInventarKaufenButton(99);
			CreateFilter(tS_.GetText(1876), 14);
			CreateInventarKaufenButton(33);
			CreateInventarKaufenButton(34);
			CreateInventarKaufenButton(35);
			CreateInventarKaufenButton(40);
			CreateInventarKaufenButton(41);
			CreateInventarKaufenButton(42);
			CreateInventarKaufenButton(69);
			CreateInventarKaufenButton(70);
			CreateInventarKaufenButton(71);
			CreateInventarKaufenButton(155);
			CreateInventarKaufenButton(156);
			CreateInventarKaufenButton(157);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 14:
			CreateFilter(tS_.GetText(1880), 16);
			CreateInventarKaufenButton(36);
			CreateInventarKaufenButton(115);
			CreateInventarKaufenButton(116);
			CreateInventarKaufenButton(117);
			CreateInventarKaufenButton(118);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 15:
			CreateFilter(tS_.GetText(1881), 17);
			CreateInventarKaufenButton(45);
			CreateInventarKaufenButton(125);
			CreateInventarKaufenButton(126);
			CreateInventarKaufenButton(127);
			CreateInventarKaufenButton(128);
			CreateFilter(tS_.GetText(1882), 18);
			CreateInventarKaufenButton(46);
			CreateInventarKaufenButton(154);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 9:
			CreateFilter(tS_.GetText(1883), 19);
			CreateInventarKaufenButton(47);
			CreateInventarKaufenButton(79);
			CreateInventarKaufenButton(80);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 13:
			CreateFilter(tS_.GetText(1884), 20);
			CreateInventarKaufenButton(54);
			CreateInventarKaufenButton(111);
			CreateInventarKaufenButton(112);
			CreateInventarKaufenButton(113);
			CreateInventarKaufenButton(114);
			CreateInventarKaufenButton(55);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 7:
			CreateFilter(tS_.GetText(1866), 23);
			CreateInventarKaufenButton(61);
			CreateInventarKaufenButton(62);
			CreateInventarKaufenButton(63);
			CreateInventarKaufenButton(64);
			CreateInventarKaufenButton(65);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 3:
			CreateFilter(tS_.GetText(1866), 26);
			CreateInventarKaufenButton(74);
			CreateInventarKaufenButton(88);
			CreateInventarKaufenButton(89);
			CreateInventarKaufenButton(90);
			CreateInventarKaufenButton(91);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 4:
			CreateFilter(tS_.GetText(1866), 29);
			CreateInventarKaufenButton(75);
			CreateInventarKaufenButton(103);
			CreateInventarKaufenButton(104);
			CreateInventarKaufenButton(105);
			CreateInventarKaufenButton(106);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 5:
			CreateFilter(tS_.GetText(1885), 32);
			CreateInventarKaufenButton(76);
			CreateInventarKaufenButton(81);
			CreateInventarKaufenButton(82);
			CreateInventarKaufenButton(119);
			CreateInventarKaufenButton(120);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 10:
			CreateFilter(tS_.GetText(1886), 35);
			CreateInventarKaufenButton(77);
			CreateInventarKaufenButton(121);
			CreateInventarKaufenButton(122);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 17:
			CreateFilter(tS_.GetText(1887), 38);
			CreateInventarKaufenButton(144);
			CreateInventarKaufenButton(145);
			CreateInventarKaufenButton(146);
			CreateInventarKaufenButton(147);
			CreateInventarKaufenButton(148);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 8:
			CreateFilter(tS_.GetText(1888), 41);
			CreateInventarKaufenButton(149);
			CreateInventarKaufenButton(150);
			CreateInventarKaufenButton(151);
			CreateInventarKaufenButton(152);
			CreateInventarKaufenButton(153);
			CreateFilter(tS_.GetText(1867), 4);
			CreateInventarKaufenButton(28);
			CreateInventarKaufenButton(29);
			CreateInventarKaufenButton(30);
			CreateInventarKaufenButton(108);
			CreateInventarKaufenButton(109);
			CreateInventarKaufenButton(110);
			CreateInventarKaufenButton(136);
			CreateInventarKaufenButton(137);
			CreateInventarKaufenButton(138);
			CreateInventarKaufenButton(31);
			CreateFilter(tS_.GetText(1868), 2);
			CreateInventarKaufenButton(100);
			CreateInventarKaufenButton(101);
			CreateInventarKaufenButton(102);
			CreateInventarKaufenButton(107);
			CreateInventarKaufenButtons_STANDARD(room);
			break;
		case 16:
			break;
		}
	}

	private void CreateInventarKaufenButtons_STANDARD(int room)
	{
		CreateFilter(tS_.GetText(1875), 44);
		CreateInventarKaufenButton(4);
		CreateInventarKaufenButton(5);
		CreateInventarKaufenButton(131);
		if (room != 5)
		{
			CreateInventarKaufenButton(72);
		}
		CreateInventarKaufenButton(2);
		CreateInventarKaufenButton(73);
		CreateInventarKaufenButton(185);
		if (room != 5)
		{
			CreateInventarKaufenButton(39);
		}
		CreateFilter(tS_.GetText(1874), 45);
		CreateInventarKaufenButton(3);
		CreateInventarKaufenButton(44);
		CreateInventarKaufenButton(49);
		CreateInventarKaufenButton(178);
		CreateInventarKaufenButton(179);
		CreateInventarKaufenButton(180);
		CreateFilter(tS_.GetText(1873), 46);
		CreateInventarKaufenButton(7);
		CreateInventarKaufenButton(8);
		CreateInventarKaufenButton(9);
		CreateInventarKaufenButton(181);
		CreateInventarKaufenButton(182);
		CreateInventarKaufenButton(183);
		CreateFilter(tS_.GetText(1872), 47);
		CreateInventarKaufenButton(158);
		CreateInventarKaufenButton(159);
		CreateInventarKaufenButton(160);
		CreateInventarKaufenButton(184);
		CreateFilter(tS_.GetText(1871), 48);
		if (room != 5)
		{
			CreateInventarKaufenButton(17);
			CreateInventarKaufenButton(129);
			CreateInventarKaufenButton(130);
		}
		CreateInventarKaufenButton(132);
		CreateInventarKaufenButton(133);
		CreateInventarKaufenButton(143);
		CreateInventarKaufenButton(135);
		CreateInventarKaufenButton(134);
		CreateInventarKaufenButton(142);
		if (room != 5)
		{
			CreateFilter(tS_.GetText(1869), 49);
			CreateInventarKaufenButton(12);
			CreateInventarKaufenButton(13);
			CreateInventarKaufenButton(14);
			CreateInventarKaufenButton(15);
			CreateInventarKaufenButton(16);
			CreateInventarKaufenButton(18);
			CreateInventarKaufenButton(19);
			CreateInventarKaufenButton(20);
			CreateInventarKaufenButton(21);
			CreateInventarKaufenButton(22);
			CreateInventarKaufenButton(83);
			CreateInventarKaufenButton(84);
			CreateInventarKaufenButton(85);
			CreateInventarKaufenButton(86);
			CreateInventarKaufenButton(87);
			CreateInventarKaufenButton(165);
			CreateInventarKaufenButton(166);
			CreateInventarKaufenButton(167);
			CreateInventarKaufenButton(168);
			CreateInventarKaufenButton(169);
			CreateInventarKaufenButton(170);
			CreateInventarKaufenButton(171);
			CreateInventarKaufenButton(172);
			CreateInventarKaufenButton(173);
			CreateInventarKaufenButton(174);
		}
		CreateFilter(tS_.GetText(1870), 50);
		CreateInventarKaufenButton(93);
		CreateInventarKaufenButton(94);
		CreateInventarKaufenButton(95);
		CreateInventarKaufenButton(96);
		CreateInventarKaufenButton(97);
		CreateInventarKaufenButton(98);
		CreateInventarKaufenButton(139);
		CreateInventarKaufenButton(140);
		CreateInventarKaufenButton(141);
		CreateInventarKaufenButton(175);
		CreateInventarKaufenButton(176);
		CreateInventarKaufenButton(177);
	}

	public void BUTTON_CloseSelectInventar(bool resetScrollbar)
	{
		if (resetScrollbar)
		{
			uiObjects[4].GetComponent<Scrollbar>().value = 1f;
		}
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(7);
		}
		mS_.UpdatePathfindingNextFrameExtern();
		if ((bool)mS_.pickedObject)
		{
			Object.Destroy(mS_.pickedObject);
		}
		sfx_.PlaySound(3, force: true);
		DisableAllMenus();
		guiMain_.CloseMenu();
		mS_.ResetAllColliderLayer();
		base.gameObject.SetActive(value: false);
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i])
			{
				mS_.arrayObjectScripts[i].WakeUpObject();
			}
		}
	}

	public void BUTTON_Abwahl()
	{
		if ((bool)mS_.pickedObject)
		{
			Object.Destroy(mS_.pickedObject);
		}
	}

	public void DisableAllMenus()
	{
		uiObjects[0].SetActive(value: false);
		uiObjects[1].SetActive(value: false);
	}
}
