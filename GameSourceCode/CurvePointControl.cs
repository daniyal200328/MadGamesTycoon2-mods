using UnityEngine;
using UnityEngine.EventSystems;

public class CurvePointControl : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	public int objectNumber;

	public void OnDrag(PointerEventData eventData)
	{
		base.transform.position = Input.mousePosition;
		DrawCurve.use.UpdateLine(objectNumber, Input.mousePosition);
	}
}
