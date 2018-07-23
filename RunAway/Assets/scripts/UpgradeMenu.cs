using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(weapon))]
public class UpgradeMenu : MonoBehaviour {

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Text speedText;

	[SerializeField]
	private Text fireRateText;

	[SerializeField]
	private float healthMultiplier = 1.25f;

	[SerializeField]
	private float speedMultiplier = 1.3f;

	[SerializeField]
	private float fireRateAddon = 2f;

	[SerializeField]
	private int UpgradeCost = 50;



	private PlayerStats stats;






	AudioManager audioManager;

	[SerializeField]
	string mouseHoverSound = "ButtonHover";
	[SerializeField]
	string buttonPressSound = "ButtonPress";

	void Start()
	{
		audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("audiomanager not found");
		}

}


	void OnEnable()
	{
		stats = PlayerStats.instance;
		UpdateValues ();

	
	}


	void UpdateValues()
	{
		healthText.text = "HEALTH : " + stats.maxHealth.ToString();
		speedText.text = "SPEED : " + stats.movementSpeed.ToString();
		fireRateText.text = "FIRE RATE : " + stats.fireRate.ToString();
}

	public void UpgradeHealth()
	{  
		if (GameMaster.Money < UpgradeCost) {

			audioManager.PlaySound ("NoMoney");
			return;
		}
		
		stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);

		GameMaster.Money -= UpgradeCost;

		UpdateValues ();
		audioManager.PlaySound(buttonPressSound);
	}

	public void UpgradeSpeed()
	{  
		if (GameMaster.Money < UpgradeCost) {

			audioManager.PlaySound ("NoMoney");
			return;
		}
		stats.movementSpeed = Mathf.Round(stats.movementSpeed * speedMultiplier);

		GameMaster.Money -= UpgradeCost;

		UpdateValues ();
		audioManager.PlaySound(buttonPressSound);
	}

	public void UpgradeFireRate()
	{  
		

		if (GameMaster.Money < UpgradeCost) {

			audioManager.PlaySound ("NoMoney");
			return;
		}

		stats.fireRate += fireRateAddon;

		GameMaster.Money -= UpgradeCost;

		UpdateValues ();
		audioManager.PlaySound(buttonPressSound);
	}

	public void OnMouseOver()
	{
		audioManager.PlaySound(mouseHoverSound);
	}
}
