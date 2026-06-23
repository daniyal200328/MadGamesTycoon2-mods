using Suimono.Core;
using UnityEngine;

[ExecuteInEditMode]
public class utility_ConvertToSurface : MonoBehaviour
{
	public bool convertToSuimono;

	private SuimonoModule moduleObject;

	private SuimonoObject surfaceComponent;

	private GameObject mainObj;

	private Transform surfaceObj;

	private Transform scaleObj;

	private Renderer objRenderer;

	private MeshFilter objMeshFilter;

	private Mesh objMesh;

	private void Start()
	{
	}

	private void LateUpdate()
	{
	}

	private bool CheckAllResources()
	{
		bool result = true;
		if (base.gameObject.GetComponent<Renderer>() == null)
		{
			result = false;
			Debug.Log("GameObject requires a <Renderer> Component!");
		}
		if (base.gameObject.GetComponent<MeshFilter>() == null)
		{
			result = false;
			Debug.Log("GameObject requires a <MeshFilter> Component!");
		}
		else if (base.gameObject.GetComponent<MeshFilter>().sharedMesh == null)
		{
			result = false;
			Debug.Log("MeshFilter requires a Mesh!");
		}
		return result;
	}
}
