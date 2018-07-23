using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	private Image bgImage;
	private Image joystickImage;
	private Vector3 inputVector;

	private void Start()
	{
		bgImage = GetComponent<Image>();
		joystickImage = transform.GetChild (0).GetComponent<Image> ();
	}

	public virtual void OnDrag(PointerEventData ped)
	{
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bgImage.rectTransform,ped.position,ped.pressEventCamera,out pos)) {

			Debug.Log ("done");
		}
	}
	public virtual void OnPointerDown(PointerEventData ped)
	{
		OnDrag (ped);
	}
	public virtual void OnPointerUp(PointerEventData ped)
	{

	}

}
