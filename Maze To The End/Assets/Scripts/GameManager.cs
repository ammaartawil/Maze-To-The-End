using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public MazeGenerator mazeGenerator;
	public Player playerPrefab;
	private Player player1Instance;
	private Player player2Instance;
	private GameObject text;

	public int player1Wins;
	public int player2Wins;

	public Camera mainCamera;

	private void Start () {
		text = GameObject.FindGameObjectWithTag ("Text");
		text.GetComponent<Text>().text = "MAZE TO THE END\n This is a maze race!\n Find your way to the green tile.\n First person there wins!\nPlayer 1 controls: WASD\n Player 2 controls: Arrow keys";
		StartCoroutine(StartGame());
	}

	private void Update () {
		
	}

	private void BeginGame () {
		text.GetComponent<Text> ().fontSize = 100;

		mazeGenerator.Init();

		// Set up players
		player1Instance = Instantiate (playerPrefab) as Player;
		player1Instance.setPlayerNumber (1);
		// Random start location
		player1Instance.SetStartLocation (mazeGenerator.getPlayerStart(), mazeGenerator.size);

		player2Instance = Instantiate (playerPrefab) as Player;
		player2Instance.setPlayerNumber (2);
		player2Instance.SetStartLocation (mazeGenerator.getPlayerStart(), mazeGenerator.size);

		// Arrange cameras
		player1Instance.GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, 0.49f, 1f);
		player2Instance.GetComponentInChildren<Camera> ().rect = new Rect (0.51f, 0f, 0.49f, 1f);
		//mainCamera.rect = new Rect (0f, 0f, 0.49f, 1f);

		text.GetComponent<Text>().text = "Go";
		StartCoroutine (HideGo());
	}		
		
	public void RestartGame () {
		StopAllCoroutines ();

		// Destory all the cells in the maze
		foreach(Transform child in mazeGenerator.transform) {
			Destroy(child.gameObject);
		}

		if (player1Instance != null) {
			Destroy (player1Instance.gameObject);
		}
		if (player2Instance != null) {
			Destroy (player2Instance.gameObject);
		}
		text.GetComponent<Text>().text = "";

		BeginGame();
	}

	IEnumerator HideGo() {
		yield return new WaitForSeconds(2);
		text.GetComponent<Text>().text = "";
	}

	IEnumerator StartGame() {
		yield return new WaitForSeconds(10);
		BeginGame ();
	}
}