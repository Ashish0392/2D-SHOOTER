using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	private int maxLives = 3; 

	private static int _remainingLives;

	public static int RemainingLives
	{
		get{ return _remainingLives;}
	}

	[SerializeField]
	private int startingMoney;

	public static int Money;

	public float scoreGenerationRate = 2f;

	public static int score;


	void Awake (){
		if (gm == null)
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
	}



	public Transform playerPrefab;
	public Transform spawnPoint; 
	public int respawnDelay = 2;
	public Transform spawnPrefab;
	public string respawnCountdownSound = "RespawnCountdown";
	public string spawnSound = "Spawn";
	public string gameOverSound = "GameOver";

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;

	[SerializeField]
	private GameObject upgradeMenu;

	public delegate void UpgradeMenuCallback(bool active);
	public UpgradeMenuCallback onToggleUpgradeMenu; 


	private AudioManager audioManager;

	void Start()
	{
		_remainingLives = maxLives;
		Money = startingMoney;
		score = 0;

		if (cameraShake == null) {
			Debug.LogError ("no camera shake reference in game master");
		}

		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("audiomanager not found");
		}

		InvokeRepeating ("GenerateScore", 1.5f, 2.5f);

	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.U)) {

			ToggleUpgradeMenu ();

		}

	}

	private void ToggleUpgradeMenu()
	{
		upgradeMenu.SetActive ( !upgradeMenu.activeSelf );
		onToggleUpgradeMenu.Invoke (upgradeMenu.activeSelf);

	}

	public void EndGame()
	{
		Debug.Log ("Game over");

		audioManager.PlaySound (gameOverSound);
		gameOverUI.SetActive (true);

		CancelInvoke ();
	}

	public IEnumerator _RespawnPlayer (){
		audioManager.PlaySound (respawnCountdownSound); 
		yield return new WaitForSeconds (respawnDelay);

		audioManager.PlaySound (spawnSound);
		Transform clonePlayer = Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;

		Transform Clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
		Destroy (Clone.gameObject, 1f);


	}

	public static void KillPlayer (Player player){
		Destroy (player.gameObject);
		_remainingLives -= 1;

		if (_remainingLives <= 0) {

			gm.EndGame ();
		} 
		else {
			gm.StartCoroutine(gm._RespawnPlayer ());
		}


	}

	public static void KillEnemy(Enemy enemy)
	{
		//Debug.LogError ("kill enemy reached");
		gm._killEnemy (enemy);

	}

	public void _killEnemy (Enemy _enemy)
	{
		//sound
		audioManager.PlaySound(_enemy.deathSound);

		Money += _enemy.moneyDrop;

		score += _enemy.scoreDrop;

		//Debug.LogError ("_killenemy reached");
		//particles
		Transform _clone = Instantiate (_enemy.enemyDeathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		//cameshke
		cameraShake.Shake (_enemy.shakeAmount, _enemy.shakeLenght);
		Destroy (_enemy.gameObject);
		Destroy (_clone.gameObject, 0.5f);



	}

	void GenerateScore()
	{
		score += 1;

	}
}
