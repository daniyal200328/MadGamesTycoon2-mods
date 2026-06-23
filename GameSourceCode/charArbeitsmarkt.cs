using UnityEngine;

public class charArbeitsmarkt : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public textScript tS_;

	public GUI_Main guiMain_;

	public clothScript clothScript_;

	public createCharScript cCS_;

	public int myID;

	public bool male;

	public string myName;

	public int wochenAmArbeitsmarkt;

	public int legend = -1;

	public int beruf;

	public float s_gamedesign;

	public float s_programmieren;

	public float s_grafik;

	public float s_sound;

	public float s_pr;

	public float s_gametests;

	public float s_technik;

	public float s_forschen;

	public bool[] perks;

	public int model_body;

	public int model_eyes;

	public int model_hair;

	public int model_beard;

	public int model_skinColor;

	public int model_hairColor;

	public int model_beardColor;

	public int model_HoseColor;

	public int model_ShirtColor;

	public int model_Add1Color;

	public bool mitarbeitersuche;

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
		if (!clothScript_)
		{
			clothScript_ = main_.GetComponent<clothScript>();
		}
		if (!cCS_)
		{
			cCS_ = main_.GetComponent<createCharScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Create(taskMitarbeitersuche task_)
	{
		FindScripts();
		myID = Random.Range(1, 99999999);
		base.name = "AA_" + myID;
		male = true;
		if (Random.Range(0, 100) < 33)
		{
			male = false;
		}
		myName = tS_.GetRandomCharName(male);
		if ((bool)task_)
		{
			mitarbeitersuche = true;
		}
		s_gamedesign = Random.Range(10f, 20f);
		s_programmieren = Random.Range(10f, 20f);
		s_grafik = Random.Range(10f, 20f);
		s_sound = Random.Range(10f, 20f);
		s_pr = Random.Range(10f, 20f);
		s_gametests = Random.Range(10f, 20f);
		s_technik = Random.Range(10f, 20f);
		s_forschen = Random.Range(10f, 20f);
		float num = 0f;
		if (!mS_.multiplayer)
		{
			num = mS_.GetStudioLevel(mS_.studioPoints) * 3;
		}
		if (!task_)
		{
			int num2 = Random.Range(0, 8);
			if (!mS_.multiplayer && (bool)mS_.forschungSonstiges_ && Random.Range(0, 100) > 20)
			{
				if (num2 == 4 && !mS_.forschungSonstiges_.IsErforscht(30) && !mS_.forschungSonstiges_.IsErforscht(29))
				{
					num2 = 0;
				}
				if (num2 == 5 && !mS_.forschungSonstiges_.IsErforscht(28))
				{
					num2 = 1;
				}
				if (num2 == 6 && !mS_.forschungSonstiges_.IsErforscht(38))
				{
					num2 = 7;
				}
			}
			switch (num2)
			{
			case 0:
				s_gamedesign = Random.Range(30f, 40f + num);
				beruf = 0;
				break;
			case 1:
				s_programmieren = Random.Range(30f, 40f + num);
				beruf = 1;
				break;
			case 2:
				s_grafik = Random.Range(30f, 40f + num);
				beruf = 2;
				break;
			case 3:
				s_sound = Random.Range(30f, 40f + num);
				beruf = 3;
				break;
			case 4:
				s_pr = Random.Range(30f, 40f + num);
				beruf = 4;
				break;
			case 5:
				s_gametests = Random.Range(30f, 40f + num);
				beruf = 5;
				break;
			case 6:
				s_technik = Random.Range(30f, 40f + num);
				beruf = 6;
				break;
			case 7:
				s_forschen = Random.Range(30f, 40f + num);
				beruf = 7;
				break;
			}
		}
		else
		{
			if (task_.geschlecht == 1)
			{
				male = true;
			}
			if (task_.geschlecht == 2)
			{
				male = false;
			}
			myName = tS_.GetRandomCharName(male);
			float num3 = Random.Range(30f, 35f);
			switch (task_.berufserfahrung)
			{
			case 0:
				num3 = Random.Range(30f, 35f);
				break;
			case 1:
				num3 = Random.Range(50f, 55f);
				break;
			case 2:
				num3 = Random.Range(70f, 75f);
				break;
			}
			switch (task_.beruf)
			{
			case 0:
				s_gamedesign = num3;
				beruf = 0;
				break;
			case 1:
				s_programmieren = num3;
				beruf = 1;
				break;
			case 2:
				s_grafik = num3;
				beruf = 2;
				break;
			case 3:
				s_sound = num3;
				beruf = 3;
				break;
			case 4:
				s_pr = num3;
				beruf = 4;
				break;
			case 5:
				s_gametests = num3;
				beruf = 5;
				break;
			case 6:
				s_technik = num3;
				beruf = 6;
				break;
			case 7:
				s_forschen = num3;
				beruf = 7;
				break;
			}
		}
		int num4 = 0;
		bool flag = false;
		if (mS_.year > 1976 && !task_ && (Random.Range(0, 50) == 1 || (mS_.globalEvent == 5 && Random.Range(0, 25) == 1)) && tS_.GetRandomDevLegend(this) != -1)
		{
			flag = true;
			s_gamedesign = Random.Range(10f, 20f);
			s_programmieren = Random.Range(10f, 20f);
			s_grafik = Random.Range(10f, 20f);
			s_sound = Random.Range(10f, 20f);
			s_pr = Random.Range(10f, 20f);
			s_gametests = Random.Range(10f, 20f);
			s_technik = Random.Range(10f, 20f);
			s_forschen = Random.Range(10f, 20f);
			switch (beruf)
			{
			case 0:
				s_gamedesign = Random.Range(80f, 95f);
				break;
			case 1:
				s_programmieren = Random.Range(80f, 95f);
				break;
			case 2:
				s_grafik = Random.Range(80f, 95f);
				break;
			case 3:
				s_sound = Random.Range(80f, 95f);
				break;
			case 4:
				s_pr = Random.Range(80f, 95f);
				break;
			case 5:
				s_gametests = Random.Range(80f, 95f);
				break;
			case 6:
				s_technik = Random.Range(80f, 95f);
				break;
			case 7:
				s_forschen = Random.Range(80f, 95f);
				break;
			}
			tS_.GetText(427);
			guiMain_.CreateTopNewsDevLegend(myName, beruf);
			for (int i = 0; i < perks.Length; i++)
			{
				if (perks[i])
				{
					num4++;
				}
			}
		}
		int num5 = -1;
		if ((bool)task_)
		{
			num5 = task_.perk;
			perks[task_.perk] = true;
			num4++;
		}
		for (int j = 0; j < 20; j++)
		{
			int num6 = Random.Range(0, perks.Length);
			if (num6 != 0 && num6 != 1 && num6 != num5 && (bool)guiMain_.uiPerks[num6] && Random.Range(0, 5) == 1 && num4 < 4)
			{
				perks[num6] = true;
				num4++;
				if (14 == num6 && beruf != 0)
				{
					perks[14] = false;
				}
				if (3 == num6 && beruf > 1)
				{
					perks[3] = false;
				}
				if (21 == num6 && beruf != 1)
				{
					perks[21] = false;
				}
				if (23 == num6 && beruf != 2)
				{
					perks[23] = false;
				}
				if (24 == num6 && beruf != 1)
				{
					perks[24] = false;
				}
				if (25 == num6 && beruf != 0)
				{
					perks[25] = false;
				}
				if (26 == num6 && beruf != 1)
				{
					perks[26] = false;
				}
				if (num6 == 10)
				{
					perks[19] = false;
				}
				if (num6 == 19)
				{
					perks[10] = false;
				}
				if (num5 == 10)
				{
					perks[19] = false;
					perks[10] = true;
				}
				if (num6 == 3)
				{
					perks[21] = false;
				}
				if (num6 == 21)
				{
					perks[3] = false;
				}
				if (num5 == 3)
				{
					perks[21] = false;
					perks[3] = true;
				}
				if (num6 == 2)
				{
					perks[20] = false;
				}
				if (num6 == 20)
				{
					perks[2] = false;
				}
				if (num5 == 2)
				{
					perks[20] = false;
					perks[2] = true;
				}
				if (num6 == 6)
				{
					perks[27] = false;
				}
				if (num6 == 27)
				{
					perks[6] = false;
				}
				if (num5 == 6)
				{
					perks[27] = false;
					perks[6] = true;
				}
				if (num6 == 5)
				{
					perks[22] = false;
				}
				if (num6 == 22)
				{
					perks[5] = false;
				}
				if (num5 == 5)
				{
					perks[22] = false;
					perks[5] = true;
				}
				if (perks[1])
				{
					RemoveBadPerks();
				}
				if ((bool)task_ && task_.noBadPerks)
				{
					RemoveBadPerks();
				}
			}
		}
		int num7 = 0;
		if (male)
		{
			model_body = Random.Range(0, cCS_.charGfxMales.Length);
			if (Random.Range(0, 100) < 20)
			{
				num7 = Random.Range(1, clothScript_.prefabMaleEyes.Length);
			}
			model_eyes = num7;
		}
		else
		{
			model_body = Random.Range(0, cCS_.charGfxFemales.Length);
			if (Random.Range(0, 100) < 20)
			{
				num7 = Random.Range(1, clothScript_.prefabFemaleEyes.Length);
			}
			model_eyes = num7;
		}
		if (male)
		{
			model_hair = -1;
			if (Random.Range(0, 100) > 10)
			{
				num7 = Random.Range(0, clothScript_.prefabMaleHairs.Length);
				model_hair = num7;
			}
		}
		else
		{
			num7 = Random.Range(0, clothScript_.prefabFemaleHairs.Length);
			model_hair = num7;
		}
		model_beard = -1;
		if (male && Random.Range(0, 100) < 33)
		{
			num7 = Random.Range(0, clothScript_.prefabBeards.Length);
			model_beard = num7;
		}
		if (Random.Range(0, 100) < 60)
		{
			num7 = Random.Range(0, clothScript_.matColor_Skin.Length);
			model_skinColor = num7;
		}
		else
		{
			model_skinColor = 0;
		}
		if (male)
		{
			model_beardColor = (model_hairColor = Random.Range(0, clothScript_.matColor_MaleHair.Length));
		}
		else
		{
			model_beardColor = (model_hairColor = Random.Range(0, clothScript_.matColor_FemaleHair.Length));
		}
		if (male)
		{
			num7 = Random.Range(0, clothScript_.matColor_MaleHose.Length);
			model_HoseColor = num7;
		}
		else
		{
			num7 = Random.Range(0, clothScript_.matColor_FemaleHose.Length);
			model_HoseColor = num7;
		}
		if (male)
		{
			num7 = Random.Range(0, clothScript_.matColor_MaleShirt.Length);
			model_ShirtColor = num7;
		}
		else
		{
			num7 = Random.Range(0, clothScript_.matColor_FemaleShirt.Length);
			model_ShirtColor = num7;
		}
		num7 = Random.Range(0, clothScript_.matColor_AllColors.Length);
		model_Add1Color = num7;
		if (flag)
		{
			if (tS_.model_body != -2)
			{
				model_body = tS_.model_body;
			}
			if (tS_.model_eyes != -2)
			{
				model_eyes = tS_.model_eyes;
			}
			if (tS_.model_hair != -2)
			{
				model_hair = tS_.model_hair;
			}
			if (tS_.model_beard != -2)
			{
				model_beard = tS_.model_beard;
			}
			if (tS_.model_skinColor != -2)
			{
				model_skinColor = tS_.model_skinColor;
			}
			if (tS_.model_hairColor != -2)
			{
				model_hairColor = tS_.model_hairColor;
			}
			if (tS_.model_beardColor != -2)
			{
				model_beardColor = tS_.model_hairColor;
			}
			if (tS_.model_HoseColor != -2)
			{
				model_HoseColor = tS_.model_HoseColor;
			}
			if (tS_.model_ShirtColor != -2)
			{
				model_ShirtColor = tS_.model_ShirtColor;
			}
			if (tS_.model_Add1Color != -2)
			{
				model_Add1Color = tS_.model_Add1Color;
			}
		}
		if (!task_ && mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			mS_.mpCalls_.SERVER_Send_CreateArbeitsmarkt(this);
		}
	}

	private void RemoveBadPerks()
	{
		if (perks[18])
		{
			perks[18] = false;
		}
		if (perks[19])
		{
			perks[19] = false;
		}
		if (perks[20])
		{
			perks[20] = false;
		}
		if (perks[21])
		{
			perks[21] = false;
		}
		if (perks[22])
		{
			perks[22] = false;
		}
		if (perks[27])
		{
			perks[27] = false;
		}
	}

	public void RemoveFromArbeitsmarkt(bool eingestellt)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (mS_.multiplayer && !mitarbeitersuche)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_KillAA(myID, 0, eingestellt, 0);
			}
			else if (!mS_.mpCalls_.disableSend)
			{
				mS_.mpCalls_.CLIENT_Send_DeleteArbeitsmarkt(myID, eingestellt);
			}
		}
		if (!eingestellt && legend != -1)
		{
			mS_.devLegendsInUse[legend] = false;
		}
		Object.Destroy(base.gameObject);
	}

	public int GetGehalt()
	{
		int num = Mathf.RoundToInt(0f + s_gamedesign + s_programmieren + s_grafik + s_sound + s_pr + s_gametests + s_technik + s_forschen) * 10;
		for (int i = 0; i < perks.Length; i++)
		{
			if (perks[i])
			{
				num = i switch
				{
					0 => num, 
					1 => num + 10000, 
					14 => num + 1000, 
					15 => num + 2000, 
					18 => num - 500, 
					19 => num - 500, 
					20 => num - 500, 
					21 => num - 500, 
					22 => num - 500, 
					27 => num - 500, 
					_ => num + 500, 
				};
			}
		}
		if (num < 1000)
		{
			num = 1000;
		}
		if (perks[18])
		{
			num *= 2;
		}
		if (mS_.settings_sandbox)
		{
			if (mS_.sandbox_mitarbeiterGehalt > 99f)
			{
				return 0;
			}
			if (mS_.sandbox_mitarbeiterGehalt > 0f)
			{
				return Mathf.RoundToInt((float)num * mS_.sandbox_mitarbeiterGehalt);
			}
		}
		return num;
	}

	public int GetBestSkill()
	{
		float num = 0f;
		num = s_gamedesign;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 0;
		}
		num = s_programmieren;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 1;
		}
		num = s_grafik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 2;
		}
		num = s_sound;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 3;
		}
		num = s_pr;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 4;
		}
		num = s_gametests;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 5;
		}
		num = s_technik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 6;
		}
		num = s_forschen;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 7;
		}
		return 0;
	}

	public float GetBestSkillValue()
	{
		float num = 0f;
		num = s_gamedesign;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_programmieren;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_grafik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_sound;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_pr;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_gametests;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_technik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_forschen;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik)
		{
			_ = s_forschen;
			return num;
		}
		return num;
	}

	public bool HasNegativPerk()
	{
		if (perks[18])
		{
			return true;
		}
		if (perks[19])
		{
			return true;
		}
		if (perks[20])
		{
			return true;
		}
		if (perks[21])
		{
			return true;
		}
		if (perks[22])
		{
			return true;
		}
		if (perks[27])
		{
			return true;
		}
		return false;
	}

	public int AmountPositivePerks()
	{
		int num = 0;
		for (int i = 0; i < perks.Length; i++)
		{
			if (perks[i])
			{
				num++;
			}
		}
		if (perks[18])
		{
			num--;
		}
		if (perks[19])
		{
			num--;
		}
		if (perks[20])
		{
			num--;
		}
		if (perks[21])
		{
			num--;
		}
		if (perks[22])
		{
			num--;
		}
		if (perks[27])
		{
			num--;
		}
		return num;
	}
}
