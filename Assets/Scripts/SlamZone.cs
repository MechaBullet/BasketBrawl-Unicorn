using UnityEngine;
using System.Collections;

public class SlamZone : MonoBehaviour {
	public bool Scoring() {
		return GetComponent<BoxCollider2D>().IsTouching(GameObject.FindGameObjectWithTag("Ball").GetComponent<CircleCollider2D>());
	}
}
