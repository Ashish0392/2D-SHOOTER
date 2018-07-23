using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;            // Array (list) of all back and foregrounds to be parallaxed
	private float[] parallaxScales;             // The proportion of camera movement to move backgrounds
	public float smoothing = 1f;                  // how smooth the parallax. always > 0

	private Transform cam;                      // main camera transform
	private Vector3 previousCamPos;             // position of camera in previous frame


	//Is called before start. Used for reference.
	void Awake (){
         
		    // set up camera the reference
		cam = Camera.main.transform;
	
	}


	// Use this for initialization
	void Start () {
		// previous frame had current frames camera position
		previousCamPos = cam.position;

		//assigning coresponding parallaxscales 
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++) {
		
			parallaxScales[i] = backgrounds[i].position.z * -1;

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	 
		//for each background
		for (int i = 0; i < backgrounds.Length; i++) {
			// the parallax is opposite of camera move bcoz previous frame multiplied by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales [i];

			// set a target x position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//create a target position which is the background's current position with it's target x position 

			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds [i].position.y, backgrounds [i].position.z);

			//fade between current position and the target position using lerp
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);


		}

		//set previous campos to cameras position at the end of the frame
		previousCamPos = cam.position;
	}
}
