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
		Inventory.equipped = gameObject;
		if (item) {
			//Inventory.hitting = weapon;
			Inventory.equippedIsForPlacing = true;
		} else if (weapon) {
			//Inventory.hitting = weapon;
			Inventory.equippedIsForPlacing = true;
		}
	}

	public void drop() {

		GameObject collectable = (GameObject)Dictionary.get ("Collectable");
		
		collectable.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
		collectable.GetComponent<Collect> ().objectThisRepresents = Dictionary.get (invName);
		GameObject.Instantiate (collectable, transform.position, Quaternion.identity);


	}
	
}
