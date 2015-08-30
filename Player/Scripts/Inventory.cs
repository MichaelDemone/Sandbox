﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {


	public static bool hitting = true, equippedIsForPlacing = false;
	public static GameObject equipped;
	public static GameObject[] inventory;
	public static int[] itemAmount;

	private GameObject tileParent;
	private bool currentlyPlacingAnObject = false;
	private float placeTime;
	private KeyCode[] codes;

	private static InventoryUI inventoryUI;
	private const int INVENTORY_SIZE = 81;

	// Use this for initialization
	void Start () {

		// Initializing information about what is in inventory
		inventory = new GameObject[INVENTORY_SIZE];
		for (int i = 0; i < inventory.Length; i++) {
			inventory[i] = null;
		}

		// Initializing information about how many items are in each
		itemAmount = new int[INVENTORY_SIZE];
		for (int i = 0; i < inventory.Length; i++) {
			itemAmount[i] = 0;
		}

		codes = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
			, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};

		placeTime = Time.time;
		inventoryUI = GameObject.Find ("Inventory").GetComponent<InventoryUI>();
		tileParent = GameObject.Find ("Ground");

	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < codes.Length; i++) {
			if(Input.GetKeyDown(codes[i])) {
				inventoryUI.equip(i);
				equipped = inventory[i];
			}
		}

		if (equippedIsForPlacing && Input.GetMouseButton (1) && !currentlyPlacingAnObject) {
			currentlyPlacingAnObject = true;

			Vector3 pos = Input.mousePosition;
			pos.z = -25;
			pos.x = Screen.width - pos.x;
			pos.y = Screen.height - pos.y;
			pos = Camera.main.ScreenToWorldPoint(pos);

			RaycastHit2D hit = Physics2D.Raycast(pos, new Vector3(0,0,1));

			if(hit.collider != null && hit.collider.tag.Equals("Tile")) {
				//Debug.Log(hit.collider.name);
			}
			else {
				int xPos = Mathf.RoundToInt (pos.x);
				int yPos = Mathf.RoundToInt (pos.y);

				GameObject go = (GameObject) GameObject.Instantiate (equipped, new Vector3(xPos, yPos, 0), Quaternion.identity);

				if(go.CompareTag("Tile")) {
					go.transform.parent = tileParent.transform;
				}


			}
		} else if (currentlyPlacingAnObject && Time.time - placeTime > 0.25) {
			currentlyPlacingAnObject = false;
		}

	}

	public static void addItem(GameObject item, GameObject collectable) {

		bool foundEmpty = false;
		int emptyPosition = -1;

		// Go through inventory
		for(int i = 0; i < INVENTORY_SIZE; i++) {

			// If item in slot equals current item, increase amount of it
			if(inventory[i] != null && inventory[i].Equals(item)){
				// If it can't be increased, keep going until a new 
				// open spot is found
				if(increaseNumberOfItems(item, collectable, i)) {
					GameObject.Destroy(collectable);
					return;
				}
			}
			// Records the first empty spot
			else if(!foundEmpty && inventory[i] == null) {
				foundEmpty = true;
				emptyPosition = i;
			}
		}

		// Puts item into first empty spot
		if (foundEmpty) {
			Sprite inventorySprite = item.GetComponent<Item>().inventorySprite;
			inventoryUI.addItem(inventorySprite, emptyPosition);

			inventory[emptyPosition] = item;
			increaseNumberOfItems(item, collectable, emptyPosition);
			GameObject.Destroy(collectable);
			return;
		}

	}

	private static bool increaseNumberOfItems(GameObject item, GameObject collectable, int slotNumber) {
		int maxStackSize = item.GetComponent<Item> ().maxStackSize;
		if (itemAmount [slotNumber] == maxStackSize) {
			return false;
		} else if (itemAmount [slotNumber] > maxStackSize) {
			Debug.Log ("Item amount exceeded max amount.");
			return false;
		} else {
			int amountInCollectable = collectable.GetComponent<Collect>().amount;
			int amountInInventory = itemAmount[slotNumber];

			if(amountInCollectable + amountInInventory > maxStackSize) {
				int amountBeingAdded = maxStackSize - amountInInventory;
				collectable.GetComponent<Collect>().amount -= amountBeingAdded;
				amountInInventory += amountBeingAdded;
				itemAmount[slotNumber] = amountInInventory;
				addItem(item, collectable);

			} else {
				amountInInventory += amountInCollectable;
				itemAmount[slotNumber] = amountInInventory;
			}

			inventoryUI.increaseNumberOfItems (slotNumber, amountInInventory);

			return true;
		}
	}
}
