using UnityEngine;
using System.Collections;

public class InputScaleCorrection : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if(transform.root.localScale.x < 0 && transform.localScale.x >= 0) {
			Vector3 scale = transform.localScale;
			scale.x = -1;
			transform.localScale = scale;
		}
		else if(transform.localScale.x < 0) {
			Vector3 scale = transform.localScale;
			scale.x = 1;
		}
	}
}
