using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject player;

	private float zPos;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player2");
		zPos = this.transform.position.z;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = player.transform.position;
		pos.z = zPos;
		transform.position = pos;
	}
}
