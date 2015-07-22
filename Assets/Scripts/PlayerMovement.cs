using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D)) ]

public class PlayerMovement : MonoBehaviour {
	public float speed = 2.0f;
	public float jumpHeight = 15.0f;
	private Rigidbody2D body;
	private LayerMask mask;
	private float origSpeed;
	// Use this for initialization
	void Start () {
		origSpeed = speed;
		body = GetComponent<Rigidbody2D>();
	}

	public void Move() {
		if(Input.GetButton("Slam") && GetComponent<PlayerInfo>().slam > 0) {
			GetComponent<PlayerInfo>().DrainSlam(2);
			speed = origSpeed * 2;
		}
		else speed = origSpeed;

		body.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0), ForceMode2D.Impulse);
		Vector3 scale = transform.localScale;
		if(Input.GetAxis("Horizontal") > 0 && scale.x < 0) {
			scale.x *= -1;
			transform.localScale = scale;
		}
		else if(Input.GetAxis("Horizontal") < 0 && scale.x > 0) {
			scale.x *= -1;
			transform.localScale = scale;
		}
		//Store the current velocity
		Vector2 vel = body.velocity;
		if(vel.x > speed) {
			vel.x = speed;
			body.velocity = vel;
		}
		else if(vel.x < -speed) {
			vel.x = -speed;
			body.velocity = vel;
		}

		//Check for jump
		if(Input.GetButtonDown("Jump")) {
			//body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
			//vel.y = jumpHeight;

			switch(WallCheck()) {
			case 0:
				if(Grounded()) body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
				break;
			case 1:
				//Debug.Log("Wall on right");
				body.velocity = Vector2.zero;
				body.AddForce(Vector2.up * jumpHeight + -Vector2.right * origSpeed, ForceMode2D.Impulse);
				break;
			case 2:
				//Debug.Log("Wall on left");
				body.velocity = Vector2.zero;
				body.AddForce(Vector2.up * jumpHeight + Vector2.right * origSpeed, ForceMode2D.Impulse);
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

	public bool Grounded() {
		RaycastHit2D hit;
		bool result = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, 0, -Vector2.up, 0.5f);
		//Debug.Log(result);
		return result;
	}
}
