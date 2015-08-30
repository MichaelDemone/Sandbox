using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour{

	// General
	private static bool init = false;

	public bool item, weapon;
	public int maxStackSize, numberOfDrops;
	public Sprite equipped, inventorySprite;
	public new string name;


	// Weapons
	public int strength;

	// Time

	void Start() {

	}


	public void selectItem() {
		Inventory.equipped = gameObject;
		if (item) {
			//Inventory.hitting = weapon;
			Inventory.placing = true;
		} else if (weapon) {
			//Inventory.hitting = weapon;
			Inventory.placing = true;
		}
	}
	
}
