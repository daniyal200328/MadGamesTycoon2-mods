using UnityEngine;

public class carScript : MonoBehaviour
{
	public GameObject[] prefabCars;

	public AnimationClip carAnimation;

	private GameObject aktuellesCar;

	private void Start()
	{
	}

	private void Update()
	{
		if (!aktuellesCar)
		{
			aktuellesCar = Object.Instantiate(prefabCars[Random.Range(0, prefabCars.Length)]);
			aktuellesCar.GetComponent<Animation>().AddClip(carAnimation, "myAnim");
			aktuellesCar.GetComponent<Animation>().Play("myAnim");
		}
		else if (!aktuellesCar.GetComponent<Animation>().isPlaying)
		{
			Object.Destroy(aktuellesCar);
		}
	}
}
