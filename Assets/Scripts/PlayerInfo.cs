using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	public Transform spawn;
	public Transform dunkPanel;
	public float boost;
	public float maxBoost = 1000;
	public float time = 3.0f;
	public int team;

	string victoryString;
	GameObject[] particles;
	BBall ball;
	Slider meter;
	Text slamText;
	Outline outline;
	HSBColor hsbc;

	// Use this for initialization
	void Awake () {
		boost = maxBoost / 2;
		hsbc = HSBColor.FromColor (Color.red);
		ball = GameObject.Find("Ball").GetComponent<BBall>();
		//slamText = GameObject.Find("Slam - Team " + team / 2);
		dunkPanel = GameObject.Find("End").transform;
		//Debug.Log(outline.transform.name);
	}

	void Start() {
		slamText = GameObject.Find("Canvas/Slam - Team " + (int)((team % 2 + 1 ) % 2 + 1)).GetComponent<Text>();
		//Handle player boost meter
		meter = GameObject.Find("Canvas/BoostMeter - Player " + team).GetComponent<Slider>();
		meter.maxValue = maxBoost;
		outline = meter.transform.FindChild("Background").GetComponent<Outline>();
		particles = GameObject.FindGameObjectsWithTag("SlamZone");
		for(int i = 0; i < particles.Length; i++) {
			if(particles[i].transform.root.GetComponent<Hoop>().team == team) {
				particles[i] = null;
			}
		}
		dunkPanel.gameObject.SetActive(false);
		for(int i = 0; i < particles.Length; i++) {
			if(particles[i]) {
				particles[i].SetActive(false);
			}
		}
		victoryString = slamText.text;
		slamText.text = "";
	}

	// Update is called once per frame
	void Update () {
		meter.value = Mathf.Lerp(meter.value, boost, Time.deltaTime * 5);

		if(slamText.text.Length >= victoryString.Length) {
			//Debug.Log(HSBColor.ToColor(hsbc));
			//hsbc.h = (hsbc.h + Time.deltaTime / time) % 1.0f;
			//outline.effectColor = HSBColor.ToColor(hsbc);

			if(ball.player == transform) {
				for(int i = 0; i < particles.Length; i++) {
					if(particles[i]) {
						particles[i].SetActive(true);
					}
				}
			}
			else {
				for(int i = 0; i < particles.Length; i++) {
					if(particles[i]) {
						particles[i].SetActive(false);
					}
				}
			}
			//meter.colors.normalColor = Color
		}
		else {
			//outline.effectColor = Color.black;
			for(int i = 0; i < particles.Length; i++) {
				if(particles[i]) {
					particles[i].SetActive(false);
				}
			}
			boost += 2;
		}
	}

	public void Slam() {
		if(ball.player == transform && slamText.text.Length >= victoryString.Length && GetComponent<BoxCollider2D>().IsTouching(GameObject.Find("SlamZone").GetComponent<BoxCollider2D>())) {
			//Debug.Log("boost Dunk");
			dunkPanel.gameObject.SetActive(true);
		}
	}

	public void DrainBoost(int amount) {
		boost -= amount;
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().Emit(1);
	}

	public IEnumerator Respawn(float time) {
		yield return new WaitForSeconds(time);
	}

	public void AddLetter() {
		int current = slamText.text.Length;
		slamText.text = victoryString.Substring(0, current + 1);
	}
}
