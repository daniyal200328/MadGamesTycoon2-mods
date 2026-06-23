using UnityEngine;

namespace MeshDistortLite;

[RequireComponent(typeof(Distort))]
public class AnimatedDistort : MonoBehaviour
{
	public enum Animate
	{
		force,
		displacement
	}

	public enum Type
	{
		constant,
		pingpong,
		sin,
		curve
	}

	public float animationFramesPerSec = 30f;

	public int animationFrames = 1;

	protected Distort distort;

	public Type type;

	public Animate animate = Animate.displacement;

	public AnimationCurve curveType;

	public float constantSpeed = 1f;

	public float minValue;

	public float maxValue = 10f;

	public int playAnimationIndex;

	public bool updatingAnimation;

	public bool isPlaying = true;

	private float playingAnimationTime;

	public int currentAnimation
	{
		get
		{
			return playAnimationIndex - 1;
		}
		set
		{
			playAnimationIndex = value + 1;
		}
	}

	private void Start()
	{
		Setup();
	}

	public void Play()
	{
		isPlaying = true;
		playingAnimationTime = 0f;
	}

	public void Stop()
	{
		isPlaying = false;
		playingAnimationTime = 0f;
	}

	private void LateUpdate()
	{
		if (isPlaying && !updatingAnimation && playAnimationIndex == 0)
		{
			Animation(playingAnimationTime, Time.deltaTime);
			playingAnimationTime += Time.deltaTime;
		}
	}

	private void Setup()
	{
		distort = GetComponent<Distort>();
		distort.MakeDynamic();
	}

	public void CalculateInRealTime()
	{
		playAnimationIndex = 0;
	}

	private void Animation(float displaceOffset, float delta)
	{
		if (distort == null)
		{
			Setup();
		}
		for (int i = 0; i < distort.distort.Count; i++)
		{
			if (type == Type.constant)
			{
				if (animate == Animate.displacement)
				{
					float num = constantSpeed * delta * distort.distort[i].animationSpeed;
					distort.distort[i].movementDisplacement += num;
				}
				else
				{
					distort.distort[i].force += constantSpeed * delta * distort.distort[i].animationSpeed;
				}
			}
			else if (type == Type.pingpong)
			{
				if (animate == Animate.displacement)
				{
					float movementDisplacement = Mathf.Lerp(minValue, maxValue, Mathf.PingPong(displaceOffset * constantSpeed * distort.distort[i].animationSpeed, 1f));
					distort.distort[i].movementDisplacement = movementDisplacement;
				}
				else
				{
					distort.distort[i].force = Mathf.Lerp(minValue, maxValue, Mathf.PingPong(displaceOffset * constantSpeed * distort.distort[i].animationSpeed, 1f));
				}
			}
			else if (type == Type.sin)
			{
				if (animate == Animate.displacement)
				{
					float movementDisplacement2 = Mathf.Lerp(minValue, maxValue, (Mathf.Sin(displaceOffset * constantSpeed * distort.distort[i].animationSpeed) + 1f) * 0.5f);
					distort.distort[i].movementDisplacement = movementDisplacement2;
				}
				else
				{
					distort.distort[i].force = Mathf.Lerp(minValue, maxValue, (Mathf.Sin(displaceOffset * constantSpeed * distort.distort[i].animationSpeed) + 1f) * 0.5f);
				}
			}
			else if (type == Type.curve)
			{
				if (animate == Animate.displacement)
				{
					float movementDisplacement3 = curveType.Evaluate(displaceOffset * constantSpeed * distort.distort[i].animationSpeed);
					distort.distort[i].movementDisplacement = movementDisplacement3;
				}
				else
				{
					distort.distort[i].force = curveType.Evaluate(displaceOffset * constantSpeed * distort.distort[i].animationSpeed);
				}
			}
		}
		distort.UpdateDistort();
	}
}
