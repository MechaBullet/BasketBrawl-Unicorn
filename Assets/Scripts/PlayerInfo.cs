using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	public Transform spawn;
	public Transform dunkPanel;
	public float slam;
	public float maxSlam = 1000;
	public float time = 3.0f;
	public int team;

	GameObject particles;
	BBall ball;
	Slider meter;
	Outline outline;
	HSBColor hsbc;

	// Use this for initialization
	void Awake () {
		slam = maxSlam / 2;
		hsbc = HSBColor.FromColor (Color.red);
		meter = GameObject.Find("Canvas/SlamMeter").GetComponent<Slider>();
		ball = GameObject.Find("Ball").GetComponent<BBall>();
		dunkPanel = GameObject.Find("End").transform;
		meter.maxValue = maxSlam;
		outline = meter.transform.FindChild("Background").GetComponent<Outline>();
		//Debug.Log(outline.transform.name);
	}

	void Start() {
		particles = GameObject.Find("Particles");
		dunkPanel.gameObject.SetActive(false);
		particles.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		meter.value = Mathf.Lerp(meter.value, slam, Time.deltaTime);

		if(slam >= maxSlam) {
			//Debug.Log(HSBColor.ToColor(hsbc));
			hsbc.h = (hsbc.h + Time.deltaTime / time) % 1.0f;
			outline.effectColor = HSBColor.ToColor(hsbc);

			if(ball.player == transform) {
				particles.SetActive(true);
			}
			else particles.SetActive(false);
			//meter.colors.normalColor = Color
		}
		else {
			outline.effectColor = Color.black;
			particles.SetActive(false);
		}
	}

	public void Slam() {
		if(particles.activeSelf == true && GetComponent<BoxCollider2D>().IsTouching(GameObject.Find("SlamZone").GetComponent<BoxCollider2D>())) {
			//Debug.Log("Slam Dunk");
			dunkPanel.gameObject.SetActive(true);
		}
	}

	public void DrainSlam(int amount) {
		slam -= amount;
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().Emit(1);
	}

	public IEnumerator Respawn(float time) {
		yield return new WaitForSeconds(time);
	}
}
