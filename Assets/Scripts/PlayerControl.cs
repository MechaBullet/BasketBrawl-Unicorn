using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	PlayerMovement movement;
	PlayerCombat combat;
	PlayerInfo info;
	BBall ball;
	private Vector2 angle;
	string playerString;

	string[] inputs = new string[6]{"Horizontal","Vertical","Fire1","Fire2","Jump","Slam"};
	public int targetDir;
	public Sprite[] inputIcons; /*= new string[6] {
											{"InputIcons_D", "InputIcons_W", "InputIcons_Mouse1", "InputIcons_Mouse2", "InputIcons_Space", "InputIcons_Shift"},
											{"InputIcons_A", "InputIcons_S", null, null, null, null}
	};*/
	public string targetKey = "";
	public string targetIcon = "";

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
	}

	void Update() {
		if(info.dunking && targetKey != "") {
			CheckTargetKey();
		}

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

	public void CheckTargetKey() {
		if(targetKey == "Horizontal" || targetKey == "Vertical") {
			if(Mathf.Floor(Input.GetAxis(targetKey + playerString)*targetDir) == targetDir) {
				SetNewTargetKey();
				info.ChargeDunk(targetKey, targetDir);
			}
		}
		else if (Input.GetButtonDown(targetKey + playerString)) {
			SetNewTargetKey();
			info.ChargeDunk(targetKey, targetDir);
		}
	}

	public void SetNewTargetKey() {
		int yNum = 0;
		int num = Random.Range(0, inputs.Length - 1);

		targetKey = inputs[num];
		if(targetKey == "Horizontal" || targetKey == "Vertical") {
			int[] options = new int[2]{-1,1};
			targetDir = options[Random.Range(0, 1)];
			if(targetDir == 1) num += 6;
		}
		Sprite sprite = inputIcons[num];
		transform.FindChild("InputPrompt").GetComponent<SpriteRenderer>().sprite = sprite;
		Debug.Log(sprite);
	}
}
