using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	PlayerMovement movement;
	PlayerCombat combat;
	PlayerInfo info;
	BBall ball;
	private Vector2 angle;
	string playerString;

	// Use this for initialization
	void Start () {
		movement = GetComponent<PlayerMovement>();
		combat = GetComponent<PlayerCombat>();
		info = GetComponent<PlayerInfo>();
		ball = GameObject.Find("Ball").GetComponent<BBall>();
		angle = Vector2.right;
		playerString = " - Player " + info.team;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!info.dunking)
			movement.Move(Input.GetAxis("Horizontal" + playerString), Input.GetButton("Slam" + playerString), Input.GetButtonDown("Jump" + playerString));
		else {
			if(Input.anyKeyDown) {
				info.ChargeDunk();
			}
		}
	}

	void Update() {
		if(ball.player == transform && Input.GetButtonDown("Fire1" + playerString)) {
			angle = Vector2.right;
		}

		else if(ball.player == transform && Input.GetButton("Fire1" + playerString)) {
			angle = combat.IncreaseThrowAngle(angle);
			Debug.DrawLine( transform.position, transform.position + new Vector3(angle.x * transform.localScale.x, angle.y, 0) * 2 );
		}

		else if(ball.player == transform && Input.GetButtonUp("Fire1" + playerString)) {
			//Debug.Log("Throwing Ball");
			angle = combat.Throw(angle);
		}

		else if(Input.GetButtonDown("Fire1" + playerString)) {
			combat.Attack(Input.GetAxis("Horizontal" + playerString), Input.GetAxis("Vertical" + playerString), Input.GetButton("Slam" + playerString));
		}
		if(Input.GetAxis ("Vertical" + playerString) == -1) {
			info.Slam();
		}
	}
}
