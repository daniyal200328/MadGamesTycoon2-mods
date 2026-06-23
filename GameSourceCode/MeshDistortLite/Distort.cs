using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshDistortLite;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Distort : MonoBehaviour
{
	public enum Type
	{
		Stretch
	}

	public enum Calculate
	{
		global,
		local
	}

	public Calculate calculation;

	[HideInInspector]
	public bool updateIntEditor = true;

	public List<DistortData> distort = new List<DistortData>();

	[NonSerialized]
	public List<MeshDistortData> meshList;

	public Bounds combinedBounds;

	public bool showDebugLines;

	public float debugLinesDistance = 1f;

	public Vector3[,] debugLines;

	public bool showMeshInEditor = true;

	public bool showPreviewWindow = true;

	public bool calculateInGPU;

	public ComputeShader distortShader;

	protected int dirtortKernel;

	private bool hasSkinnedMesh;

	private AnimatedDistort animDistort;

	private void Awake()
	{
		animDistort = GetComponent<AnimatedDistort>();
	}

	private void Reset()
	{
	}

	private void OnEnable()
	{
		if (meshList == null)
		{
			SetVertices();
		}
		if (Application.isPlaying)
		{
			foreach (MeshDistortData mesh in meshList)
			{
				if (mesh.skin != null)
				{
					mesh.skin.enabled = false;
					hasSkinnedMesh = true;
				}
			}
		}
		UpdateDistort();
	}

	public UnityEngine.Object[] GetAllMeshes()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (MeshDistortData mesh in meshList)
		{
			list.Add(mesh.mesh);
		}
		return list.ToArray();
	}

	private void SetDebugLines()
	{
		int num = 10;
		debugLines = new Vector3[12, num];
		float num2 = 0.2f;
		Bounds bounds = combinedBounds;
		Vector3 extents = bounds.extents;
		if (extents.x < num2)
		{
			extents.x = num2;
		}
		if (extents.y < num2)
		{
			extents.y = num2;
		}
		if (extents.z < num2)
		{
			extents.z = num2;
		}
		bounds.extents = extents;
		extents *= debugLinesDistance;
		for (int i = 0; i < num; i++)
		{
			float num3 = bounds.max.x - bounds.min.x;
			float num4 = bounds.max.y - bounds.min.y;
			float num5 = bounds.max.z - bounds.min.z;
			float x = bounds.min.x + num3 * ((float)i / (float)num);
			float y = bounds.min.y + num4 * ((float)i / (float)num);
			float z = bounds.min.z + num5 * ((float)i / (float)num);
			debugLines[0, i] = new Vector3(x, bounds.center.y + extents.y, bounds.center.z);
			debugLines[1, i] = new Vector3(x, bounds.center.y - extents.y, bounds.center.z);
			debugLines[2, i] = new Vector3(x, bounds.center.y, bounds.center.z + extents.z);
			debugLines[3, i] = new Vector3(x, bounds.center.y, bounds.center.z - extents.z);
			debugLines[4, i] = new Vector3(bounds.center.x + extents.x, y, bounds.center.z);
			debugLines[5, i] = new Vector3(bounds.center.x - extents.x, y, bounds.center.z);
			debugLines[6, i] = new Vector3(bounds.center.x, y, bounds.center.z + extents.z);
			debugLines[7, i] = new Vector3(bounds.center.x, y, bounds.center.z - extents.z);
			debugLines[8, i] = new Vector3(bounds.center.x + extents.x, bounds.center.y, z);
			debugLines[9, i] = new Vector3(bounds.center.x - extents.x, bounds.center.y, z);
			debugLines[10, i] = new Vector3(bounds.center.x, bounds.center.y + extents.y, z);
			debugLines[11, i] = new Vector3(bounds.center.x, bounds.center.y - extents.y, z);
		}
	}

	private void SetVertices()
	{
		meshList = new List<MeshDistortData>();
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			MeshDistortData item = new MeshDistortData(meshFilter.transform, meshFilter.GetComponent<Renderer>().sharedMaterial, meshFilter);
			meshList.Add(item);
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren2)
		{
			MeshDistortData item2 = new MeshDistortData(skinnedMeshRenderer.transform, skinnedMeshRenderer.sharedMaterial, skinnedMeshRenderer);
			meshList.Add(item2);
		}
		SetBounds();
	}

	private void SetBounds()
	{
		combinedBounds = default(Bounds);
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Bounds bounds = componentsInChildren[i].GetComponent<Renderer>().bounds;
			if (combinedBounds.size.magnitude == 0f)
			{
				combinedBounds = bounds;
			}
			else
			{
				combinedBounds.Encapsulate(bounds);
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			Bounds bounds2 = componentsInChildren2[i].bounds;
			if (combinedBounds.size.magnitude == 0f)
			{
				combinedBounds = bounds2;
			}
			else
			{
				combinedBounds.Encapsulate(bounds2);
			}
		}
	}

	private void ResetVertices()
	{
		foreach (MeshDistortData mesh in meshList)
		{
			mesh.ResetMesh();
		}
	}

	public void EditParameters()
	{
	}

	public void LateUpdate()
	{
		if (base.transform.hasChanged)
		{
			UpdateDistort();
			base.transform.hasChanged = false;
		}
		else if (hasSkinnedMesh && (animDistort == null || !animDistort.enabled || !animDistort.isPlaying))
		{
			UpdateDistort();
		}
	}

	public void UpdateDistort()
	{
		if (base.isActiveAndEnabled)
		{
			UpdateInCPU();
		}
	}

	public void UpdateInCPU()
	{
		Vector3 position = base.transform.position;
		Vector3 localScale = base.transform.localScale;
		Quaternion rotation = base.transform.rotation;
		if (calculation == Calculate.local)
		{
			base.transform.position = Vector3.zero;
			base.transform.localScale = Vector3.one;
			base.transform.rotation = Quaternion.identity;
		}
		SetBounds();
		_ = GetComponent<Animator>() != null;
		foreach (MeshDistortData mesh in meshList)
		{
			if (mesh.meshTransform == null)
			{
				continue;
			}
			if (mesh.mesh == null)
			{
				mesh.UpdateMesh();
			}
			Vector3[] array = mesh.skinVertices.Clone() as Vector3[];
			int num = array.Length;
			Matrix4x4 localToWorldMatrix = mesh.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = mesh.worldToLocalMatrix;
			for (int i = 0; i < num; i++)
			{
				array[i] = localToWorldMatrix.MultiplyPoint3x4(array[i]);
			}
			if (base.enabled)
			{
				foreach (DistortData item in distort)
				{
					if (item.enabled)
					{
						item.SetBounds(combinedBounds);
						for (int j = 0; j < num; j++)
						{
							item.DistortVertice(ref array[j]);
						}
					}
				}
			}
			for (int k = 0; k < num; k++)
			{
				array[k] = worldToLocalMatrix.MultiplyPoint3x4(array[k]);
			}
			mesh.mesh.vertices = array;
			mesh.mesh.RecalculateNormals();
			if (Application.isPlaying && mesh.skin != null && calculation == Calculate.global)
			{
				Matrix4x4 matrix = Matrix4x4.TRS(mesh.meshTransform.position, mesh.meshTransform.rotation, Vector3.one);
				Graphics.DrawMesh(mesh.mesh, matrix, mesh.originalMaterial, 0);
			}
		}
		if (calculation != Calculate.local)
		{
			return;
		}
		base.transform.position = position;
		base.transform.localScale = localScale;
		base.transform.rotation = rotation;
		foreach (MeshDistortData mesh2 in meshList)
		{
			if (Application.isPlaying && mesh2.skin != null)
			{
				Matrix4x4 matrix2 = Matrix4x4.TRS(mesh2.meshTransform.position, mesh2.meshTransform.rotation, mesh2.meshTransform.lossyScale);
				Graphics.DrawMesh(mesh2.mesh, matrix2, mesh2.originalMaterial, 0);
			}
		}
	}

	public void UpdateDebugLines()
	{
		SetDebugLines();
		for (int i = 0; i < debugLines.GetLength(0); i++)
		{
			for (int j = 0; j < debugLines.GetLength(1); j++)
			{
				Vector3 vertice = debugLines[i, j];
				for (int k = 0; k < distort.Count; k++)
				{
					if (distort[k].enabled)
					{
						distort[k].DistortVertice(ref vertice);
					}
				}
				debugLines[i, j] = vertice;
			}
		}
	}

	public void MakeDynamic()
	{
		foreach (MeshDistortData mesh in meshList)
		{
			mesh.mesh.MarkDynamic();
		}
	}

	public void AddDistortion()
	{
		DistortData item = new DistortData();
		if (distort == null)
		{
			distort = new List<DistortData>();
		}
		distort.Add(item);
	}

	public void RemoveDistort(int index)
	{
		distort.RemoveAt(index);
	}

	private void OnDrawGizmos()
	{
		if (showMeshInEditor && !Application.isPlaying && base.isActiveAndEnabled)
		{
			foreach (MeshDistortData mesh in meshList)
			{
				if (mesh.meshTransform != null)
				{
					Gizmos.color = Color.magenta;
					Gizmos.DrawMesh(mesh.mesh, mesh.meshTransform.position, mesh.meshTransform.rotation, mesh.meshTransform.lossyScale);
				}
			}
		}
		if (!showDebugLines)
		{
			return;
		}
		if (debugLines == null)
		{
			SetDebugLines();
		}
		_ = Vector3.zero;
		for (int i = 0; i < debugLines.GetLength(0); i++)
		{
			if (i <= 3)
			{
				Gizmos.color = Color.red;
			}
			else if (i <= 7)
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.blue;
			}
			for (int j = 1; j < debugLines.GetLength(1); j++)
			{
				Gizmos.DrawLine(debugLines[i, j - 1], debugLines[i, j]);
			}
		}
	}
}
