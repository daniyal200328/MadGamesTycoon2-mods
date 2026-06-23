using UnityEngine;

public class sui_demo_ControllerMaster : MonoBehaviour
{
	public enum Sui_Demo_ControllerType
	{
		none,
		character,
		boat,
		orbit
	}

	public Transform cameraObject;

	public Sui_Demo_ControllerType currentControllerType = Sui_Demo_ControllerType.character;

	private sui_demo_ControllerCharacter characterController;

	private sui_demo_ControllerBoat boatController;

	private sui_demo_ControllerOrbit orbitController;

	private bool resetController;

	private Sui_Demo_ControllerType useController = Sui_Demo_ControllerType.character;

	private void Start()
	{
		characterController = base.gameObject.GetComponent<sui_demo_ControllerCharacter>();
		boatController = base.gameObject.GetComponent<sui_demo_ControllerBoat>();
		orbitController = base.gameObject.GetComponent<sui_demo_ControllerOrbit>();
	}

	private void LateUpdate()
	{
		if (currentControllerType != useController)
		{
			resetController = true;
		}
		else
		{
			resetController = false;
		}
		if (currentControllerType == Sui_Demo_ControllerType.none)
		{
			if (characterController != null)
			{
				characterController.isActive = false;
			}
			if (boatController != null)
			{
				boatController.isActive = false;
			}
			if (orbitController != null)
			{
				orbitController.isActive = false;
			}
		}
		if (currentControllerType == Sui_Demo_ControllerType.character)
		{
			if (boatController != null)
			{
				boatController.isActive = false;
			}
			if (orbitController != null)
			{
				orbitController.isActive = false;
			}
			if (characterController != null)
			{
				characterController.isActive = true;
			}
		}
		if (currentControllerType == Sui_Demo_ControllerType.boat)
		{
			if (characterController != null)
			{
				characterController.isActive = false;
			}
			if (orbitController != null)
			{
				orbitController.isActive = false;
			}
			if (boatController != null)
			{
				boatController.isActive = true;
			}
		}
		if (currentControllerType == Sui_Demo_ControllerType.orbit)
		{
			if (characterController != null)
			{
				characterController.isActive = false;
			}
			if (boatController != null)
			{
				boatController.isActive = false;
			}
			if (orbitController != null)
			{
				orbitController.isActive = true;
			}
		}
		if (characterController != null)
		{
			if (currentControllerType == Sui_Demo_ControllerType.boat)
			{
				characterController.isInBoat = true;
				characterController.cameraTarget.transform.position = boatController.targetAnimator.playerPosition.transform.position;
				characterController.cameraTarget.transform.rotation = boatController.targetAnimator.playerPosition.transform.rotation;
				characterController.cameraTarget.gameObject.GetComponent<Collider>().enabled = false;
				characterController.cameraTarget.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			}
			if (currentControllerType == Sui_Demo_ControllerType.character && resetController)
			{
				characterController.isInBoat = false;
				characterController.cameraTarget.transform.position = boatController.targetAnimator.playerExit.transform.position;
				characterController.cameraTarget.gameObject.GetComponent<Collider>().enabled = true;
				characterController.cameraTarget.gameObject.GetComponent<Rigidbody>().useGravity = true;
				characterController.cameraTarget.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			}
		}
		if (resetController)
		{
			resetController = false;
			useController = currentControllerType;
		}
	}
}
