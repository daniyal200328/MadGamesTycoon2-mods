using UnityEngine;

public class Menu_GameDesign : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private engineFeatures engineFeatures_;

	private gameplayFeatures gameplayFeatures_;

	private void Start()
	{
		FindScripts();
		CreateEngineFeatures();
		CreateGameplayFeatures();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
			engineFeatures_ = main_.GetComponent<engineFeatures>();
			gameplayFeatures_ = main_.GetComponent<gameplayFeatures>();
		}
	}

	private void CreateGameplayFeatures()
	{
		int num = 1;
		int num2 = 0;
		Transform parent = uiObjects[1].transform;
		num2 = 0;
		NewItem(uiPrefabs[7], parent);
		NewItem(uiPrefabs[1], parent);
		for (int i = 0; i < gameplayFeatures_.gameplayFeatures_TYP.Length; i++)
		{
			if (gameplayFeatures_.gameplayFeatures_TYP[i] == gameplayFeatures_.GetTypGameplay())
			{
				NewItem(uiPrefabs[6], parent).GetComponent<Item_GameplayFeatures_GameDesign>().myID = i;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[2], parent);
		NewItem(uiPrefabs[1], parent);
		for (int j = 0; j < gameplayFeatures_.gameplayFeatures_TYP.Length; j++)
		{
			if (gameplayFeatures_.gameplayFeatures_TYP[j] == gameplayFeatures_.GetTypGrafik())
			{
				NewItem(uiPrefabs[6], parent).GetComponent<Item_GameplayFeatures_GameDesign>().myID = j;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[9], parent);
		NewItem(uiPrefabs[1], parent);
		for (int k = 0; k < gameplayFeatures_.gameplayFeatures_TYP.Length; k++)
		{
			if (gameplayFeatures_.gameplayFeatures_TYP[k] == gameplayFeatures_.GetTypSteuerung())
			{
				NewItem(uiPrefabs[6], parent).GetComponent<Item_GameplayFeatures_GameDesign>().myID = k;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[3], parent);
		NewItem(uiPrefabs[1], parent);
		for (int l = 0; l < gameplayFeatures_.gameplayFeatures_TYP.Length; l++)
		{
			if (gameplayFeatures_.gameplayFeatures_TYP[l] == gameplayFeatures_.GetTypSound())
			{
				NewItem(uiPrefabs[6], parent).GetComponent<Item_GameplayFeatures_GameDesign>().myID = l;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[8], parent);
		NewItem(uiPrefabs[1], parent);
		for (int m = 0; m < gameplayFeatures_.gameplayFeatures_TYP.Length; m++)
		{
			if (gameplayFeatures_.gameplayFeatures_TYP[m] == gameplayFeatures_.GetTypMultiplayer())
			{
				NewItem(uiPrefabs[6], parent).GetComponent<Item_GameplayFeatures_GameDesign>().myID = m;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		NewItems(uiPrefabs[1], parent, num + 1);
	}

	private void CreateEngineFeatures()
	{
		int num = 1;
		int num2 = 0;
		Transform parent = uiObjects[0].transform;
		num2 = 0;
		NewItem(uiPrefabs[2], parent);
		NewItem(uiPrefabs[1], parent);
		for (int i = 0; i < engineFeatures_.engineFeatures_TYP.Length; i++)
		{
			if (engineFeatures_.engineFeatures_TYP[i] == engineFeatures_.GetTypGrafik())
			{
				NewItem(uiPrefabs[0], parent).GetComponent<Item_EngineFeatures_GameDesign>().myID = i;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[3], parent);
		NewItem(uiPrefabs[1], parent);
		for (int j = 0; j < engineFeatures_.engineFeatures_TYP.Length; j++)
		{
			if (engineFeatures_.engineFeatures_TYP[j] == engineFeatures_.GetTypSound())
			{
				NewItem(uiPrefabs[0], parent).GetComponent<Item_EngineFeatures_GameDesign>().myID = j;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[4], parent);
		NewItem(uiPrefabs[1], parent);
		for (int k = 0; k < engineFeatures_.engineFeatures_TYP.Length; k++)
		{
			if (engineFeatures_.engineFeatures_TYP[k] == engineFeatures_.GetTypKI())
			{
				NewItem(uiPrefabs[0], parent).GetComponent<Item_EngineFeatures_GameDesign>().myID = k;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		num2 = 0;
		NewItem(uiPrefabs[5], parent);
		NewItem(uiPrefabs[1], parent);
		for (int l = 0; l < engineFeatures_.engineFeatures_TYP.Length; l++)
		{
			if (engineFeatures_.engineFeatures_TYP[l] == engineFeatures_.GetTypPhysik())
			{
				NewItem(uiPrefabs[0], parent).GetComponent<Item_EngineFeatures_GameDesign>().myID = l;
				num2++;
				if (num2 > num)
				{
					num2 = 0;
				}
			}
		}
		NewItems(uiPrefabs[1], parent, num2);
		NewItems(uiPrefabs[1], parent, num + 1);
	}

	private GameObject NewItem(GameObject newGO, Transform parent)
	{
		return Object.Instantiate(newGO, new Vector3(0f, 0f, 0f), Quaternion.identity, parent);
	}

	private void NewItems(GameObject newGO, Transform parent, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Object.Instantiate(newGO, new Vector3(0f, 0f, 0f), Quaternion.identity, parent);
		}
	}
}
