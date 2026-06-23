using System.Collections.Generic;
using UnityEngine;

namespace UntitledTools.VertexWind;

public class VertexWind : MonoBehaviour
{
	public struct WindEffector
	{
		public Vector3 pos;

		public Vector3 strength;

		public float radius;
	}

	public List<MeshFilter> objs = new List<MeshFilter>();

	public float speed = 10f;

	public float scale = 1f;

	public bool useMeshCombination = true;

	public Vector3 amount = Vector3.one * 0.5f;

	private Mesh[] instancedMeshes;

	private Vector3[][] objsOriginalVerts;

	private List<MeshFilter> newObjects = new List<MeshFilter>();

	private WindEffectorRadius[] effectorObjs;

	private WindEffector[] effectors;

	private ComputeShader windShader;

	private int doWindCalcId;

	public string objectTag = string.Empty;

	public bool showAdvancedSelection;

	public bool showObjectsList = true;

	public MeshFilter selectedObj;

	private void Start()
	{
		effectorObjs = Object.FindObjectsOfType<WindEffectorRadius>();
		effectors = new WindEffector[effectorObjs.Length];
		for (int i = 0; i < effectorObjs.Length; i++)
		{
			effectors[i] = default(WindEffector);
		}
		windShader = Resources.Load<ComputeShader>("WindShader");
		doWindCalcId = windShader.FindKernel("DoWindCalc");
		if (useMeshCombination)
		{
			instancedMeshes = CombineMeshes(objs);
			objsOriginalVerts = new Vector3[instancedMeshes.Length][];
			for (int j = 0; j < instancedMeshes.Length; j++)
			{
				instancedMeshes[j].MarkDynamic();
				objsOriginalVerts[j] = instancedMeshes[j].vertices;
			}
			for (int k = 0; k < objs.Count; k++)
			{
				objs[k].gameObject.SetActive(value: false);
			}
		}
		else
		{
			instancedMeshes = new Mesh[objs.Count];
			objsOriginalVerts = new Vector3[objs.Count][];
			for (int l = 0; l < objs.Count; l++)
			{
				instancedMeshes[l] = Object.Instantiate(objs[l].mesh);
				instancedMeshes[l].MarkDynamic();
				objsOriginalVerts[l] = instancedMeshes[l].vertices;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < effectorObjs.Length; i++)
		{
			effectors[i].pos = effectorObjs[i].transform.position;
			effectors[i].radius = effectorObjs[i].radius;
			effectors[i].strength = effectorObjs[i].amount;
		}
		if (useMeshCombination)
		{
			for (int j = 0; j < instancedMeshes.Length; j++)
			{
				instancedMeshes[j].vertices = CalcNewVerts(objsOriginalVerts[j], newObjects[j].transform.position);
				newObjects[j].mesh = instancedMeshes[j];
			}
		}
		else
		{
			for (int k = 0; k < objs.Count; k++)
			{
				instancedMeshes[k].vertices = CalcNewVerts(objsOriginalVerts[k], objs[k].transform.position);
				objs[k].mesh = instancedMeshes[k];
			}
		}
	}

	private Vector3[] CalcNewVerts(Vector3[] verts, Vector3 objectPos)
	{
		ComputeBuffer computeBuffer = new ComputeBuffer(verts.Length, 12);
		computeBuffer.SetData(verts);
		windShader.SetFloat("time", Time.time * speed);
		windShader.SetFloat("scale", scale);
		windShader.SetVector("amount", amount);
		windShader.SetVector("objPos", objectPos);
		windShader.SetBuffer(doWindCalcId, "verts", computeBuffer);
		if (effectors.Length != 0)
		{
			ComputeBuffer computeBuffer2 = new ComputeBuffer(effectors.Length, 28);
			computeBuffer2.SetData(effectors);
			windShader.SetBuffer(doWindCalcId, "effectors", computeBuffer2);
			windShader.SetBool("effectorsExist", val: true);
			windShader.Dispatch(doWindCalcId, verts.Length, effectors.Length, 1);
			computeBuffer2.Release();
		}
		else
		{
			ComputeBuffer computeBuffer3 = new ComputeBuffer(1, 16);
			windShader.SetBuffer(doWindCalcId, "effectors", computeBuffer3);
			windShader.SetBool("effectorsExist", val: false);
			windShader.Dispatch(doWindCalcId, verts.Length, 1, 1);
			computeBuffer3.Release();
		}
		Vector3[] array = new Vector3[verts.Length];
		computeBuffer.GetData(array);
		computeBuffer.Release();
		return array;
	}

	private Mesh[] CombineMeshes(List<MeshFilter> meshes)
	{
		int num = 0;
		int index = 0;
		List<Mesh> list = new List<Mesh>();
		List<CombineInstance> list2 = new List<CombineInstance>();
		for (int i = 0; i < meshes.Count; i++)
		{
			Mesh mesh = Object.Instantiate(meshes[i].mesh);
			num += mesh.vertexCount;
			if (num < 65535)
			{
				CombineInstance item = default(CombineInstance);
				Vector3[] array = new Vector3[mesh.vertexCount];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = mesh.vertices[j] + meshes[i].transform.position;
				}
				item.mesh = mesh;
				item.mesh.vertices = array;
				list2.Add(item);
				continue;
			}
			Mesh mesh2 = new Mesh
			{
				name = "Combined Mesh " + i
			};
			mesh2.CombineMeshes(list2.ToArray(), mergeSubMeshes: true, useMatrices: false, hasLightmapData: false);
			list2 = new List<CombineInstance>();
			list.Add(mesh2);
			GameObject gameObject = Object.Instantiate(meshes[index].gameObject);
			gameObject.transform.position = Vector3.zero;
			gameObject.name = "Combined Meshes " + index + " - " + i;
			gameObject.GetComponent<MeshFilter>().mesh = mesh2;
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			if ((bool)gameObject.GetComponent<Collider>())
			{
				Object.Destroy(gameObject.GetComponent<Collider>());
				gameObject.AddComponent<MeshCollider>().sharedMesh = mesh2;
			}
			newObjects.Add(gameObject.GetComponent<MeshFilter>());
			index = i;
			num = 0;
			i--;
		}
		Mesh mesh3 = new Mesh
		{
			name = "Combined Final Mesh"
		};
		mesh3.CombineMeshes(list2.ToArray(), mergeSubMeshes: true, useMatrices: false, hasLightmapData: false);
		list.Add(mesh3);
		GameObject gameObject2 = Object.Instantiate(meshes[index].gameObject);
		gameObject2.transform.position = Vector3.zero;
		gameObject2.name = "Combined Meshes " + index + " - " + meshes.Count;
		gameObject2.GetComponent<MeshFilter>().mesh = mesh3;
		gameObject2.hideFlags = HideFlags.HideInHierarchy;
		Object.Destroy(gameObject2.GetComponent<Collider>());
		gameObject2.AddComponent<MeshCollider>().sharedMesh = mesh3;
		newObjects.Add(gameObject2.GetComponent<MeshFilter>());
		return list.ToArray();
	}

	public void ObjsAddChildren()
	{
		objs.AddRange(GetComponentsInChildren<MeshFilter>());
	}

	public void ObjsAddCurrent()
	{
		if (GetComponent<MeshFilter>() != null)
		{
			objs.Add(GetComponent<MeshFilter>());
		}
	}

	public void ObjsAddSelected()
	{
	}

	public void ObjsAreTagged()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(objectTag);
		List<MeshFilter> list = new List<MeshFilter>();
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i].GetComponent<MeshFilter>())
			{
				list.Add(array[i].GetComponent<MeshFilter>());
			}
		}
		objs.AddRange(list);
	}
}
