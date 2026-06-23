using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
	private float time = 20f;

	private float targetChromaticIntensityUpper = 1.5f;

	private float targetChromaticIntensityLower;

	private float currentChromaticIntensity;

	private ChromaticAberration chromatic;

	private PostProcessVolume volumeChromatic;

	private float targetVignetteIntensityUpper = 0.43f;

	private float targetVignetteIntensityLower = 0.2f;

	private float targetVignetteSmoothness = 0.3f;

	private float targetVignetteRoundness = 1f;

	private bool targetVignetteRounded;

	private float currentVignetteIntensity;

	private Vignette vignette;

	private PostProcessVolume volumeVignette;

	private float blend = 1f;

	private void Start()
	{
		chromatic = ScriptableObject.CreateInstance<ChromaticAberration>();
		chromatic.enabled.Override(x: true);
		chromatic.intensity.Override(targetChromaticIntensityUpper);
		volumeChromatic = PostProcessManager.instance.QuickVolume(base.gameObject.layer, 100f, chromatic);
		vignette = ScriptableObject.CreateInstance<Vignette>();
		vignette.enabled.Override(x: true);
		vignette.intensity.Override(targetVignetteIntensityUpper);
		vignette.smoothness.Override(targetVignetteSmoothness);
		vignette.roundness.Override(targetVignetteRoundness);
		vignette.rounded.Override(targetVignetteRounded);
		volumeVignette = PostProcessManager.instance.QuickVolume(base.gameObject.layer, 90f, vignette);
	}

	public void BlendIn()
	{
		blend = 1f;
		chromatic.intensity.value = 1f;
		vignette.intensity.value = 1f;
	}

	private void Update()
	{
		blend -= Time.deltaTime * 1f;
		chromatic.intensity.value = blend;
		vignette.intensity.value = blend;
	}

	private void OnDestroy()
	{
		RuntimeUtilities.DestroyVolume(volumeChromatic, destroyProfile: true, destroyGameObject: true);
		RuntimeUtilities.DestroyVolume(volumeVignette, destroyProfile: true, destroyGameObject: true);
	}
}
