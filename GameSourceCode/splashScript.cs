using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashScript : MonoBehaviour
{
	private bool sceneIsLoading;

	private float splashTimer;

	private void Start()
	{
		LoadSettings();
	}

	private void Update()
	{
		if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
		{
			splashTimer = 1f;
		}
		splashTimer += Time.deltaTime;
		if (!(splashTimer < 1f) && !sceneIsLoading)
		{
			sceneIsLoading = true;
			StartCoroutine(LoadYourAsyncScene());
		}
	}

	private void LoadSettings()
	{
		PlayerPrefs.SetInt("LoadSavegame", -1);
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene("scene01");
	}

	private IEnumerator LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("scene01");
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
