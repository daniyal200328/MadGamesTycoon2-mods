using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrainFailProductions.PolyFewRuntime;

public class PolygonReduction : MonoBehaviour
{
	public Slider reductionStrength;

	public Slider preservationStrength;

	public Toggle preserveUVFoldover;

	public Toggle preserveUVSeams;

	public Toggle preserveBorders;

	public Toggle enableSmartLinking;

	public Toggle preserveFace;

	public Toggle recalculateNormals;

	public Toggle regardCurvature;

	public InputField trianglesCount;

	public Text message;

	public Text progress;

	public Button exportButton;

	public Button importFromFileSystem;

	public Button importFromWeb;

	public Slider progressSlider;

	public GameObject uninteractivePanel;

	public GameObject targetObject;

	public Transform preservationSphere;

	public EventSystem eventSystem;

	private PolyfewRuntime.ObjectMeshPairs objectMeshPairs;

	private bool didApplyLosslessLast;

	private bool disableTemporary;

	private GameObject barabarianRef;

	private PolyfewRuntime.ReferencedNumeric<float> downloadProgress = new PolyfewRuntime.ReferencedNumeric<float>(0f);

	private bool isImportingFromNetwork;

	private bool isWebGL;

	private void Start()
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			isWebGL = true;
		}
		uninteractivePanel.SetActive(value: false);
		exportButton.interactable = false;
		barabarianRef = targetObject;
		objectMeshPairs = PolyfewRuntime.GetObjectMeshPairs(targetObject, includeInactive: true);
		trianglesCount.text = PolyfewRuntime.CountTriangles(countDeep: true, targetObject).ToString() ?? "";
	}

	private void Update()
	{
		if ((bool)eventSystem)
		{
			if ((bool)eventSystem.currentSelectedGameObject && (bool)eventSystem.currentSelectedGameObject.GetComponent<RectTransform>())
			{
				FlyCamera.deactivated = true;
			}
			else
			{
				FlyCamera.deactivated = false;
			}
			if (isWebGL)
			{
				exportButton.gameObject.SetActive(value: false);
				importFromFileSystem.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnReductionChange(float value)
	{
		if (disableTemporary)
		{
			return;
		}
		didApplyLosslessLast = false;
		if (targetObject == null)
		{
			return;
		}
		if (Mathf.Approximately(0f, value))
		{
			AssignMeshesFromPairs();
			trianglesCount.text = PolyfewRuntime.CountTriangles(countDeep: true, targetObject).ToString() ?? "";
			return;
		}
		PolyfewRuntime.SimplificationOptions simplificationOptions = new PolyfewRuntime.SimplificationOptions();
		simplificationOptions.simplificationStrength = value;
		simplificationOptions.enableSmartlinking = enableSmartLinking.isOn;
		simplificationOptions.preserveBorderEdges = preserveBorders.isOn;
		simplificationOptions.preserveUVSeamEdges = preserveUVSeams.isOn;
		simplificationOptions.preserveUVFoldoverEdges = preserveUVFoldover.isOn;
		simplificationOptions.recalculateNormals = recalculateNormals.isOn;
		simplificationOptions.regardCurvature = regardCurvature.isOn;
		if (preserveFace.isOn)
		{
			simplificationOptions.regardPreservationSpheres = true;
			simplificationOptions.preservationSpheres.Add(new PolyfewRuntime.PreservationSphere(preservationSphere.position, preservationSphere.lossyScale.x, preservationStrength.value));
		}
		else
		{
			simplificationOptions.regardPreservationSpheres = false;
		}
		trianglesCount.text = PolyfewRuntime.SimplifyObjectDeep(objectMeshPairs, simplificationOptions, delegate
		{
		}).ToString() ?? "";
	}

	public void SimplifyLossless()
	{
		disableTemporary = true;
		reductionStrength.value = 0f;
		disableTemporary = false;
		didApplyLosslessLast = true;
		PolyfewRuntime.SimplificationOptions simplificationOptions = new PolyfewRuntime.SimplificationOptions
		{
			enableSmartlinking = enableSmartLinking.isOn,
			preserveBorderEdges = preserveBorders.isOn,
			preserveUVSeamEdges = preserveUVSeams.isOn,
			preserveUVFoldoverEdges = preserveUVFoldover.isOn,
			recalculateNormals = recalculateNormals.isOn,
			regardCurvature = regardCurvature.isOn,
			simplifyMeshLossless = true
		};
		if (preserveFace.isOn)
		{
			simplificationOptions.regardPreservationSpheres = true;
		}
		else
		{
			simplificationOptions.regardPreservationSpheres = false;
		}
		trianglesCount.text = PolyfewRuntime.SimplifyObjectDeep(objectMeshPairs, simplificationOptions, delegate
		{
		}).ToString() ?? "";
	}

	public void ImportOBJ()
	{
		PolyfewRuntime.OBJImportOptions oBJImportOptions = new PolyfewRuntime.OBJImportOptions();
		oBJImportOptions.zUp = false;
		oBJImportOptions.localPosition = new Vector3(-2.199f, -1f, -1.7349f);
		oBJImportOptions.localScale = new Vector3(0.045f, 0.045f, 0.045f);
		string objAbsolutePath = Application.dataPath + "/PolyFew/demo/TestModels/Meat.obj";
		string texturesFolderPath = Application.dataPath + "/PolyFew/demo/TestModels/textures";
		string materialsFolderPath = Application.dataPath + "/PolyFew/demo/TestModels/materials";
		GameObject importedObject;
		PolyfewRuntime.ImportOBJFromFileSystem(objAbsolutePath, texturesFolderPath, materialsFolderPath, delegate(GameObject imp)
		{
			importedObject = imp;
			Debug.Log("Successfully imported GameObject:   " + importedObject.name);
			barabarianRef.SetActive(value: false);
			targetObject = importedObject;
			ResetSettings();
			objectMeshPairs = PolyfewRuntime.GetObjectMeshPairs(targetObject, includeInactive: true);
			trianglesCount.text = PolyfewRuntime.CountTriangles(countDeep: true, targetObject).ToString() ?? "";
			exportButton.interactable = true;
			importFromWeb.interactable = false;
			importFromFileSystem.interactable = false;
			preserveFace.interactable = false;
			preservationStrength.interactable = false;
			disableTemporary = true;
			preservationSphere.gameObject.SetActive(value: false);
			disableTemporary = false;
		}, delegate(Exception ex)
		{
			Debug.LogError("Failed to load OBJ file.   " + ex.ToString());
		}, oBJImportOptions);
	}

	public void ImportOBJFromNetwork()
	{
		isImportingFromNetwork = true;
		PolyfewRuntime.OBJImportOptions oBJImportOptions = new PolyfewRuntime.OBJImportOptions();
		oBJImportOptions.zUp = false;
		oBJImportOptions.localPosition = new Vector3(0.87815f, 1.4417f, -4.4708f);
		oBJImportOptions.localScale = new Vector3(0.0042f, 0.0042f, 0.0042f);
		string objName = "onion";
		string diffuseTexURL = "https://dl.dropbox.com/s/0u4ij6sddi7a3gc/onion.jpg?dl=1";
		string bumpTexURL = "";
		string specularTexURL = "";
		string opacityTexURL = "";
		string materialURL = "https://dl.dropbox.com/s/fuzryqigs4gxwvv/onion.mtl?dl=1";
		progressSlider.value = 0f;
		uninteractivePanel.SetActive(value: true);
		downloadProgress = new PolyfewRuntime.ReferencedNumeric<float>(0f);
		StartCoroutine(UpdateProgress());
		GameObject importedObject;
		PolyfewRuntime.ImportOBJFromNetwork("https://dl.dropbox.com/s/v09bh0hiivja10e/onion.obj?dl=1", objName, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, downloadProgress, delegate(GameObject imp)
		{
			AssignMeshesFromPairs();
			isImportingFromNetwork = false;
			importedObject = imp;
			barabarianRef.SetActive(value: false);
			targetObject = importedObject;
			ResetSettings();
			objectMeshPairs = PolyfewRuntime.GetObjectMeshPairs(targetObject, includeInactive: true);
			trianglesCount.text = PolyfewRuntime.CountTriangles(countDeep: true, targetObject).ToString() ?? "";
			exportButton.interactable = true;
			uninteractivePanel.SetActive(value: false);
			importFromWeb.interactable = false;
			importFromFileSystem.interactable = false;
			preserveFace.interactable = false;
			preservationStrength.interactable = false;
			disableTemporary = true;
			preservationSphere.gameObject.SetActive(value: false);
			disableTemporary = false;
		}, delegate(Exception ex)
		{
			uninteractivePanel.SetActive(value: false);
			isImportingFromNetwork = false;
			Debug.LogError("Failed to download and import OBJ file.   " + ex.Message);
		}, oBJImportOptions);
	}

	public void ExportGameObjectToOBJ()
	{
		string persistentDataPath = Application.persistentDataPath;
		GameObject exportObject = GameObject.Find("onion");
		if ((bool)exportObject)
		{
			exportObject = exportObject.transform.GetChild(0).GetChild(0).gameObject;
		}
		else
		{
			exportObject = GameObject.Find("Meat");
			if (!exportObject)
			{
				return;
			}
			exportObject = exportObject.transform.GetChild(0).GetChild(0).gameObject;
		}
		PolyfewRuntime.OBJExportOptions exportOptions = new PolyfewRuntime.OBJExportOptions(applyPosition: true, applyRotation: true, applyScale: true, generateMaterials: true, exportTextures: true);
		PolyfewRuntime.ExportGameObjectToOBJ(exportObject, persistentDataPath, delegate
		{
			Debug.Log("Successfully exported GameObject:  " + exportObject.name);
			string text = "Successfully exported the file to:  \n" + Application.persistentDataPath;
			StartCoroutine(ShowMessage(text));
		}, delegate(Exception ex)
		{
			Debug.LogError("Failed to export OBJ. " + ex.ToString());
		}, exportOptions);
	}

	public void OnToggleStateChanged(bool isOn)
	{
		if (!disableTemporary)
		{
			preservationSphere.gameObject.SetActive(preserveFace.isOn);
			if (didApplyLosslessLast)
			{
				SimplifyLossless();
				return;
			}
			preservationStrength.interactable = preserveFace.isOn;
			OnReductionChange(reductionStrength.value);
		}
	}

	public void OnPreservationStrengthChange(float value)
	{
		OnToggleStateChanged(isOn: true);
	}

	public void Reset()
	{
		ResetSettings();
		AssignMeshesFromPairs();
		if ((bool)GameObject.Find("onion"))
		{
			targetObject.SetActive(value: false);
		}
		else if ((bool)GameObject.Find("Meat"))
		{
			targetObject.SetActive(value: false);
		}
		targetObject = barabarianRef;
		preserveFace.interactable = true;
		preservationStrength.interactable = preserveFace.isOn;
		targetObject.SetActive(value: true);
		objectMeshPairs = PolyfewRuntime.GetObjectMeshPairs(targetObject, includeInactive: true);
		trianglesCount.text = PolyfewRuntime.CountTriangles(countDeep: true, targetObject).ToString() ?? "";
		exportButton.interactable = false;
		importFromWeb.interactable = true;
		importFromFileSystem.interactable = true;
	}

	public static void OnSliderSelect()
	{
		FlyCamera.deactivated = true;
	}

	public static void OnSliderDeselect()
	{
		FlyCamera.deactivated = false;
	}

	private bool IsMouseOverUI(RectTransform uiElement)
	{
		Vector2 point = uiElement.InverseTransformPoint(Input.mousePosition);
		if (uiElement.rect.Contains(point))
		{
			return true;
		}
		return false;
	}

	private IEnumerator ShowMessage(string message)
	{
		Debug.Log(message);
		this.message.text = message;
		yield return new WaitForSeconds(4.5f);
		this.message.text = "";
	}

	private void ResetSettings()
	{
		disableTemporary = true;
		reductionStrength.value = 0f;
		preserveUVSeams.isOn = false;
		preserveUVFoldover.isOn = false;
		preserveBorders.isOn = false;
		enableSmartLinking.isOn = true;
		preserveFace.isOn = false;
		preservationSphere.gameObject.SetActive(value: false);
		disableTemporary = false;
		preservationStrength.value = 100f;
	}

	private IEnumerator UpdateProgress()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);
			progressSlider.value = downloadProgress.Value;
			progress.text = (int)downloadProgress.Value + "%";
		}
	}

	private void AssignMeshesFromPairs()
	{
		if (objectMeshPairs == null)
		{
			return;
		}
		foreach (GameObject key in objectMeshPairs.Keys)
		{
			if (!(key != null))
			{
				continue;
			}
			PolyfewRuntime.MeshRendererPair meshRendererPair = objectMeshPairs[key];
			if (meshRendererPair.mesh == null)
			{
				continue;
			}
			if (meshRendererPair.attachedToMeshFilter)
			{
				MeshFilter component = key.GetComponent<MeshFilter>();
				if (!(component == null))
				{
					component.sharedMesh = meshRendererPair.mesh;
				}
			}
			else if (!meshRendererPair.attachedToMeshFilter)
			{
				SkinnedMeshRenderer component2 = key.GetComponent<SkinnedMeshRenderer>();
				if (!(component2 == null))
				{
					component2.sharedMesh = meshRendererPair.mesh;
				}
			}
		}
	}
}
