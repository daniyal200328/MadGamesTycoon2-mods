using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ReachableGames;

[Serializable]
[PostProcess(typeof(PostLinerEffect), PostProcessEvent.BeforeTransparent, "ReachableGames/Post Liner", false)]
public sealed class PostLiner : PostProcessEffectSettings
{
	[Tooltip("RGB controls the color of the fill.")]
	public ColorParameter fillColor = new ColorParameter
	{
		value = Color.white
	};

	[Range(0f, 1f)]
	[Tooltip("At 0, only edges are drawn, at 1 the whole object is brightly tinted.")]
	public FloatParameter fillBlend = new FloatParameter
	{
		value = 0.122f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter fillDepthFading = new FloatParameter
	{
		value = 0.083f
	};

	[Space]
	[Tooltip("RGB controls the color of the outline.")]
	public ColorParameter outlineColor = new ColorParameter
	{
		value = Color.yellow
	};

	[Range(0f, 10f)]
	[Tooltip("Larger values makes for thicker outlines.")]
	public FloatParameter lineThickness = new FloatParameter
	{
		value = 0.78f
	};

	[Range(0f, 5f)]
	[Tooltip("With interpenetrating objects, this controls how much of the wrong object will have outlines too.  Depth map precision issue.")]
	public FloatParameter errorTolerance = new FloatParameter
	{
		value = 0.03f
	};

	[Space]
	[Range(1E-05f, 0.001f)]
	[Tooltip("Sensitivity to changes in depth.")]
	public FloatParameter topologySensitivity = new FloatParameter
	{
		value = 0.00027f
	};

	[Range(0f, 1f)]
	[Tooltip("Blend control for depth-based outlines.")]
	public FloatParameter topologyBlend = new FloatParameter
	{
		value = 0.68f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter topologyDepthFading = new FloatParameter
	{
		value = 0.02f
	};

	[Space]
	[Range(0f, 1f)]
	[Tooltip("Control the amount of hard edge lines interior to the object.")]
	public FloatParameter hardEdgeBlend = new FloatParameter
	{
		value = 0.652f
	};

	[Range(0f, 1f)]
	[Tooltip("Controls distance where this effect fades away completely.  Value is a range between near and far plane.")]
	public FloatParameter hardEdgeDepthFading = new FloatParameter
	{
		value = 0.02f
	};

	[Space]
	[Range(0f, 1f)]
	[Tooltip("Master knob for the maximum blend amount.")]
	public FloatParameter finalBlend = new FloatParameter
	{
		value = 1f
	};
}
