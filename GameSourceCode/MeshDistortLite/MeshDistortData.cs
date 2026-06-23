using System;
using UnityEngine;

namespace MeshDistortLite;

[Serializable]
public class MeshDistortData
{
	public Mesh mesh;

	public MeshFilter filter;

	public Material originalMaterial;

	public Transform meshTransform;

	protected Matrix4x4 skinLocalToWorldMatrix;

	protected Matrix4x4 skinWorldToLocalMatrix;

	private Mesh bakedMesh = new Mesh();

	public Vector3[] originalVertices;

	public SkinnedMeshRenderer skin;

	public ComputeBuffer verticeBuffer;

	public ComputeBuffer matrixBuffer;

	public Transform[] bones;

	public Transform root;

	public Matrix4x4 localToWorldMatrix
	{
		get
		{
			if (skin == null)
			{
				return meshTransform.localToWorldMatrix;
			}
			return skinLocalToWorldMatrix;
		}
	}

	public Matrix4x4 worldToLocalMatrix
	{
		get
		{
			if (skin == null)
			{
				return meshTransform.worldToLocalMatrix;
			}
			return skinWorldToLocalMatrix;
		}
	}

	public Vector3[] skinVertices
	{
		get
		{
			if (skin == null)
			{
				return originalVertices;
			}
			mesh.vertices = originalVertices;
			Transform parent = skin.transform.parent;
			Vector3 localScale = skin.transform.localScale;
			skin.transform.parent = null;
			skin.transform.localScale = Vector3.one;
			skin.BakeMesh(bakedMesh);
			skinLocalToWorldMatrix = skin.transform.localToWorldMatrix;
			skinWorldToLocalMatrix = skin.transform.worldToLocalMatrix;
			skin.transform.parent = parent;
			skin.transform.localScale = localScale;
			return bakedMesh.vertices;
		}
	}

	public MeshDistortData(Transform transform, Material material, MeshFilter filter)
	{
		this.filter = filter;
		originalMaterial = material;
		meshTransform = transform;
		UpdateMesh();
		originalVertices = mesh.vertices;
	}

	public MeshDistortData(Transform transform, Material material, SkinnedMeshRenderer skin)
	{
		this.skin = skin;
		originalMaterial = material;
		meshTransform = transform;
		UpdateMesh();
		originalVertices = mesh.vertices;
		if (Application.isPlaying)
		{
			bones = skin.bones;
			root = skin.rootBone;
		}
	}

	public void CreateBuffers()
	{
		ReleaseBuffers();
		verticeBuffer = new ComputeBuffer(originalVertices.Length, 12);
		matrixBuffer = new ComputeBuffer(2, 64);
	}

	public void ReleaseBuffers()
	{
		if (verticeBuffer != null)
		{
			verticeBuffer.Dispose();
			verticeBuffer = null;
		}
		if (matrixBuffer != null)
		{
			matrixBuffer.Dispose();
			matrixBuffer = null;
		}
	}

	public void BufferSet(ComputeShader shader, int kernel)
	{
		shader.SetBuffer(kernel, "vertices", verticeBuffer);
		shader.SetBuffer(kernel, "matrixList", matrixBuffer);
	}

	public void UpdateMesh()
	{
		if (Application.isPlaying)
		{
			if (skin != null)
			{
				skin.sharedMesh = UnityEngine.Object.Instantiate(skin.sharedMesh);
				mesh = skin.sharedMesh;
			}
			else
			{
				mesh = filter.mesh;
			}
		}
		else
		{
			mesh = UnityEngine.Object.Instantiate((filter != null) ? filter.sharedMesh : skin.sharedMesh);
			mesh.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public void ResetMesh()
	{
		mesh.vertices = originalVertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}
