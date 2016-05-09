using UnityEngine;
using System.Collections;

public class EndCell : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag.Equals ("Player")) {
			
			print ("Someone entered the end cell! It was player " + collider.gameObject.GetComponent<Player>().playerNumber);
		}
	}
}
