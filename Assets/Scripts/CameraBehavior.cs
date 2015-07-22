using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraBehavior : MonoBehaviour {
	//GameObject[] player;
	List <GameObject> players;
	Vector3 center;
	Vector3 destination;
	Vector3 startPos;
	public float zoom;
	public float oldZoom;

/*	var center : Vector3;
	var destination : Vector3;
	var zoom : float;
	var oldZoom : float;
	var startPos : Vector3;
*/
	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	void Update () {
		players = new List<GameObject>();
		GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < playerObjs.Length; i++) {
			players.Add(playerObjs[i]);
		}
		players.Add(GameObject.Find("Ball"));
		center = Vector3.zero;
		if(players.Count > 0) {
			for(int i = 0; i < players.Count; i++) { 
				center += players[i].transform.position;
			}
			center = center / players.Count;
		}
		else center = startPos;
		
		CalculateZoom();
		Adjust();
	}
	
	void CalculateZoom() {
		zoom = 0;
		for(int i = 0; i < players.Count; i++) {
			float distance;
			distance = Vector3.Distance(players[i].transform.position, center);
			if(distance > zoom) zoom = distance;
		}
		if (zoom < 10) zoom = 10;
		if (zoom > 40) zoom = 40;
	}
	
	void Adjust() {
		destination = new Vector3(center.x, center.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, destination, 5.0f * Time.deltaTime);
		
		oldZoom = GetComponent<Camera>().orthographicSize;
		GetComponent<Camera>().orthographicSize = Mathf.Lerp(oldZoom, zoom, 5.0f * Time.deltaTime);
	}
}
