using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

	GameObject[] spawns;
	public int players = 2;
	//IComparer comparer;

	// Use this for initialization
	void Awake () {
		spawns = GameObject.FindGameObjectsWithTag("Player Spawn");
		spawns = spawns.OrderBy(spawn => spawn.name.Last()).ToArray();
		for(int i = 0; i < spawns.Length && i < players; i++) {
			//Debug.Log(spawns[i].name.Last());
			//Get the transform of the spawn point and calculate point below
			Transform target = spawns[i].transform;
			RaycastHit2D hit = Physics2D.Raycast(target.position, -Vector2.up);
			//Load the hoop object and calculate it's offset from the ground
			GameObject hoop = (GameObject)Resources.Load("Hoop");
			Vector3 hoopOffset = -Vector3.right * target.localScale.x + Vector3.up * (hoop.GetComponent<BoxCollider2D>().size.y * hoop.transform.localScale.y) / 2;
			//Create the player and hoop
			GameObject player = (GameObject)Instantiate((GameObject)Resources.Load("Player"), (Vector3)hit.point, target.rotation);
			hoop = (GameObject)Instantiate((GameObject)Resources.Load("Hoop"), (Vector3)hit.point + hoopOffset, target.rotation);
			Vector3 theScale;
			theScale = hoop.transform.localScale;
			theScale.x *= target.localScale.x;
			hoop.transform.localScale = theScale;

			theScale = player.transform.localScale;
			theScale.x *= target.localScale.x;
			player.transform.localScale = theScale;
			//Set instantiated objects' properties
			player.GetComponent<PlayerInfo>().spawn = transform;
			player.GetComponent<PlayerInfo>().team = i + 1;
			hoop.GetComponent<Hoop>().team = i + 1;
			//Set the hoop and player number to the spawn numbers
		}

		if(players < 4) {
			for(int i = 0; i < players; i++) {

			}
		}
	}
}
