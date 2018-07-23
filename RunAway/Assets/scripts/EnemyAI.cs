using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

	//what to chase?
	public Transform target;

	//how many times each second we will update our path
	public float updateRate = 2f;

	//caching
	private Seeker seeker;
	private Rigidbody2D rb;

	//the calculated path
	public Path path;

	//the AI's Speed per sec

	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;

	//the max distance from the AI to a waypoint for it to the next waypoint
	public float nextWaypointDistance = 3f;

	//waypoint we are currently moving toward
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false; 

	void Start (){
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		if (target == null) {
		        if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
				}
				return;
		}

		//Start a new path to the target position, return the result to the onepathcomplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath ());


	}

	IEnumerator SearchForPlayer()
	{
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");

		if (sResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchForPlayer ());
		} else {
			target = sResult.transform;
			searchingForPlayer = false;

			StartCoroutine (UpdatePath ());

			return false;

		}
	}

	IEnumerator UpdatePath(){
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return false;
		}

		//Start a new path to the target position, return the result to the onepathcomplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1f / updateRate);
		StartCoroutine (UpdatePath());
	}
	 

	public void OnPathComplete(Path p){
		Debug.Log ("we got a path. Did it have an error" + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}

	}

	void FixedUpdate(){
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return;
		}

		//TODO: Always look at player?
		if (path == null)
			return;

		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded)
				return;

			Debug.Log ("End of path Reached.");
			pathIsEnded = true;
			return;

		}

		pathIsEnded = false;

		//Direction to next waypoint

		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//move the AI
		rb.AddForce (dir, fMode);

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);

		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}

	}

}
