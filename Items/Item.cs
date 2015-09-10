using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour{

	// General
	public bool item, weapon;
	public int maxStackSize, numberOfDrops;
	public Sprite equipped, inventorySprite;
	public new string name;
	public string invName;


	// Weapons
	public int strength;

	// Time

	void Start() {

	}


	public void selectItem() {
		if (Inventory.equipped != null && Inventory.equipped.GetComponent<Item> ().CompareTag("Light")) {
			Destroy(GameObject.Find ("Player").GetComponentInChildren<Light>().gameObject);
		}

		Inventory.equipped = gameObject;
		if (item) {
			//Inventory.hitting = weapon;
			Inventory.equippedIsForPlacing = true;
		} else if (weapon) {
			//Inventory.hitting = weapon;
			Inventory.equippedIsForPlacing = true;
		}

		if (CompareTag("Light")) {
			GameObject lightPrefab = Dictionary.get("Light");
			GameObject light = (GameObject) GameObject.Instantiate(lightPrefab);
			light.transform.SetParent(GameObject.Find("Player").transform);
			light.transform.localPosition = new Vector3(0.1f,0,-1);
		}
	}

	public void drop() {

		GameObject collectable = (GameObject)Dictionary.get ("Collectable");
		
		collectable.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
		collectable.GetComponent<Collect> ().objectThisRepresents = Dictionary.get (invName);
		GameObject.Instantiate (collectable, transform.position, Quaternion.identity);
	}


	// Lighting
	public void illuminateMain() {

		GetComponent<SpriteRenderer> ().color = Color.white;

		int x = Mathf.RoundToInt(transform.position.x);
		int y = Mathf.RoundToInt(transform.position.y);
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

	private void illuminate() {
		GetComponent<SpriteRenderer>().color = Color.grey;
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



	public void unIlluminateMain() {
		GetComponent<SpriteRenderer> ().color = Color.black;
		unIlluminateOther(0,1);
		unIlluminateOther(1,1);
		unIlluminateOther(1,0);
		unIlluminateOther(1,-1);
		unIlluminateOther(0,-1);
		unIlluminateOther(-1,-1);
		unIlluminateOther(-1,0);
		unIlluminateOther(-1,1);
	}

	private void unIlluminate() {
		GetComponent<SpriteRenderer> ().color = Color.black;
	}

	private void unIlluminateOther(int x, int y) {
		Vector2 pos = transform.position;
		pos.x += x;
		pos.y += y;

		Collider2D col = Physics2D.OverlapPoint (pos);

		if (col != null && col.CompareTag ("Tile")) {
			if(col.GetComponent<SpriteRenderer>().color != Color.black)
				col.GetComponent<SpriteRenderer>().color = Color.black;
		}
	}
}
