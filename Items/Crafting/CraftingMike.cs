using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingMike : MonoBehaviour {

    Collider2D[] childrenColliders;
    Item[,] items;

    public bool settingHold = false;

	// Use this for initialization
	void Start () {
        childrenColliders = GetComponentsInChildren<Collider2D>();
        items = new Item[7, 7];
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)) {

            // Find mouse position in world
            Vector2 mosPos = Input.mousePosition;
            mosPos = Camera.main.ScreenToWorldPoint(mosPos);

            // Find colliders that the mouse is colliding with
            Collider2D[] r = Physics2D.OverlapPointAll(mosPos);

            // If none are found, return
            if (r == null) return;

            foreach(Collider2D hit in r) {
                if (hit.GetComponent<Toggle>() != null || !hit.name.Contains("Image ("))
                    continue;
                putItemInSpot(hit);
                return;

            }
        }
	}

    private void putItemInSpot(Collider2D hit) {

        // Find column number through name
        string name = hit.name.Replace("Image (", "");
        name = name.Replace(")", "");
        int columnNum = int.Parse(name);

        // Find row number through parents name
        name = hit.transform.parent.name;
        name = name.Replace("row (", "");
        name = name.Replace(")", "");
        int rowNum = int.Parse(name);


        if (items[rowNum, columnNum] != null || Inventory.equipped == null || Inventory.equipped.GetComponent<usableItem>() != null) {
            if (settingHold) {
                GameObject.Find("Button Manager").GetComponent<buttonManager>().create(rowNum, columnNum);
            }
            return;
        }

        // Get equipped item and change sprite
        Item it = Inventory.equipped.GetComponent<Item>();
        hit.GetComponent<Image>().sprite = it.inventorySprite;

        // Set item and item array
        items[rowNum, columnNum] = it;
        Inventory.reduceEquippedByOne();
    }

    public void returnItems() {

        // Clear the items array
        for (int i = 0 ; i < items.GetLength(0); i++) {
            for (int j = 0 ; j < items.GetLength(1) ; j++) {
                if (items[i, j] != null) {
                    Inventory.addItem(items[i, j].gameObject, 1);
                    items[i, j] = null;
                }
            }
        }

        // Visually clear the array
        foreach(Collider2D col in childrenColliders) {
            col.GetComponent<Image>().sprite = null;
        }
    }

    public void craftItem(string name, int xHolding, int yHolding) {

        settingHold = false;

        GameObject usable = GameObject.Find("Player").GetComponentInChildren<usableItem>().gameObject;
        GameObject go = (GameObject) GameObject.Instantiate(usable, new Vector2(2000, 2000), Quaternion.identity);

        go.GetComponent<usableItem>().setValues(items, name, 1, 1, 1, 1, xHolding, yHolding);

        // Remove items
        for (int i = 0 ; i < items.GetLength(0) ; i++) {
            for (int j = 0 ; j < items.GetLength(1) ; j++) {
                items[i, j] = null;
            }
        }

        // Visually clear the array
        foreach (Collider2D col in childrenColliders) {
            col.GetComponent<Image>().sprite = null;
        }
    }
}
