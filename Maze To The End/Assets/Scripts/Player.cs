using UnityEngine;

public class Player : MonoBehaviour {

	public int playerNumber;
	public Material player1Material;
	public Material player2Material;
	public float RotationSpeed = 0.01f;
	private Rigidbody rb;
	public float speed;
	public float rotateSpeed;

	public void Awake (){
		rb = GetComponent<Rigidbody> ();
	}

	public void setPlayerNumber (int number) {
		playerNumber = number;
		if (number == 1) {
			GetComponent<Renderer> ().material = player1Material;
		} else {
			GetComponent<Renderer> ().material = player2Material;
		}
	}

	/*public void SetLocation (MazeCell cell) {
		currentCell = cell;
		transform.localPosition = new Vector3(cell.transform.localPosition.x,
			cell.transform.localPosition.y + 0.5f, cell.transform.localPosition.z);
	}*/
	public void SetStartLocation(Cell cell, int size) {
		//transform.localPosition = new Vector3((int)UnityEngine.Random.Range (0, width), 3f, (int)UnityEngine.Random.Range (0, height));
		transform.position = new Vector3(cell.xPos * 3, 3f, size * 3f - cell.zPos * 3);
	}

	/*private void Move (MazeDirection direction) {
		MazeCellEdge edge = currentCell.GetEdge(direction);
		if (edge is MazePassage) {
			SetLocation(edge.otherCell);
		}
	}

	private void Rotate (MazeDirection direction) {
		//transform.localRotation = direction.ToRotation();
		transform.rotation = Quaternion.Lerp(transform.rotation, direction.ToRotation(), Time.time * RotationSpeed);
		//currentDirection = direction;
		print (currentDirection);
	}*/

	void Update () {
		if (playerNumber == 2 && Input.GetKey(KeyCode.UpArrow) || playerNumber == 1 && Input.GetKey(KeyCode.W) ) {
			//Move(currentDirection);
			rb.transform.Translate (Vector3.forward * speed * Time.deltaTime);
		}
		if (playerNumber == 2 && Input.GetKey(KeyCode.RightArrow) || playerNumber == 1 && Input.GetKey(KeyCode.D)) {
			//Rotate(currentDirection.GetNextClockwise());
			transform.Rotate (Vector3.up * rotateSpeed * Time.deltaTime);
		}
		if (playerNumber == 2 && Input.GetKey(KeyCode.DownArrow) || playerNumber == 1 && Input.GetKey(KeyCode.S)) {
			//Move(currentDirection.GetOpposite());
			rb.transform.Translate (Vector3.back * speed * Time.deltaTime);
		}
		if (playerNumber == 2 && Input.GetKey(KeyCode.LeftArrow) || playerNumber == 1 && Input.GetKey(KeyCode.A)) {
			//Rotate(currentDirection.GetNextCounterclockwise());
			transform.Rotate (-Vector3.up * rotateSpeed * Time.deltaTime);
		}

	}
}
