using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour {
	
	public int strength;
	public Sprite[] breakingStages;
	public GameObject collectablePeice;
	public float timeBetweenBreaking;
	public int miningDistance;

	private GameObject player, inventory;
	private float timeLastHit;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		inventory = GameObject.Find ("Inventory");
		timeLastHit = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (strength == 0) {
			throwPeice();
			GameObject.Destroy (this.gameObject);
		}
	}

	void throwPeice() {
		Vector3 thisPos = transform.position;
		GameObject.Instantiate (collectablePeice, thisPos, Quaternion.identity);
	}

	void OnMouseOver() {

		if (inventory.GetComponent<Inventory>().hitting && Input.GetMouseButton (0)) {

			Vector3 vectorDifference = this.transform.position - player.transform.position;
			if (vectorDifference.magnitude < miningDistance && Time.time - timeLastHit > timeBetweenBreaking) {
				strength -= player.GetComponent<Stats> ().strength;
				timeLastHit = Time.time;
			}
		}
	}
}
