using Suimono.Core;
using UnityEngine;
using UnityEngine.UI;

public class ui_suimonoHandler : MonoBehaviour
{
	public float uiScale = 1f;

	private Transform lightObject;

	private SuimonoModule suimonoModule;

	private SuimonoObject suimonoObject;

	private CanvasScaler uiCanvasScale;

	private Text textVersion;

	private Slider sliderTOD;

	private Slider sliderBeaufort;

	private void Start()
	{
		lightObject = GameObject.Find("Directional Light").GetComponent<Transform>();
		suimonoModule = GameObject.Find("SUIMONO_Module").GetComponent<SuimonoModule>();
		suimonoObject = GameObject.Find("SUIMONO_Surface").GetComponent<SuimonoObject>();
		uiCanvasScale = base.transform.GetComponent<CanvasScaler>();
		textVersion = GameObject.Find("Text_version").GetComponent<Text>();
		sliderTOD = GameObject.Find("Slider_TOD").GetComponent<Slider>();
		sliderBeaufort = GameObject.Find("Slider_Beaufort").GetComponent<Slider>();
	}

	private void LateUpdate()
	{
		if (uiCanvasScale != null)
		{
			uiCanvasScale.scaleFactor = uiScale;
		}
		if (lightObject != null && sliderTOD != null)
		{
			lightObject.localEulerAngles = new Vector3(Mathf.Lerp(-15f, 90f, sliderTOD.value), lightObject.localEulerAngles.y, lightObject.localEulerAngles.z);
		}
		if (suimonoModule != null)
		{
			textVersion.text = "Version " + suimonoModule.suimonoVersionNumber;
		}
		if (suimonoObject != null)
		{
			suimonoObject.beaufortScale = sliderBeaufort.value;
		}
	}
}
