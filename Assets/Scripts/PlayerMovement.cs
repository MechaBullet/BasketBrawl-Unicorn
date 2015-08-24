	using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D)) ]

public class PlayerMovement : MonoBehaviour {
	public float speed = 2.0f;
	private Rigidbody2D body;
	private LayerMask mask;
	private float origSpeed;
	private bool dazed;
	// Use this for initialization
	void Start () {
		origSpeed = speed;
		body = GetComponent<Rigidbody2D>();
		dazed = false;
	}

	public void Move(float horizontal, bool boostButton, bool jumpButton) {
		if(boostButton && GetComponent<PlayerInfo>().boost > 0) {
			GetComponent<PlayerInfo>().DrainBoost(10);
			speed = origSpeed * 1.5f;
		}
		else speed = origSpeed;

		if(!dazed) {
			body.AddForce(new Vector2(horizontal, 0), ForceMode2D.Impulse);
		}

		Vector3 scale = transform.localScale;
		if(horizontal > 0 && scale.x < 0) {
			scale.x *= -1;
			transform.localScale = scale;
		}
		else if(horizontal < 0 && scale.x > 0) {
			scale.x *= -1;
			transform.localScale = scale;
		}
		//Store the current velocity
		Vector2 vel = body.velocity;
		if(vel.x > speed && !dazed) {
			vel.x = speed;
			body.velocity = vel;
		}
		else if(vel.x < -speed && !dazed) {
			vel.x = -speed;
			body.velocity = vel;
		}

		//Check for jump
		if(jumpButton) {
			//body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
			//vel.y = jumpHeight;
			switch(WallCheck()) {
			case 0:
				if(Grounded()) body.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
				break;
			case 1:
				//Debug.Log("Wall on right");
				body.velocity = Vector2.zero;
				body.AddForce(Vector2.up * speed + -Vector2.right * origSpeed, ForceMode2D.Impulse);
				break;
			case 2:
				//Debug.Log("Wall on left");
				body.velocity = Vector2.zero;
				body.AddForce(Vector2.up * speed + Vector2.right * origSpeed, ForceMode2D.Impulse);
				break;
			default: break;
			}
		}
	}

	public int WallCheck() {
		int result;

		//Check left side
		if(Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0, -Vector2.right, 0.25f, 1 << LayerMask.NameToLayer("Platform"))) {
			return result = 2;
		}
		//Check right side
		if(Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0, Vector2.right, 0.25f, 1 << LayerMask.NameToLayer("Platform"))) {
			return result = 1;
		}
		return result = 0;

	}

	public void Daze(float time) {
		StartCoroutine(DazeWait(time));
	}

	IEnumerator DazeWait(float time) {
		dazed = true;
		yield return new WaitForSeconds(time);
		dazed = false;
	}

	public bool Grounded() {
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0, -Vector2.up, 0.5f, layerMask);
		return hit;
	}
}
