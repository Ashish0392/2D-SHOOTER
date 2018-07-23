using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]


public class Tiling : MonoBehaviour {

	public int offsetX = 2;                 //offset for errors

	//used for checking if to instantiate
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false;      //if object is not tilable

	private float spriteWidth = 0f;       //width of our element
	private Camera cam;
	private Transform myTransform;


	void Awake () {
	
		cam = Camera.main;
		myTransform = transform;
	
	}


	// Use this for initialization
	void Start () {
	
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;

	}
	
	// Update is called once per frame
	void Update () {
		if (hasALeftBuddy == false || hasARightBuddy == false) {
		 
			//calculate the camera extend (half the width) of what the camera  can see in world coordinates
			float camHorizontalExtended = cam.orthographicSize * Screen.width / Screen.height;

			//calculate the x position where the camera can see the edge of the sprite element
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtended;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtended;


			//checking if we can see the edge of the element and then calling MakeNewBuddy if can
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) 
			{
			
				MakeNewBuddy (1);
				hasARightBuddy = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) 
			{
				MakeNewBuddy (-1);
				hasALeftBuddy = true;
			}


		}
	}

	//function to create buddy on the required side
	void MakeNewBuddy (int rightOrLeft){
		//calculate pos for new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

		//instantiating buddy and storing in variable
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		//if not tilable reverse x 
		if (reverseScale == true) 
		{
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		
		}

		newBuddy.parent = myTransform.parent;

		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true; 
		}

	}


}
