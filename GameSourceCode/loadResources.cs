using System.IO;
using UnityEngine;

public class loadResources : MonoBehaviour
{
	private mainScript mS_;

	private GUI_Main guiMain_;

	private void Start()
	{
		FindScripts();
		LoadLogos();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void LoadLogos()
	{
		string text = Application.dataPath + "/Extern/CompanyLogos/";
		FileInfo[] files = new DirectoryInfo(text).GetFiles("*.png");
		guiMain_.logoSprites = new Sprite[files.Length];
		for (int i = 0; i < files.Length; i++)
		{
			string filePath = text + i + ".png";
			guiMain_.logoSprites[i] = mS_.LoadPNG(filePath);
		}
	}
}
