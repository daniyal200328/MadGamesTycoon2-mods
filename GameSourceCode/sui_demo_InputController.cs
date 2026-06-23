using UnityEngine;

public class sui_demo_InputController : MonoBehaviour
{
	[HideInInspector]
	public bool inputMouseKey0;

	[HideInInspector]
	public bool inputKeySHIFTL;

	[HideInInspector]
	public bool inputKeySPACE;

	[HideInInspector]
	public bool inputKeyW;

	[HideInInspector]
	public bool inputKeyS;

	[HideInInspector]
	public bool inputKeyA;

	[HideInInspector]
	public bool inputKeyD;

	[HideInInspector]
	public bool inputKeyF;

	[HideInInspector]
	public bool inputKeyQ;

	[HideInInspector]
	public bool inputKeyE;

	[HideInInspector]
	public bool inputKeyESC;

	[HideInInspector]
	public bool inputMouseKey1;

	[HideInInspector]
	public float inputMouseX;

	[HideInInspector]
	public float inputMouseY;

	[HideInInspector]
	public float inputMouseWheel;

	private void Update()
	{
		inputKeyW = Input.GetKey("w");
		inputKeyS = Input.GetKey("s");
		inputKeyA = Input.GetKey("a");
		inputKeyD = Input.GetKey("d");
		inputKeyQ = Input.GetKey("q");
		inputKeyE = Input.GetKey("e");
		inputMouseKey0 = Input.GetKey("mouse 0");
		inputMouseKey1 = Input.GetKey("mouse 1");
		inputMouseX = Input.GetAxisRaw("Mouse X");
		inputMouseY = Input.GetAxisRaw("Mouse Y");
		inputMouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
		inputKeySHIFTL = Input.GetKey("left shift");
		inputKeySPACE = Input.GetKey("space");
		inputKeyF = Input.GetKey("f");
		inputKeyESC = Input.GetKey("escape");
	}
}
