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
		if (Inventory.equipped != null && Inventory.equipped.GetComponent<Item> ().name.Equals ("Torch")) {
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

		if (name.Equals ("Torch")) {
			GameObject lightPrefab = Dictionary.get("Light");
			GameObject light = (GameObject) GameObject.Instantiate(lightPrefab);
			light.transform.SetParent(GameObject.Find("Player").transform);
			light.transform.localPosition = new Vector3(0.1f,0,-1);
			light.GetComponent<Light>().intensity = 8f;
			light.GetComponent<Light>().range = 4f;
		}
	}

	public void drop() {

		GameObject collectable = (GameObject)Dictionary.get ("Collectable");
		
		collectable.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
		collectable.GetComponent<Collect> ().objectThisRepresents = Dictionary.get (invName);
		GameObject.Instantiate (collectable, transform.position, Quaternion.identity);


	}
	
}
