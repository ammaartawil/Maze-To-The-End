using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	private Maze mazeInstance;
	public Player playerPrefab;
	private Player player1Instance;
	private Player player2Instance;

	private void Start () {
		BeginGame();
	}

	private void Update () {
		
	}

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();
		player1Instance = Instantiate (playerPrefab) as Player;
		player1Instance.setPlayerNumber (1);
		player1Instance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		player2Instance = Instantiate (playerPrefab) as Player;
		player2Instance.setPlayerNumber (2);
		player2Instance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		player1Instance.GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
		player2Instance.GetComponentInChildren<Camera> ().rect = new Rect (0.5f, 0f, 0.5f, 1f);
	}		


	private void RestartGame () {
		StopAllCoroutines ();
		Destroy(mazeInstance.gameObject);
		if (player1Instance != null) {
			Destroy (player1Instance.gameObject);
		}
		if (player2Instance != null) {
			Destroy (player2Instance.gameObject);
		}

		BeginGame();
	}
}