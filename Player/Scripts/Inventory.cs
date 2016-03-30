using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {


	public static bool hitting = true, equippedIsForPlacing = false;
	public static GameObject equipped;
	public static int equippedNum, lastEquipped;
	public static GameObject[] inventory;
	public static int[] itemAmount;

	private GameObject tileParent;
	private float placeTime;
	private KeyCode[] quickslots;

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

		quickslots = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
			, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};

		placeTime = Time.time;
		inventoryUI = GameObject.Find ("Inventory").GetComponent<InventoryUI>();
		tileParent = GameObject.Find ("Ground");

	}
	
	// Update is called once per frame
	void Update () {

        // Check to see if the player has switched weapons
		for (int i = 0; i < quickslots.Length; i++) {
			if(Input.GetKeyDown(quickslots[i]) && inventory[i] != null) {
                
                if (equipped != null && equipped.GetComponent<usableItem>() != null) {
                    GetComponentInChildren<equippedWeapon>().unequip();
                }

                inventoryUI.equip(i, lastEquipped);
                
				equipped = inventory[i];
				equippedNum = i;
                lastEquipped = equippedNum;
                if (equipped.GetComponent<usableItem>() != null) {
                    GetComponentInChildren<equippedWeapon>().equip(equipped.GetComponent<usableItem>());
                    equippedIsForPlacing = false;
                } else {
                    equippedIsForPlacing = true;
                }
            }
		}

        // Check to see if the player is trying to place a block within range
		if (Input.GetMouseButton (1) && equippedIsForPlacing && Time.time - placeTime > 0.25) {
			placeItem();
		}
	}

    /// <summary>
    /// Add an item using a collectable (dropped by blocks)
    /// </summary>
    /// <param name="item"></param>
    /// <param name="collectable"></param>
	public static void addItem(GameObject item, GameObject collectable) {

		if(addItem(item, collectable.GetComponent<Collect>().amount)) {
            Destroy(collectable);
        }
	}


    public static bool addItem(GameObject item, int amount) {
        bool foundEmpty = false;
        int emptyPosition = -1;

        // Go through inventory
        for (int i = 0 ; i < INVENTORY_SIZE ; i++) {

            // If item in slot equals current item, increase amount of it
            if (inventory[i] != null && inventory[i].Equals(item)) {

                // If it can't be increased, keep going until a new 
                // open spot is found
                amount = increaseNumberOfItems(item, amount, i);
                if (amount == 0) {
                    return true;
                }
            }
            // Records the first empty spot
            else if (!foundEmpty && inventory[i] == null) {
                foundEmpty = true;
                emptyPosition = i;
            }
        }

        // Puts item into first empty spot
        if (foundEmpty) {
            Sprite inventorySprite = item.GetComponent<Item>().inventorySprite;
            if(inventorySprite != null)
                inventoryUI.addItem(inventorySprite, emptyPosition);
            else if (item.GetComponent<usableItem>() != null) {
                inventoryUI.addItem(item.GetComponent<usableItem>().sprites, emptyPosition);
            }

            inventory[emptyPosition] = item;
            increaseNumberOfItems(item, amount, emptyPosition);

            DontDestroyOnLoad(item);

            return true;
        }
        return false; // Don't destroy collectable because it was not added to inventory.
    }

    /// <summary>
    /// Increase number of "item" determined by the number within the collectable
    /// </summary>
    /// <param name="item"></param>
    /// <param name="collectable"></param>
    /// <param name="slotNumber"></param>
    /// <returns></returns>
	private static int increaseNumberOfItems(GameObject item, int amount, int slotNumber) {

		int maxStackSize = item.GetComponent<Item> ().maxStackSize;

        // Check to see if the item has gone above it's max amount
		if (itemAmount [slotNumber] >= maxStackSize) {
			return amount;
		}  else {

			int amountInInventory = itemAmount[slotNumber];

            // If the amount of blocks in collectable is more than the stack size,
            // change the amount in the collectable and run the add the collectable
            // to inventory again
			if(amount + amountInInventory > maxStackSize) {
				int amountBeingAdded = maxStackSize - amountInInventory;
				amount -= amountBeingAdded;
				itemAmount[slotNumber] += amountBeingAdded;
				addItem(item, amount);

			} else {
				itemAmount[slotNumber] += amount;
                amount = 0;
			}

			inventoryUI.changeNumOfItems (slotNumber, itemAmount[slotNumber]);

			return amount;
		}
	}

    /// <summary>
    /// As the name implies, reduces the equipped item number by one
    /// </summary>
    public static void reduceEquippedByOne() {
        if (itemAmount[equippedNum] == 0) {
            return;
        }
        itemAmount[equippedNum] -= 1;
        inventoryUI.changeNumOfItems(equippedNum, itemAmount[equippedNum]);
        if (itemAmount[equippedNum] == 0) removeItem();
    }

    /// <summary>
    /// Removes equipped item and resets all variables
    /// </summary>
	private static void removeItem() {
        if (Inventory.equipped != null && Inventory.equipped.GetComponent<Item>().name.Equals("Torch")) {
            Destroy(GameObject.Find("Player").GetComponentInChildren<Light>().gameObject);
        }

        inventory[equippedNum] = null;
        equipped = null;
        equippedIsForPlacing = false;
        lastEquipped = equippedNum;
        equippedNum = -1;
    }
    /// <summary>
    /// Place the currently equipped item at the mouse position
    /// </summary>
	private void placeItem() {

        Vector3 pos = Input.mousePosition;
        pos.z = 25;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        Collider2D hit = Physics2D.OverlapPoint(pos);

        if (hit != null && !hit.isTrigger) {
            return;
        }

        if (CreateMap.map.Contains(pos)) return;

        // Place object
        GameObject go = (GameObject) GameObject.Instantiate(equipped, pos, Quaternion.identity);

        // Add to map of place
        CreateMap.map.Add(pos, go.GetComponent<Item>().name);

        reduceEquippedByOne();


        if (go.CompareTag("Tile")) {
            go.transform.parent = tileParent.transform;
        } else if (go.CompareTag("Light")) {
            go.GetComponentInChildren<LightSource>().lightUp();
        }
    }
}
