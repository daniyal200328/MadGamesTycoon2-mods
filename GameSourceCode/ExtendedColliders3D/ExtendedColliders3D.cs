using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedColliders3D;

[Serializable]
[AddComponentMenu("Physics/Extended Colliders 3D")]
public class ExtendedColliders3D : MonoBehaviour
{
	public enum ColliderType
	{
		Circle,
		CircleHalf,
		Cone,
		ConeHalf,
		Cube,
		Cylinder,
		CylinderHalf,
		Quad,
		Triangle,
		Sphere
	}

	[Serializable]
	public class ExtendedCollders3DProperties
	{
		public bool convex;

		public bool isTrigger;

		public PhysicMaterial material;

		public ColliderType colliderType = ColliderType.Cylinder;

		public Vector3 centre = Vector3.zero;

		public Vector3 rotation = Vector3.zero;

		public Vector3 size = Vector3.one;

		public bool flipFaces;

		public int circleVertices = 16;

		public bool circleTwoSided;

		public int coneFaces = 16;

		public bool coneCap = true;

		public bool coneHalfCapFlatEnd = true;

		public bool cubeTopFace = true;

		public bool cubeBottomFace = true;

		public bool cubeLeftFace = true;

		public bool cubeRightFace = true;

		public bool cubeForwardFace = true;

		public bool cubeBackFace = true;

		public int cylinderFaces = 16;

		public bool cylinderCapTop = true;

		public bool cylinderCapBottom = true;

		public Vector2 cylinderTaperTop = Vector2.one;

		public Vector2 cylinderTaperBottom = Vector2.one;

		public bool cylinderHalfCapFlatEnd = true;

		public bool quadTwoSided;

		public bool triangleTwoSided;

		public int sphereStacks = 8;

		public int sphereSlices = 16;

		public Color colour = new Color32(145, 244, 140, 239);
	}

	public ExtendedCollders3DProperties properties = new ExtendedCollders3DProperties();

	private void Reset()
	{
		autoSizeColliderToMeshFilter();
	}

	private void Awake()
	{
		MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
		meshCollider.enabled = base.enabled;
		meshCollider.sharedMesh = generateMesh(applyTransform: false);
		meshCollider.convex = properties.convex;
		meshCollider.isTrigger = properties.isTrigger;
		meshCollider.material = properties.material;
		UnityEngine.Object.Destroy(this);
	}

	private void Start()
	{
	}

	private Mesh generateMesh(bool applyTransform)
	{
		Mesh obj = new Mesh
		{
			name = "Extended Colliders 3D Mesh"
		};
		generateVerticesAndTriangles(applyTransform, out var vertices, out var triangles);
		obj.vertices = vertices;
		obj.triangles = triangles;
		obj.RecalculateNormals();
		return obj;
	}

