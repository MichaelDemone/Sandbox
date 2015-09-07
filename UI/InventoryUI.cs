using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	public Sprite blank, hotbar, fullInventory;
	
	private Toggle selected;
	private GameObject[] images, imageNums;
	private bool menuOpen = false;

	// Use this for initialization
	void Start () {

		// Initializing images
		ArrayList imagesTemp = new ArrayList ();
		GameObject image;
		for (int i = 0;;i++) {
			image = GameObject.Find("Inventory Image (" + i + ")");
			if(image != null) {
				imagesTemp.Add(image);
			}
			else
				break;
		}
		
		images = new GameObject[imagesTemp.Count];
		for (int i = 0; i < imagesTemp.Count; i++) {
			images[i] = (GameObject) imagesTemp.ToArray()[i];
		}
		
		// Initialize image numbers
		ArrayList imageNumsTemp = new ArrayList ();
		GameObject imageNum;
		for (int i = 0;;i++) {
			imageNum = GameObject.Find("Item Number (" + i + ")");
			if(imageNum != null) {
				imageNumsTemp.Add(imageNum);
				imageNum.GetComponent<Text>().text = "";
			}
			else
				break;
		}
		
		imageNums = new GameObject[imageNumsTemp.Count];
		for (int i = 0; i < imageNumsTemp.Count; i++) {
			imageNums[i] = (GameObject) imageNumsTemp.ToArray()[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (selected != null) {
			selected.isOn = true;
		}
	}

	public void equip(int slotNumber) {
		if(images[slotNumber].GetComponent<Image>().sprite.name.Equals("Clear")) {
			return;
		}
		images[slotNumber].GetComponent<Toggle>().isOn = true;
		return;
	}


	public void addItem(Sprite item, int inventoryPosition) {
		if(!menuOpen && inventoryPosition > 8)
			images[inventoryPosition].GetComponent<Image>().sprite = blank;
		else
			images[inventoryPosition].GetComponent<Image>().sprite = item;
	}

	public void changeNumOfItems(int slotNumber, int newSize) {
		if(!menuOpen && slotNumber > 8 || newSize == 1)
			imageNums [slotNumber].GetComponent<Text> ().text = "";
		else
			imageNums [slotNumber].GetComponent<Text> ().text = newSize + "";

		if (newSize == 0) {
			images[slotNumber].GetComponent<Image>().sprite = blank;
			imageNums [slotNumber].GetComponent<Text> ().text = "";
			ColorBlock cb2 = selected.colors;
			cb2.normalColor = Color.white;
			selected.colors = cb2;
			selected.isOn = false;
			selected = null;
		}

	}

	// Hiding and showing inventory
	public void showOrHideInventory() {
		if(menuOpen) {
			hideInventory();
			menuOpen = false;
		}
		else {
			showInventory();
			menuOpen = true;
		}		
	}
	
	private void hideInventory() {
		GetComponent<Image> ().sprite = hotbar;
		
		for (int i = 9; i < images.Length; i++) {
			images[i].GetComponent<Image>().sprite = blank;
			imageNums[i].GetComponent<Text>().text = "";
		}

		GameObject saveWorldButton = GameObject.Find ("Menu Button");
		saveWorldButton.transform.localScale = new Vector3 (0, 0, 0);
	}
	
	private void showInventory() {
		GetComponent<Image> ().sprite = fullInventory;

		for(int i = 9; i < images.Length; i++) {

			if(Inventory.inventory[i] != null)
				images[i].GetComponent<Image>().sprite = Inventory.inventory[i].GetComponent<Item>().inventorySprite;
			else
				images[i].GetComponent<Image>().sprite = blank;

			if(Inventory.itemAmount[i] != 0) {
				if(Inventory.itemAmount[i] == 1)
					imageNums[i].GetComponent<Text>().text = Inventory.itemAmount[i] + "";
				else
					imageNums[i].GetComponent<Text>().text = "";
			}
		}
		GameObject saveWorldButton = GameObject.Find ("Menu Button");
		saveWorldButton.transform.localScale = new Vector3 (1, 1, 0);

	}
	
	// Toggles, this is called from the inventory images
	void toggleChanged() {

		/*if (Inventory.equipped != null) {
			return;
		}*/

		for (int i = 0; i < images.Length; i++) {
			Toggle tog = images[i].GetComponent<Toggle>();
			if(tog.isOn && tog != selected) {
				
				if(!images[i].GetComponent<Image>().sprite.name.Equals("Clear")) {
					
					if(selected != null) {
						ColorBlock cb2 = selected.colors;
						cb2.normalColor = Color.white;
						selected.colors = cb2;
						selected.isOn = false;
					}
					
					Inventory.inventory[i].GetComponent<Item>().selectItem();
					
					// Make selected highlighted green
					ColorBlock cb = tog.colors;
					cb.normalColor = Color.green;
					tog.colors = cb;
					
					selected = tog;
					return;
				}
				Debug.Log("Tog is on");
			}
		}
	}
}