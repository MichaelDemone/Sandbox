using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour{

	// General
	private static GameObject inventor;
	private static bool init = false;

	public bool item, weapon;
	public int maxStackSize, numberOfDrops;
	public Sprite equipped, inventorySprite;
	public new string name;


	// Weapons
	public int strength;

	// Time

	void Start() {
		if (!init) {
			inventor = GameObject.Find ("Inventory");
			init = true;
		} 
	}


	public void selectItem() {
		inventor.GetComponent<Inventory> ().equipped = gameObject;
		if (item) {
			inventor.GetComponent<Inventory> ().hitting = weapon;
			inventor.GetComponent<Inventory> ().placing = item;
		} else if (weapon) {
			inventor.GetComponent<Inventory> ().hitting = weapon;
			inventor.GetComponent<Inventory> ().placing = item;
		}
	}
	
}
