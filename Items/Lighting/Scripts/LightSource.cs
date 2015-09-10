using UnityEngine;
using System.Collections;

public class LightSource : MonoBehaviour {

	public float brightness, range;
	public float blockRange;

	// Use this for initialization
	void Start () {
		GetComponent<Light> ().intensity = brightness;
		GetComponent<Light> ().range = range;
	}

	public void place() {
		
	}

	// Update is called once per frame
	void FixedUpdate () {

		GetComponent<Light> ().intensity = brightness;
		GetComponent<Light> ().range = range;


		Collider2D[] cols2 = Physics2D.OverlapCircleAll (transform.position, blockRange + 3);
		
		foreach(Collider2D col in cols2) {
			if(col.CompareTag("Tile")) {
				col.GetComponent<SpriteRenderer>().color = Color.black;
			}
		}

		Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position, blockRange);

		foreach(Collider2D col in cols) {
			if(col.CompareTag("Tile")) {
				col.GetComponent<Item>().illuminateMain();
			}
		}


	}

	void OnDestroy() {
		Collider2D[] cols2 = Physics2D.OverlapCircleAll (transform.position, 5);
		
		foreach(Collider2D col in cols2) {
			if(col.CompareTag("Tile")) {
				col.GetComponent<Item>().unIlluminateMain();
			}
		}
	}
}
