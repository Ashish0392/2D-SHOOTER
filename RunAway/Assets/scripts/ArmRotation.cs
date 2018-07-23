using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 90;
	
	// Update is called once per frame
	void Update () {

		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;  // subtract the position of player from mouse position
		difference.Normalize ();                           //normalizing the difference to give only direction vector 

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;  // find the angle in degrees
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);

	
	}
}
