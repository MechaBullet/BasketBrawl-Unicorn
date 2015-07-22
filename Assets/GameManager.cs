using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	GameObject[] spawns;

	// Use this for initialization
	void Awake () {
		spawns = GameObject.FindGameObjectsWithTag("Player Spawn");
		for(int i = 0; i < spawns.Length; i++) {
			//Get the transform of the spawn point and calculate point below
			Transform target = spawns[i].transform;
			RaycastHit2D hit = Physics2D.Raycast(target.position, -Vector2.up);
			//Load the hoop object and calculate it's offset from the ground
			GameObject hoop = (GameObject)Resources.Load("Hoop");
			Vector3 hoopOffset = -Vector3.right * transform.localScale.x + Vector3.up * (hoop.GetComponent<BoxCollider2D>().size.y * hoop.transform.localScale.y) / 2;
			//Create the player and hoop
			GameObject player = (GameObject)Instantiate((GameObject)Resources.Load("Player"), (Vector3)hit.point, target.rotation);
			hoop = (GameObject)Instantiate((GameObject)Resources.Load("Hoop"), (Vector3)hit.point + hoopOffset, target.rotation);
			//Set instantiated objects' properties
			player.GetComponent<PlayerInfo>().spawn = transform;
			player.GetComponent<PlayerInfo>().team = i;
			hoop.GetComponent<Hoop>().team = i;
			//Set the hoop and player number to the spawn numbers
		}
	}
}
