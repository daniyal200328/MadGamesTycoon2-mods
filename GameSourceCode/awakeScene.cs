using UnityEngine;
using UnityEngine.SceneManagement;

public class awakeScene : MonoBehaviour
{
	private void Start()
	{
		Screen.SetResolution(1024, 768, fullscreen: false);
		SceneManager.LoadScene("splash");
	}

	private void Update()
	{
	}
}
