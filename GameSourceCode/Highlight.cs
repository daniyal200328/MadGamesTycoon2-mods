using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Highlight : MonoBehaviour
{
	public int lineWidth = 5;

	public int energyLineWidth = 4;

	public float selectionSize = 0.5f;

	public float force = 20f;

	public int pointsInEnergyLine = 100;

	private VectorLine line;

	private VectorLine energyLine;

	private RaycastHit hit;

	private int selectIndex;

	private float energyLevel;

	private bool canClick;

	private GameObject[] spheres;

	private double timer;

	private int ignoreLayer;

	private int defaultLayer;

	private bool fading;

	private void Start()
	{
		Time.fixedDeltaTime = 0.01f;
		spheres = new GameObject[GetComponent<MakeSpheres>().numberOfSpheres];
		ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
		defaultLayer = LayerMask.NameToLayer("Default");
		line = new VectorLine("Line", new List<Vector2>(), lineWidth);
		line.color = Color.green;
		line.capLength = (float)lineWidth * 0.5f;
		energyLine = new VectorLine("Energy", new List<Vector2>(pointsInEnergyLine), null, energyLineWidth, LineType.Continuous);
		SetEnergyLinePoints();
	}

	private void SetEnergyLinePoints()
	{
		for (int i = 0; i < energyLine.points2.Count; i++)
		{
			float x = Mathf.Lerp(70f, Screen.width - 20, ((float)i + 0f) / (float)energyLine.points2.Count);
			energyLine.points2[i] = new Vector2(x, (float)Screen.height * 0.1f);
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 50f && !fading)
		{
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) && selectIndex > 0)
			{
				ResetSelection(instantFade: true);
			}
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				spheres[selectIndex] = hit.collider.gameObject;
				spheres[selectIndex].layer = ignoreLayer;
				spheres[selectIndex].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
				selectIndex++;
				line.Resize(selectIndex * 10);
			}
		}
		for (int i = 0; i < selectIndex; i++)
		{
			float num = (float)Screen.height * selectionSize / Camera.main.transform.InverseTransformPoint(spheres[i].transform.position).z;
			Vector3 vector = Camera.main.WorldToScreenPoint(spheres[i].transform.position);
			Rect rect = new Rect(vector.x - num, vector.y - num, num * 2f, num * 2f);
			line.MakeRect(rect, i * 10);
			line.points2[i * 10 + 8] = new Vector2(rect.x - (float)lineWidth * 0.25f, rect.y + num);
			line.points2[i * 10 + 9] = new Vector2(35f, Mathf.Lerp(65f, Screen.height - 25, energyLevel));
			spheres[i].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(energyLevel, energyLevel, energyLevel));
		}
	}

	private void FixedUpdate()
	{
		int i;
		for (i = 0; i < energyLine.points2.Count - 1; i++)
		{
			energyLine.points2[i] = new Vector2(energyLine.points2[i].x, energyLine.points2[i + 1].y);
		}
		timer += Time.deltaTime * Mathf.Lerp(5f, 20f, energyLevel);
		energyLine.points2[i] = new Vector2(energyLine.points2[i].x, (float)Screen.height * (0.1f + Mathf.Sin((float)timer) * 0.08f * energyLevel));
	}

	private void LateUpdate()
	{
		line.Draw();
		energyLine.Draw();
	}

	private void ResetSelection(bool instantFade)
	{
		if (energyLevel > 0f)
		{
			StartCoroutine(FadeColor(instantFade));
		}
		selectIndex = 0;
		energyLevel = 0f;
		line.points2.Clear();
		line.Draw();
		GameObject[] array = spheres;
		foreach (GameObject gameObject in array)
		{
			if ((bool)gameObject)
			{
				gameObject.layer = defaultLayer;
			}
		}
	}

	private IEnumerator FadeColor(bool instantFade)
	{
		if (instantFade)
		{
			for (int i = 0; i < selectIndex; i++)
			{
				spheres[i].GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
			}
			yield break;
		}
		fading = true;
		Color startColor = new Color(energyLevel, energyLevel, energyLevel, 0f);
		int thisIndex = selectIndex;
		for (float t = 0f; t < 1f; t += Time.deltaTime)
		{
			for (int j = 0; j < thisIndex; j++)
			{
				spheres[j].GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(startColor, Color.black, t));
			}
			yield return null;
		}
		fading = false;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(60f, 20f, 600f, 40f), "Click to select sphere, shift-click to select multiple spheres\nThen change energy level slider and click Go");
		energyLevel = GUI.VerticalSlider(new Rect(30f, 20f, 10f, Screen.height - 80), energyLevel, 1f, 0f);
		if (selectIndex == 0)
		{
			energyLevel = 0f;
		}
		if (GUI.Button(new Rect(20f, Screen.height - 40, 32f, 20f), "Go"))
		{
			for (int i = 0; i < selectIndex; i++)
			{
				spheres[i].GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force * energyLevel, ForceMode.VelocityChange);
			}
			ResetSelection(instantFade: false);
		}
	}
}
