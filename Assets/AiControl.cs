using UnityEngine;
using System.Collections;

public class AiControl : MonoBehaviour {
	public float shootRange = 15.0f;
	public float jumpRange = 15.0f;
	public float attackRange = 2.0f;

	PlayerMovement movement;
	PlayerCombat combat;
	PlayerInfo info;
	BBall ball;

	private Vector2 angle;
	private string playerString;

	Transform hoop;
	bool jump;
	bool jumping;
	bool boosting;
	float horizontal;
	int fire;
	// 0 = None
	// 1 = Down
	// 2 = Held
	// 3 = Up
	
	// Use this for initialization
	void Start () {
		movement = GetComponent<PlayerMovement>();
		combat = GetComponent<PlayerCombat>();
		info = GetComponent<PlayerInfo>();
		ball = GameObject.Find("Ball").GetComponent<BBall>();
		angle = Vector2.right;
		playerString = " - Player " + info.team;
		horizontal = 0;

		GameObject[] hoops = GameObject.FindGameObjectsWithTag("Basket");
		for(int i = 0; i < hoops.Length; i++) {
			if(hoops[i].GetComponent<Hoop>().team != info.team) {
				hoop = hoops[i].transform;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	/*
	 * Calculate movement
	 * ==================
	 * 	If we dont have the ball..
	 * 		Move toward it
	 * 		If player is close...
	 * 			Attack them
	 *  Otherwise...
	 * 		Move to the basket
	 * 		Avoid the player
	 * 
	*/
		boosting = false;
		jump = false;

		if(ball.player != transform) {
			// Calculate movement
			horizontal = Mathf.Lerp(horizontal, Mathf.Clamp(ball.transform.position.x - transform.position.x, -1, 1), Time.deltaTime * 4);
			// Check jump
			if(ball.transform.position.y > transform.position.y + 2 && Vector2.Distance(transform.position, ball.transform.position) < jumpRange && !jumping) {
				StartCoroutine(JumpTimeout());
				jump = true;
			}
			// Check attack
			Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, attackRange);
			for(int i = 0; i < nearby.Length; i++) {
				if(nearby[i].tag == "Player" && (nearby[i].GetComponent<PlayerInfo>().team%2+1)%2+1 != (info.team%2+1)%2+1) {
					horizontal = Mathf.Lerp(horizontal, 0, Time.deltaTime * 5);
					Vector2 direction = nearby[i].transform.position - transform.position;
					combat.Attack(direction.x, direction.y, false);
				}
				   //(int)((team % 2 + 1 ) % 2 + 1)
			}
		}
		else {
			if(Vector2.Distance(transform.position, hoop.position) > shootRange/2) {
				horizontal = Mathf.Lerp(horizontal, Mathf.Clamp(hoop.transform.position.x - transform.position.x, -1, 1), Time.deltaTime * 4);
			}
			else {
				horizontal = 0;
			}
		}
		movement.Move(horizontal, boosting, jump);
	}
	
	void Update() {
		// If we started firing, 'hold down' the firing button
		if (fire == 1) fire++;
		// If we are in range and have the ball...
		if(ball.player == transform && Vector2.Distance(transform.position, hoop.position) < shootRange) {
			// If we are not currently firing, fire
			if(fire == 0) fire++;
		}
		// If we are firing and think we can take the shot, we go for it
		if(fire == 2) {
			RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.1f, Quaternion.AngleAxis(-(Vector2.Distance(transform.position, hoop.position) * 1.5f), Vector3.forward) * angle);
			foreach(RaycastHit2D hit in hits) {
				if(hit.transform.tag == "Hoop") {
					fire++;
				}
			}
		}

		if(ball.player == transform && fire == 1) {
			angle = Vector2.right;
		}
		
		else if(ball.player == transform && fire == 2) {
			angle = combat.IncreaseThrowAngle(angle);
			Debug.DrawLine( transform.position, transform.position + new Vector3(angle.x * transform.localScale.x, angle.y, 0) * 2 );
		}
		
		else if(ball.player == transform && fire == 3) {
			angle = combat.Throw(angle);
		}
		
		else if(fire == 1) {
			combat.Attack(Input.GetAxis("Horizontal" + playerString), Input.GetAxis("Vertical" + playerString), false);
		}
		if(Input.GetAxis ("Vertical" + playerString) == -1) {
			info.Slam();
		}
		if(fire == 3) {
			fire = 0;
		}
	}

	IEnumerator JumpTimeout() {
		jumping = true;
		yield return new WaitForSeconds(1.0f);
		jumping = false;
	}
}
