using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {



	public float fireRate = 5f;
	public int Damage = 10;
	public LayerMask whatToHit;
	public Transform bulletTrailPrefab;
	public Transform muzzleFlashPrefab;
	public Transform hitPrefab;


	float timeToFire = 0;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	//handle camera shaking
	public float camShakeAmount = 0.1f;
	public float camShakeLenght = 0.1f;
	CameraShake CamShake;

	public string weaponShootSound = "DefaultShot";

	AudioManager audioManager;




	Transform firePoint;

	// Use this for initialization
	void Awake () {
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) 
		{
			Debug.LogError ("No FirePoint ? what?");
		}
	
	}

	void Start()
	{
		CamShake = GameMaster.gm.GetComponent<CameraShake> ();

		if (CamShake == null)
			Debug.LogError ("no camerashake script found on GM object");


		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.LogError ("no audio manager found");
		}

	


	}

	void OnEnable()
	{
		fireRate = PlayerStats.instance.fireRate;
	}

	// Update is called once per frame
	void Update () {
		
		
		 
		if (fireRate == 0) {
		
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
			
				timeToFire = Time.time + 1 / fireRate;
				Shoot ();
			}
		}
	
	}

	void Shoot(){
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);


		//Debug.DrawLine (firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);
		if (hit.collider != null) {
			//Debug.DrawLine (firePointPosition, hit.point, Color.red); 
			//Debug.Log ("we hit" + hit.collider.name + "and did" + Damage + "damage.");

			Enemy enemy = hit.collider.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.DamageEnemy (Damage);
			}
		}

		if (Time.time >= timeToSpawnEffect) {
			Vector3 hitPos;
			Vector3 hitNormal;

			if (hit.collider == null) {
				hitPos = (mousePosition - firePointPosition) * 30;
				hitNormal = new Vector3 (999, 999, 999);
			
			} else {
				hitPos = hit.point;
				hitNormal = hit.normal;
			}

			Effect (hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}

	}

	void Effect(Vector3 hitPos, Vector3 hitNormal){
		Transform trail = Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer> ();

		if (lr != null) {
			//set position
			lr.SetPosition (0, firePoint.position);
			lr.SetPosition (1, hitPos);
		}
		Destroy (trail.gameObject, 0.04f);


	

			if (hitNormal != new Vector3 (999, 999, 999)) {
				Transform hitParticle = Instantiate (hitPrefab, hitPos, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;

				Destroy (hitParticle.gameObject, 0.2f);
			}


		Transform clone = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);

		//shake camera
		CamShake.Shake(camShakeAmount, 0.2f);

		//Play shoot sound

		audioManager.PlaySound (weaponShootSound);

	}
}
