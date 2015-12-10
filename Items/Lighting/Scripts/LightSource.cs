using UnityEngine;
using System.Collections;

public class LightSource : MonoBehaviour {

	public static ArrayList lightSources;

	public float brightness, range;
	public float blockRange;

	public bool onPlayer = false;

	// Asks if blocks around torch have been told to light up
	private bool litUp;

	// Use this for initialization
	void Start () {
		if (!Tweakables.darknessActivated) {
			enabled = false;
			return;
		}
		if (lightSources == null) {
			lightSources = new ArrayList ();
			lightSources.Add (gameObject);
		} else {
			lightSources.Add (gameObject);
		}

		GetComponent<Light> ().intensity = brightness;
		GetComponent<Light> ().range = range;

		litUp = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (onPlayer || !litUp) {

			// Unilluminate blocks lighted by player
			Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position, blockRange + 2);
			
			foreach(Collider2D col in cols) {
				if(col.CompareTag("Tile")) {
					unIlluminateMain(col.gameObject);
				}
			}

			lightUp();
			litUp = true;
		}
	}

	// Light up the blocks around this light
	public void lightUp() {
		
		Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position, blockRange);
		
		foreach(Collider2D col in cols) {
			if(col.CompareTag("Tile")) {
				illuminateMain(col.gameObject);
			}
		}
	}

	// When disabled, make the blocks around this light not light
	void OnDisable() {
		if (lightSources == null)
			return;
		lightSources.Remove (gameObject);

		// Unilluminate all the tiles around the light
		Collider2D[] cols2 = Physics2D.OverlapCircleAll (transform.position, blockRange + 2);
		
		foreach(Collider2D col in cols2) {
			if(col.CompareTag("Tile")) {
				unIlluminateMain(col.gameObject);
			}
		}

		// Make all the objects re-light up
		foreach(GameObject go in lightSources) {
			go.GetComponent<LightSource>().lightUp();
		}


	}

	/********* LIGHTING BLOCKS AROUND THIS BLOCK ***************/
	private void illuminateMain(GameObject go) {
		
		go.GetComponent<SpriteRenderer> ().color = Color.white;
		
		int x = Mathf.RoundToInt(go.transform.position.x);
		int y = Mathf.RoundToInt(go.transform.position.y);
		Vector3 origin = new Vector3(x,y,0);
		
		// Illuminate blocks around this block at half intensity
		illuminateOther(origin, 0,1);
		illuminateOther(origin, 1,1);
		illuminateOther(origin, 1,0);
		illuminateOther(origin, 1,-1);
		illuminateOther(origin, 0,-1);
		illuminateOther(origin, -1,-1);
		illuminateOther(origin, -1,0);
		illuminateOther(origin, -1,1);
		return;
		
	}
	
	private void illuminateOther(Vector2 origin, int x, int y) {
		Vector3 pos = origin;
		pos.x += x;
		pos.y += y;
		Collider2D col = Physics2D.OverlapPoint (pos);
		
		if (col != null && col.CompareTag ("Tile")) {
			if(col.GetComponent<SpriteRenderer>().color != Color.white)
				col.GetComponent<SpriteRenderer>().color = Color.grey;
		}
	}
	
	/********* DELIGHTING BLOCKS AROUND THIS BLOCK ***************/
	
	private void unIlluminateMain(GameObject go) {
		go.GetComponent<SpriteRenderer> ().color = Color.black;
		Vector3 pos = go.transform.position;
		pos.x = Mathf.RoundToInt (pos.x);
		pos.y = Mathf.RoundToInt (pos.y);
		unIlluminateOther(pos, 0,1);
		unIlluminateOther(pos, 1,1);
		unIlluminateOther(pos, 1,0);
		unIlluminateOther(pos, 1,-1);
		unIlluminateOther(pos, 0,-1);
		unIlluminateOther(pos, -1,-1);
		unIlluminateOther(pos, -1,0);
		unIlluminateOther(pos, -1,1);
	}
	
	private void unIlluminateOther(Vector3 origin, int x, int y) {
		Vector2 pos = origin;
		pos.x += x;
		pos.y += y;
		
		Collider2D col = Physics2D.OverlapPoint (pos);
		
		if (col != null && col.CompareTag ("Tile")) {
			if(col.GetComponent<SpriteRenderer>().color != Color.black)
				col.GetComponent<SpriteRenderer>().color = Color.black;
		}
	}
}
