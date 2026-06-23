using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ReachableGames.PostLinerPro;

[Serializable]
[PostProcess(typeof(PostLinerEffect), PostProcessEvent.BeforeTransparent, "ReachableGames/Post Liner Pro", true)]
public sealed class PostLinerPro : PostProcessEffectSettings
{
	[Tooltip("RGB controls the color of visible pixels.")]
	public ColorParameter fillColor = new ColorParameter
	{
		value = Color.white
	};

	[Tooltip("RGB controls the color of obscured pixels.")]
	public ColorParameter fillColorHidden = new ColorParameter
	{
		value = Color.black
	};

	[Range(0f, 1f)]
	[Tooltip("At 0, only edges are drawn, at 1 the whole object is brightly tinted.")]
	public FloatParameter fillBlend = new FloatParameter
	{
		value = 0.177f
	};

	[Range(0f, 1f)]
	[Tooltip("At 0, obscured pixels are not modified, at 1 all obscured pixels are fillColorHidden.")]
	public FloatParameter fillBlendHidden = new FloatParameter
	{
		value = 0.166f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter fillDepthFading = new FloatParameter
	{
		value = 0.025f
	};

	[Space]
	[Tooltip("RGB controls the color of the outline.")]
	public ColorParameter outlineColor = new ColorParameter
	{
		value = Color.yellow
	};

	[Tooltip("RGB controls the color of the hidden outline.")]
	public ColorParameter outlineColorHidden = new ColorParameter
	{
		value = Color.white
	};

	[Range(0f, 10f)]
	[Tooltip("Larger values makes for thicker outlines.")]
	public FloatParameter lineThickness = new FloatParameter
	{
		value = 0.58f
	};

	[Range(0f, 1f)]
	[Tooltip("With interpenetrating objects, this controls how much of the wrong object will have outlines too.  Depth map precision issue.")]
	public FloatParameter errorTolerance = new FloatParameter
	{
		value = 0.231f
	};

	[Space]
	[Range(1E-05f, 1f)]
	[Tooltip("Sensitivity to changes in depth.")]
	public FloatParameter topologySensitivity = new FloatParameter
	{
		value = 0.0001f
	};

	[Range(0f, 1f)]
	[Tooltip("Blend control for depth-based outlines.")]
	public FloatParameter topologyBlend = new FloatParameter
	{
		value = 1f
	};

	[Range(0f, 1f)]
	[Tooltip("Blend control for depth-based outlines that are obscured.")]
	public FloatParameter topologyBlendHidden = new FloatParameter
	{
		value = 0.68f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter topologyDepthFading = new FloatParameter
	{
		value = 0.005f
	};

	[Space]
	[Range(0f, 1f)]
	[Tooltip("Control the amount of hard edge lines interior to the object.")]
	public FloatParameter hardEdgeBlend = new FloatParameter
	{
		value = 0.227f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter hardEdgeDepthFading = new FloatParameter
	{
		value = 0.005f
	};

	[Space]
	[Range(0f, 1f)]
	[Tooltip("Master knob for distance over which fading out of effects happens.  Value is a relative range between near and far plane.")]
	public FloatParameter fadeDistance = new FloatParameter
	{
		value = 0.01f
	};

	[Range(0f, 1f)]
	[Tooltip("Master knob for the maximum blend amount.")]
	public FloatParameter finalBlend = new FloatParameter
	{
		value = 1f
	};
}
