using UnityEngine;

public class Player : MonoBehaviour {

	public int playerNumber;
	public Material player1Material;
	public Material player2Material;

	public float speed;
	public float rotateSpeed;

	private Rigidbody rb;

	public void Awake () {
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

	public void SetStartLocation(Cell cell, int size) {
		transform.position = new Vector3(cell.xPos * 3, 3f, size * 3f - cell.zPos * 3);
	}
		
	void Update () {
		if (Input.anyKey == false) {
			rb.angularVelocity = Vector3.zero;
			rb.velocity = Vector3.zero;
		} else {
			if (playerNumber == 2 && Input.GetKey (KeyCode.UpArrow) || playerNumber == 1 && Input.GetKey (KeyCode.W)) {
				rb.transform.Translate (Vector3.forward * speed * Time.deltaTime);
			}
			if (playerNumber == 2 && Input.GetKey (KeyCode.RightArrow) || playerNumber == 1 && Input.GetKey (KeyCode.D)) {
				transform.Rotate (Vector3.up * rotateSpeed * Time.deltaTime);
			}
			if (playerNumber == 2 && Input.GetKey (KeyCode.DownArrow) || playerNumber == 1 && Input.GetKey (KeyCode.S)) {
				rb.transform.Translate (Vector3.back * speed * Time.deltaTime);
			}
			if (playerNumber == 2 && Input.GetKey (KeyCode.LeftArrow) || playerNumber == 1 && Input.GetKey (KeyCode.A)) {
				transform.Rotate (-Vector3.up * rotateSpeed * Time.deltaTime);
			}
		}
	}
}
