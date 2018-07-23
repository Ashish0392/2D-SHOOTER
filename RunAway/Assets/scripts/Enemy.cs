using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats{

		public int maxHealth;

		private int _curHealth;

		public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public int damage = 40;

	

		public void Init()
		{
			curHealth = maxHealth;
		}
	}

	public float shakeAmount = 0.1f;

	public float shakeLenght = 0.1f;

	public int moneyDrop = 10;

	public int scoreDrop = 50;

	public Transform enemyDeathParticles;

	public string deathSound = "Explosion";


	public EnemyStats stats = new EnemyStats ();

	[Header("Optional :")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start()
	{
		

		stats.Init ();

		if (statusIndicator != null) {

			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}

		if (enemyDeathParticles == null) {
			Debug.LogError ("death particle na hai enemy ka");
		}

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
	}

	void OnUpgradeMenuToggle (bool active)
	{
		//handle what happens when upgradeMenu toggled

		GetComponent<EnemyAI> ().enabled = !active;

	}


	public void DamageEnemy (int damage){
		stats.curHealth -= damage;

		if (stats.curHealth <= 0) {
			//Debug.LogError ("enemy mara");
			GameMaster.KillEnemy (this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}

	}

	void OnCollisionEnter2D (Collision2D _collInfo)
	{
		//Debug.LogError ("Bc collision to ho gai");
		Player _player = _collInfo.collider.GetComponent<Player> ();

		if (_player != null) {
			//Debug.LogError ("player bhi null nai");
			_player.DamagePlayer (stats.damage);
			DamageEnemy (99999);

		}
	}

	void OnDestroy()
	{
		
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
