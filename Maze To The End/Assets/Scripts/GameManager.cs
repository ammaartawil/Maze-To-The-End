using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public MazeGenerator mazeGenerator;
	public Player playerPrefab;
	private Player player1Instance;
	private Player player2Instance;

	public Camera mainCamera;

	private void Start () {
		BeginGame();
	}

	private void Update () {
		
	}

	private void BeginGame () {
		mazeGenerator.Init();
		player1Instance = Instantiate (playerPrefab) as Player;
		player1Instance.setPlayerNumber (1);
		//player1Instance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		player1Instance.SetStartLocation (mazeGenerator.cells[(int)Random.Range (0, mazeGenerator.size), (int)Random.Range(0, mazeGenerator.size)], mazeGenerator.size);
		player2Instance = Instantiate (playerPrefab) as Player;
		player2Instance.setPlayerNumber (2);
		// I feel like this is a redundant line
		player2Instance.SetStartLocation (mazeGenerator.cells[(int)Random.Range (0, mazeGenerator.size), (int)Random.Range(0, mazeGenerator.size)], mazeGenerator.size);
		player1Instance.GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, 0.49f, 1f);
		player2Instance.GetComponentInChildren<Camera> ().rect = new Rect (0.51f, 0f, 0.49f, 1f);
		mainCamera.rect = new Rect (0f, 0f, 0.49f, 1f);
	}		
		
	private void RestartGame () {
		StopAllCoroutines ();
		Destroy(mazeGenerator.gameObject);
		if (player1Instance != null) {
			Destroy (player1Instance.gameObject);
		}
		if (player2Instance != null) {
			Destroy (player2Instance.gameObject);
		}

		BeginGame();
	}
}