using UnityEngine;
using System.Collections;

public class KillOnEscape : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float	tHeight = Camera.main.orthographicSize;
		float	tWidth = tHeight * Camera.main.aspect;
		if (transform.position.x < -tWidth || transform.position.x > tWidth || transform.position.y < -tHeight || transform.position.y > tHeight) {
			Destroy (gameObject);
		}
	}
}
