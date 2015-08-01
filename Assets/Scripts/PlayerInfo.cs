using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	public Transform spawn;
	public Transform dunkPanel;
	public float boost;
	public float maxBoost = 1000;
	public float time = 3.0f;
	public int rechargeRate = 3;
	public int team;
	public int dunkThreshold = 20;

	string victoryString;
	GameObject[] particles;
	bool boosting;
	BBall ball;
	Slider meter;
	Text slamText;
	Outline outline;
	HSBColor hsbc;
	Text mashText;

	public bool dunking;
	int dunkCount;

	// Use this for initialization
	void Awake () {
		boost = maxBoost / 2;
		hsbc = HSBColor.FromColor (Color.red);
		mashText = GameObject.Find("MashText").GetComponent<Text>();
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
		mashText.text = "";
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
		}
		else {
			for(int i = 0; i < particles.Length; i++) {
				if(particles[i]) {
					particles[i].SetActive(false);
				}
			}
		}

		if(boost < maxBoost && !boosting) {
			boost += rechargeRate;
			if(boost > maxBoost)
				boost = maxBoost;
		}
	}

	public void LateUpdate() {
		boosting = false;
	}

	public void Slam() {
		if(ball.player == transform && slamText.text.Length >= victoryString.Length 
		   && GetComponent<BoxCollider2D>().IsTouching(GameObject.Find("SlamZone").GetComponent<BoxCollider2D>())) {
			dunkCount = 0;
			dunking = true;
			mashText.text = "MASH: " + dunkCount + "/" + dunkThreshold;
		}
	}

	public void ChargeDunk() {
		dunkCount++;
		mashText.text = "MASH: " + dunkCount + "/" + dunkThreshold;
		if(dunkCount >= dunkThreshold) {
			dunkPanel.gameObject.SetActive(true);
		}
	}

	public void DrainBoost(int amount) {
		boosting = true;
		boost -= amount;
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().Emit(1);
	}

	public IEnumerator Respawn(float time) {
		yield return new WaitForSeconds(time);
	}

	public void AddLetter() {
		int current = slamText.text.Length;
		if(slamText.text.Length < victoryString.Length) {
			slamText.text = victoryString.Substring(0, current + 1);
		}
	}
}
