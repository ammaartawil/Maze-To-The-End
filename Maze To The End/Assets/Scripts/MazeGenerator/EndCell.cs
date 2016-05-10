using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndCell : MonoBehaviour {

	private GameObject text;
	private GameObject score;
	private GameManager gameManager;

	private bool won = false;

	// Use this for initialization
	void Start () {
		text = GameObject.FindGameObjectWithTag ("Text");
		score = GameObject.FindGameObjectWithTag ("Score");
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
		score.GetComponent<Text> ().text = "Score\n" + gameManager.player1Wins + " - " + gameManager.player2Wins;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if (!won && collider.gameObject.tag.Equals ("Player")) {
			won = true;
			int playerNumber = collider.gameObject.GetComponent<Player> ().playerNumber;
			text.GetComponent<Text>().text = "Player " + playerNumber + " wins!";

			// Update scores
			if (playerNumber == 1) {
				gameManager.player1Wins++;
			} else {
				gameManager.player2Wins++;
			}

			score.GetComponent<Text> ().text = "Score\n" + gameManager.player1Wins + " - " + gameManager.player2Wins;


			StartCoroutine (Restart());
		}
	}

	// After x seconds, reload game
	IEnumerator Restart() {
		yield return new WaitForSeconds(2);
		gameManager.RestartGame ();
	}
}
