using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	PlayerMovement movement;
	PlayerCombat combat;
	PlayerInfo info;
	BBall ball;
	private Vector2 angle;

	// Use this for initialization
	void Start () {
		movement = GetComponent<PlayerMovement>();
		combat = GetComponent<PlayerCombat>();
		info = GetComponent<PlayerInfo>();
		ball = GameObject.Find("Ball").GetComponent<BBall>();
		angle = Vector2.right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		movement.Move();
	}

	void Update() {
		if(ball.player == transform && Input.GetButtonDown("Fire2")) {
			angle = Vector2.right;
		}

		if(ball.player == transform && Input.GetButton("Fire2")) {
			angle = combat.IncreaseThrowAngle(angle);
			Debug.DrawLine( transform.position, transform.position + new Vector3(angle.x * transform.localScale.x, angle.y, 0) * 2 );
		}

		if(ball.player == transform && Input.GetButtonUp("Fire2")) {
			//Debug.Log("Throwing Ball");
			angle = combat.Throw(angle);
		}

		if(Input.GetButtonDown("Fire1")) {
			combat.Attack();
		}

		if(Input.GetButtonDown ("Dunk")) {
			info.Slam();
		}
	}
}
