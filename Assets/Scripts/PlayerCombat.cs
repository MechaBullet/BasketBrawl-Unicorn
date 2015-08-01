using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {
	public float force = 5.0f;
	Transform ball;
	bool attacking;

	void Start() {
		ball = GameObject.Find("Ball").transform;
	}

	public Vector2 Throw(Vector2 angle) {
		ball.GetComponent<BBall>().Drop(0.025f);
		//ball.transform.position = ball.transform.position + new Vector3(angle.x * transform.localScale.x, angle.y, 0);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(angle.x * transform.localScale.x * 25, angle.y * 25);//, ForceMode2D.Impulse);
		return Vector2.right;
	}
	
	public Vector2 IncreaseThrowAngle(Vector2 angle) {
		if(angle.x < 0 && angle.y > 0) return Vector2.right; 
		return Quaternion.AngleAxis(1.0f, Vector3.forward) * angle;
	}

	public void Attack(float x, float y, bool slam) {
		if(!attacking) {
			StartCoroutine(AttackTimeout());

			Collider2D[] hits;
			Vector2 knockback;
			Vector2 dropForce = Vector2.zero;
			float dazeTime = 0;
			switch(GetDirection(x, y)) {
			case 0:
			case 4:
				hits = ForwardAttack();
				dazeTime = 0.8f;
				knockback = Vector2.right * transform.localScale.x;
				if(slam) {
					knockback *= 1.5f;
					dropForce = Vector2.up * force + Vector2.right * force * -transform.localScale.x;
				}
				break;
	/*		case 1:
				hits = BackAttack();
				knockback = -Vector2.right;
				break;
	*/
			case 2:
				hits = UpAttack();
				knockback = Vector2.up;
				//dropForce = -knockback / 2;
				if (slam) 
					knockback = knockback * 2;
				break;
			case 3:
				hits = DownAttack();
				knockback = -Vector2.up;
				dropForce = Vector2.up * 2 + Vector2.right * -transform.localScale.x * 2;
				if (slam) {
					knockback = knockback * 2;
					dazeTime = 2;
				}
				break;
			//Default
			default:
				hits = new Collider2D[0];
				knockback = Vector2.zero;
				break;
			}

			for(int i = 0; i < hits.Length; i++) {
				if(hits[i].tag == "Player" && hits[i] != transform.GetComponent<Collider2D>()) {
					Hit (hits[i], knockback, dropForce, dazeTime);
				}
			}
		}
	}

	private void Hit(Collider2D player, Vector2 knockback, Vector2 dropForce, float dazeTime) {
		player.GetComponent<PlayerMovement>().Daze(dazeTime);
		//Debug.Log("Hit " + player.name);
		if(Mathf.Abs(knockback.x) > Mathf.Abs(knockback.y)) {
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		player.GetComponent<Rigidbody2D>().AddForce(knockback * force, ForceMode2D.Impulse);
		if(dropForce != Vector2.zero && ball.GetComponent<BBall>().player == player.transform) {
			ball.GetComponent<BBall>().Drop(0.2f);
			ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			ball.GetComponent<Rigidbody2D>().AddForce(dropForce, ForceMode2D.Impulse);
		}
	}

	private Collider2D[] UpAttack() {
		return Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.up, 1.0f);
	}

	private Collider2D[] DownAttack() {
		return Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.right - Vector2.up, 1.0f);
	}

	private Collider2D[] BackAttack() {
		RaycastHit2D[] hits =  Physics2D.BoxCastAll(transform.position, GetComponent<BoxCollider2D>().size, 0, -Vector2.right * transform.localScale.x, 1);
		Collider2D[] hitCols = new Collider2D[hits.Length];
		for(int i = 0; i < hits.Length; i++) {
			hitCols[i] = hits[i].collider;
		}
		return hitCols;
	}

	private Collider2D[] ForwardAttack() {
		RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, GetComponent<BoxCollider2D>().size, 0, Vector2.right * transform.localScale.x, 1);
		Collider2D[] hitCols = new Collider2D[hits.Length];
		for(int i = 0; i < hits.Length; i++) {
			hitCols[i] = hits[i].collider;
		}
		return hitCols;
	}

	private int GetDirection(float x, float y) {
		if(x == 0 && y == 0) {
			return 4;
		}
		else if(Mathf.Abs(x) >= Mathf.Abs(y)) {
			return 0;
		}
		else {
			//Up
			if (y > 0)
				return 2;
			//Down
			else
				return 3;
		}
	}

	IEnumerator AttackTimeout() {
		attacking = true;
		yield return new WaitForSeconds(0.5f);
		attacking = false;
	}
}
