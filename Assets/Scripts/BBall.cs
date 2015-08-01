using UnityEngine;
using System.Collections;

public class BBall : MonoBehaviour {
	public Transform player;
	public Transform lastPlayer;
	public Vector3 origPos;
	public float value = 150;
	private bool respawning;

	void Start () {
		respawning = false;
		origPos = transform.position;
	}

	// Use this for initialization
	void Update () {
		if(player != null) {
			GetComponent<CircleCollider2D>().isTrigger = true;
			transform.position = player.position + Vector3.right * player.localScale.x;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		if(transform.position.y <= -30 && !respawning) {
			StartCoroutine(Respawn(1.0f));
		}
	}

	void Score() {
		//lastPlayer.GetComponent<PlayerInfo>().score++;
		lastPlayer.GetComponent<PlayerInfo>().AddLetter();
		StartCoroutine(Respawn(2.0f));
	}

	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D col) {
		if(col.transform.tag == "Player" && player == null) {
			player = col.transform;
		}
		if(col.transform.tag == "Hoop" && col.transform.root.GetComponent<Hoop>().team != ((lastPlayer.GetComponent<PlayerInfo>().team % 2 + 1 ) % 2 + 1) ) {
			Score ();
		}
	}

	public void Drop(float time) {
		lastPlayer = player;
		player = null;
		StartCoroutine(Phase(time));
	}

	IEnumerator Phase(float time) {
		GetComponent<CircleCollider2D>().isTrigger = true;
		yield return new WaitForSeconds(time);
		GetComponent<CircleCollider2D>().isTrigger = false;
	}

	public IEnumerator Respawn(float time) {
		respawning = true;
		Renderer renderer =  GetComponent<Renderer>();
		Rigidbody2D body = GetComponent<Rigidbody2D>();

		player = null;
		renderer.enabled = false;
		body.isKinematic = true;
		transform.position = origPos;
		body.velocity = Vector2.zero;
		yield return new WaitForSeconds(time);
		player = null;
		GetComponent<CircleCollider2D>().isTrigger = false;
		renderer.enabled = true;
		body.velocity = Vector2.zero;
		body.isKinematic = false;
		respawning = false;
	}
}
