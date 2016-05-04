using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;
	private MazeDirection currentDirection;
	public int playerNumber;
	public Material player1Material;
	public Material player2Material;

	public void setPlayerNumber (int number) {
		playerNumber = number;
		if (number == 1) {
			GetComponent<Renderer> ().material = player1Material;
		} else {
			GetComponent<Renderer> ().material = player2Material;
		}
	}

	public void SetLocation (MazeCell cell) {
		currentCell = cell;
		transform.localPosition = new Vector3(cell.transform.localPosition.x,
			cell.transform.localPosition.y + 2, cell.transform.localPosition.z);
	}

	private void Move (MazeDirection direction) {
		MazeCellEdge edge = currentCell.GetEdge(direction);
		if (edge is MazePassage) {
			SetLocation(edge.otherCell);
		}
	}

	private void Rotate (MazeDirection direction) {
		transform.localRotation = direction.ToRotation();
		currentDirection = direction;
		print (currentDirection);
	}

	private void Update () {
		if (playerNumber == 1 && Input.GetKeyDown(KeyCode.UpArrow) || playerNumber == 2 && Input.GetKeyDown(KeyCode.W) ) {
			Move(currentDirection);
		}
		else if (playerNumber == 1 && Input.GetKeyDown(KeyCode.RightArrow) || playerNumber == 2 && Input.GetKeyDown(KeyCode.D)) {
			Rotate(currentDirection.GetNextClockwise());
		}
		else if (playerNumber == 1 && Input.GetKeyDown(KeyCode.DownArrow) || playerNumber == 2 && Input.GetKeyDown(KeyCode.S)) {
			Move(currentDirection.GetOpposite());
		}
		else if (playerNumber == 1 && Input.GetKeyDown(KeyCode.LeftArrow) || playerNumber == 2 && Input.GetKeyDown(KeyCode.A)) {
			Rotate(currentDirection.GetNextCounterclockwise());
		}
	}
}