	private void generateVerticesAndTriangles(bool applyTransform, out Vector3[] vertices, out int[] triangles)
	{
		int num = 0;
		if (properties.colliderType == ColliderType.Circle || properties.colliderType == ColliderType.CircleHalf)
		{
			int num2 = properties.circleVertices + ((properties.colliderType != ColliderType.Circle) ? 1 : 0);
			vertices = new Vector3[num2];
			triangles = new int[(num2 - 2) * (properties.circleTwoSided ? 6 : 3)];
			List<int> list = new List<int>();
			for (int i = 0; i < num2; i++)
			{
				float f = (float)i / (float)properties.circleVertices * MathF.PI * (float)((properties.colliderType != ColliderType.Circle) ? 1 : 2);
				vertices[i] = new Vector3(Mathf.Sin(f) / 2f, -0.5f, Mathf.Cos(f) / 2f);
				list.Add(i);
			}
			int num3 = list.Count / 2;
			bool flag = false;
			while (list.Count > 2)
			{
				triangles[num++] = list[(num3 + list.Count - 1) % list.Count];
				triangles[num++] = list[num3];
				triangles[num++] = list[(num3 + 1) % list.Count];
				list.RemoveAt(num3);
				if (flag)
				{
					num3 = (num3 + list.Count - 1) % list.Count;
				}
				flag = !flag;
			}
			if (properties.circleTwoSided)
			{
				for (int j = 0; j < num2 - 2; j++)
				{
					triangles[(j + num2 - 2) * 3] = triangles[j * 3];
					triangles[(j + num2 - 2) * 3 + 1] = triangles[j * 3 + 2];
					triangles[(j + num2 - 2) * 3 + 2] = triangles[j * 3 + 1];
				}
			}
		}
		else if (properties.colliderType == ColliderType.Cone || properties.colliderType == ColliderType.ConeHalf)
		{
			int num4 = properties.coneFaces + ((properties.colliderType != ColliderType.Cone) ? 1 : 0);
			vertices = new Vector3[num4 + 1];
			triangles = new int[num4 * 3 + (properties.coneCap ? ((num4 - 2) * 3) : 0) - ((properties.colliderType != ColliderType.Cone && !properties.coneHalfCapFlatEnd) ? 3 : 0)];
			vertices[vertices.Length - 1] = new Vector3(0f, 0.5f, 0f);
			for (int k = 0; k < num4; k++)
			{
				float f2 = (float)k / (float)properties.coneFaces * MathF.PI * (float)((properties.colliderType != ColliderType.Cone) ? 1 : 2);
				vertices[k] = new Vector3(Mathf.Sin(f2) / 2f, -0.5f, Mathf.Cos(f2) / 2f);
				if (properties.colliderType == ColliderType.Cone || properties.coneHalfCapFlatEnd || k < num4 - 1)
				{
					triangles[num++] = k;
					triangles[num++] = (k + 1) % num4;
					triangles[num++] = vertices.Length - 1;
				}
			}
			if (properties.coneCap)
			{
				List<int> list2 = new List<int>();
				for (int l = 0; l < num4; l++)
				{
					list2.Add(l);
				}
				int num5 = list2.Count / 2;
				bool flag2 = false;
				while (list2.Count > 2)
				{
					triangles[num++] = list2[(num5 + list2.Count - 1) % list2.Count];
					triangles[num++] = list2[(num5 + 1) % list2.Count];
					triangles[num++] = list2[num5];
					list2.RemoveAt(num5);
					if (flag2)
					{
						num5 = (num5 + list2.Count - 1) % list2.Count;
					}
					flag2 = !flag2;
				}
			}
		}
		else if (properties.colliderType == ColliderType.Cube)
		{
			vertices = new Vector3[8];
			triangles = new int[(properties.cubeTopFace ? 6 : 0) + (properties.cubeBottomFace ? 6 : 0) + (properties.cubeLeftFace ? 6 : 0) + (properties.cubeRightFace ? 6 : 0) + (properties.cubeForwardFace ? 6 : 0) + (properties.cubeBackFace ? 6 : 0)];
			int num6 = 0;
			for (int m = -1; m <= 1; m += 2)
			{
				for (int n = -1; n <= 1; n += 2)
				{
					for (int num7 = -1; num7 <= 1; num7 += 2)
					{
						vertices[num6++] = new Vector3((float)n / 2f, (float)m / 2f, (float)num7 / 2f);
					}
				}
			}
			if (properties.cubeBottomFace)
			{
				triangles[num++] = 0;
				triangles[num++] = 2;
				triangles[num++] = 1;
				triangles[num++] = 1;
				triangles[num++] = 2;
				triangles[num++] = 3;
			}
			if (properties.cubeTopFace)
			{
				triangles[num++] = 4;
				triangles[num++] = 5;
				triangles[num++] = 6;
				triangles[num++] = 6;
				triangles[num++] = 5;
				triangles[num++] = 7;
			}
			if (properties.cubeLeftFace)
			{
				triangles[num++] = 0;
				triangles[num++] = 1;
				triangles[num++] = 4;
				triangles[num++] = 4;
				triangles[num++] = 1;
				triangles[num++] = 5;
			}
			if (properties.cubeRightFace)
			{
				triangles[num++] = 3;
				triangles[num++] = 2;
				triangles[num++] = 6;
				triangles[num++] = 3;
				triangles[num++] = 6;
				triangles[num++] = 7;
			}
			if (properties.cubeBackFace)
			{
				triangles[num++] = 0;
				triangles[num++] = 4;
				triangles[num++] = 2;
				triangles[num++] = 4;
				triangles[num++] = 6;
				triangles[num++] = 2;
			}
			if (properties.cubeForwardFace)
			{
				triangles[num++] = 1;
				triangles[num++] = 3;
				triangles[num++] = 5;
				triangles[num++] = 5;
				triangles[num++] = 3;
				triangles[num++] = 7;
			}
		}
		else if (properties.colliderType == ColliderType.Cylinder || properties.colliderType == ColliderType.CylinderHalf)
		{
			int num8 = properties.cylinderFaces + ((properties.colliderType != ColliderType.Cylinder) ? 1 : 0);
			vertices = new Vector3[num8 * 2];
			triangles = new int[num8 * 6 + (properties.cylinderCapTop ? ((num8 - 2) * 3) : 0) + (properties.cylinderCapBottom ? ((num8 - 2) * 3) : 0) - ((properties.colliderType != ColliderType.Cylinder && !properties.cylinderHalfCapFlatEnd) ? 6 : 0)];
			for (int num9 = 0; num9 < num8; num9++)
			{
				float f3 = (float)num9 / (float)properties.cylinderFaces * MathF.PI * (float)((properties.colliderType != ColliderType.Cylinder) ? 1 : 2);
				vertices[num9] = new Vector3(Mathf.Sin(f3) / 2f, 0.5f, Mathf.Cos(f3) / 2f);
				vertices[num9 + num8] = vertices[num9] + new Vector3(0f, -1f, 0f);
				vertices[num9].x *= properties.cylinderTaperTop.x;
				vertices[num9].z *= properties.cylinderTaperTop.y;
				vertices[num9 + num8].x *= properties.cylinderTaperBottom.x;
				vertices[num9 + num8].z *= properties.cylinderTaperBottom.y;
				if (properties.colliderType == ColliderType.Cylinder || properties.cylinderHalfCapFlatEnd || num9 < num8 - 1)
				{
					triangles[num++] = num9;
					triangles[num++] = num9 + num8;
					triangles[num++] = (num9 + 1) % num8;
					triangles[num++] = (num9 + 1) % num8;
					triangles[num++] = num9 + num8;
					triangles[num++] = Mathf.Max((num9 + num8 + 1) % (num8 * 2), num8);
				}
			}
			for (int num10 = 0; num10 < 2; num10++)
			{
				if ((num10 != 0 || !properties.cylinderCapTop) && (num10 != 1 || !properties.cylinderCapBottom))
				{
					continue;
				}
				List<int> list3 = new List<int>();
				for (int num11 = 0; num11 < num8; num11++)
				{
					list3.Add(num11 + num8 * num10);
				}
				int num12 = list3.Count / 2;
				bool flag3 = false;
				while (list3.Count > 2)
				{
					triangles[num++] = list3[(num12 + list3.Count - 1) % list3.Count];
					triangles[num++] = list3[(num10 == 0) ? num12 : ((num12 + 1) % list3.Count)];
					triangles[num++] = list3[(num10 == 1) ? num12 : ((num12 + 1) % list3.Count)];
					list3.RemoveAt(num12);
					if (flag3)
					{
						num12 = (num12 + list3.Count - 1) % list3.Count;
					}
					flag3 = !flag3;
				}
			}
		}
		else if (properties.colliderType == ColliderType.Quad)
		{
			vertices = new Vector3[4]
			{
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0f, 0.5f)
			};
			triangles = new int[properties.quadTwoSided ? 12 : 6];
			triangles[num++] = 0;
			triangles[num++] = 1;
			triangles[num++] = 2;
			triangles[num++] = 3;
			triangles[num++] = 2;
			triangles[num++] = 1;
			if (properties.quadTwoSided)
			{
				triangles[num++] = 0;
				triangles[num++] = 2;
				triangles[num++] = 1;
				triangles[num++] = 3;
				triangles[num++] = 1;
				triangles[num++] = 2;
			}
		}
		else if (properties.colliderType == ColliderType.Triangle)
		{
			vertices = new Vector3[3]
			{
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, -0.5f)
			};
			triangles = new int[properties.triangleTwoSided ? 6 : 3];
			triangles[num++] = 0;
			triangles[num++] = 1;
			triangles[num++] = 2;
			if (properties.triangleTwoSided)
			{
				triangles[num++] = 0;
				triangles[num++] = 2;
				triangles[num++] = 1;
			}
		}
		else
		{
			if (properties.colliderType != ColliderType.Sphere)
			{
				throw new Exception("Extended Colliders 3D: Unknown collider type.");
			}
			vertices = new Vector3[(properties.sphereStacks - 1) * properties.sphereSlices + 2];
			triangles = new int[properties.sphereStacks * properties.sphereSlices * 6];
			vertices[0] = new Vector3(0f, 0.5f, 0f);
			vertices[vertices.Length - 1] = new Vector3(0f, -0.5f, 0f);
			for (int num13 = 1; num13 < properties.sphereStacks; num13++)
			{
				float num14 = Mathf.Sin((float)num13 / (float)properties.sphereStacks * MathF.PI);
				float y = Mathf.Cos((float)num13 / (float)properties.sphereStacks * MathF.PI) / 2f;
				for (int num15 = 0; num15 < properties.sphereSlices; num15++)
				{
					int num16 = (num13 - 1) * properties.sphereSlices + num15 + 1;
					int num17 = ((num15 == properties.sphereSlices - 1) ? ((num13 - 1) * properties.sphereSlices + 1) : (num16 + 1));
					float f4 = (float)num15 / (float)properties.sphereSlices * MathF.PI * 2f;
					vertices[num16] = new Vector3(Mathf.Sin(f4) / 2f * num14, y, Mathf.Cos(f4) / 2f * num14);
					if (num13 == 1)
					{
						triangles[num15 * 3] = 0;
						triangles[num15 * 3 + 1] = num16;
						triangles[num15 * 3 + 2] = num17;
						continue;
					}
					int num18 = properties.sphereSlices * 3 + (num13 - 2) * properties.sphereSlices * 6 + num15 * 6;
					triangles[num18] = num16;
					triangles[num18 + 1] = num17;
					triangles[num18 + 2] = num16 - properties.sphereSlices;
					triangles[num18 + 3] = num17 - properties.sphereSlices;
					triangles[num18 + 4] = num16 - properties.sphereSlices;
					triangles[num18 + 5] = num17;
					if (num13 == properties.sphereStacks - 1)
					{
						num18 = properties.sphereSlices * 3 + (properties.sphereStacks - 2) * properties.sphereSlices * 6 + num15 * 3;
						triangles[num18] = num17;
						triangles[num18 + 1] = num16;
						triangles[num18 + 2] = vertices.Length - 1;
					}
				}
			}
		}
		for (int num19 = 0; num19 < vertices.Length; num19++)
		{
			vertices[num19].x *= properties.size.x;
			vertices[num19].y *= properties.size.y;
			vertices[num19].z *= properties.size.z;
			vertices[num19] = Quaternion.Euler(properties.rotation) * vertices[num19];
			vertices[num19] += properties.centre;
			if (applyTransform)
			{
				Transform parent = base.transform;
				while (parent != null)
				{
					vertices[num19].x *= parent.localScale.x;
					vertices[num19].y *= parent.localScale.y;
					vertices[num19].z *= parent.localScale.z;
					vertices[num19] = parent.localRotation * vertices[num19];
					vertices[num19] += parent.localPosition;
					parent = parent.parent;
				}
			}
		}
		if (properties.flipFaces)
		{
			for (int num20 = 0; num20 < triangles.Length / 3; num20++)
			{
				int num21 = triangles[num20 * 3];
				triangles[num20 * 3] = triangles[num20 * 3 + 1];
				triangles[num20 * 3 + 1] = num21;
			}
		}
	}

	public void autoSizeColliderToMeshFilter()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh mesh = null;
		if (component != null)
		{
			mesh = component.sharedMesh;
		}
		if (mesh != null)
		{
			properties.centre = mesh.bounds.center;
			properties.size = mesh.bounds.size;
		}
	}
}
