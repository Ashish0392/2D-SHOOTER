using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {





	public int fallBoundary = -20;

	public string deathSound = "DeathVoice";
	public string damageSound = "Grunt";

	AudioManager audioManager;

	[SerializeField]
	public StatusIndicator statusIndicator;

	private PlayerStats stats;

	void Start()
	{
		stats = PlayerStats.instance;

		stats.curHealth = stats.maxHealth; 

		if (statusIndicator == null) {
			Debug.LogError ("No status indicator referenced on player");
		} 
		else {
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("audiomanager not found");
		}

		InvokeRepeating ("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
	}

	void RegenHealth()
	{
		stats.curHealth += 1;
		statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
	}

	void Update (){
		if (transform.position.y <= fallBoundary)
			DamagePlayer (9999);
		
	}

	void OnUpgradeMenuToggle (bool active)
	{
		//handle what happens when upgradeMenu toggled

		GetComponent<Platformer2DUserControl> ().enabled = !active;

		weapon _weapon = GetComponentInChildren<weapon> ();
		if (_weapon != null)
			_weapon.enabled = !active;


	}

	public void DamagePlayer(int damage){
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {

			audioManager.PlaySound (deathSound);
			GameMaster.KillPlayer (this);

		} else {
			audioManager.PlaySound (damageSound);
		}
		statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);

	}

	void OnDestroy()
	{
		
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
