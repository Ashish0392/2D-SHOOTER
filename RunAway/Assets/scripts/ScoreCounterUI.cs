using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreCounterUI : MonoBehaviour {

	private Text scoreText;

	void Awake()
	{
		scoreText = GetComponent<Text>();

	}


	// Update is called once per frame
	void Update () {
	
		scoreText.text = "SCORE : " + GameMaster.score.ToString ();

	}


}
