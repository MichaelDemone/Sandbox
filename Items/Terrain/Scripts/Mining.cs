using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour {
	
	public int strength;
	public Sprite[] breakingStages;
	public float timeBetweenBreaking;


	private float miningDistance;
	private GameObject player;
	private float timeLastHit;
	

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		timeLastHit = 0;
		miningDistance = player.GetComponent<Stats> ().miningDistance;
	}
	
	// Update is called once per frame
	void Update () {
		if (strength == 0) {
			CreateMap.map.Remove(transform.position);
			GetComponent<Item>().drop();
			GameObject.Destroy (this.gameObject);
		}
	}
	
	void OnMouseOver() {

		if (Input.GetMouseButton (0)) {

			Vector3 vectorDifference = this.transform.position - player.transform.position;

			// Possibly replace this with a trigger
			if (vectorDifference.magnitude < miningDistance && Time.time - timeLastHit > timeBetweenBreaking) {
				strength -= player.GetComponent<Stats> ().strength;
				timeLastHit = Time.time;
			}
		}
	}
}
