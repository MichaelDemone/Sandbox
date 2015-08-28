using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	

	public Sprite dirt, copper, blank, hotbar, fullInventory, woodenPickaxe;
	public GameObject dirtObj, copperObj, woodenPickaxeObj;
	public bool hitting = true, placing = false;

	private string[] information;
	private int[] intInfo;
	private GameObject[] images, imageNums;
	private bool menuOpen = false, placingObject = false;
	private float placeTime;

	private GameObject equipped;
	private Image[] images2;
	private Toggle[] toggles;
	private Toggle selected;
	private KeyCode[] codes;
	private string[] placeables = new string[] {"Dirt", "Copper"};
	private string[] equipables = new string[] {"Wooden Pickaxe"};

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

		// Initializing information about what is in inventory
		information = new string[images.Length];
		for (int i = 0; i < information.Length; i++) {
			information[i] = "";
		}

		// Initializing information about how many items are in each
		intInfo = new int[images.Length];
		for (int i = 0; i < information.Length; i++) {
			intInfo[i] = 0;
		}

		// Toggles and images
		images2 = GameObject.Find ("Inventory Images").GetComponentsInChildren<Image> ();
		toggles = GameObject.Find ("Inventory Images").GetComponentsInChildren<Toggle> ();

		codes = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
			, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};

		addItem ("Wooden Pickaxe");
		toggles [0].isOn = true;

		placeTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (selected != null) {
			selected.isOn = true;
		}

		for (int i = 0; i < codes.Length; i++) {
			if(Input.GetKeyDown(codes[i])) {
				if(images2[i].sprite.name.Equals("Clear")) {
					return;
				}
				toggles[i].isOn = true;
				return;
			}
		}

		if (placing && Input.GetMouseButton (0) && !placingObject) {
			placingObject = true;

			Vector3 pos = Input.mousePosition;
			pos.z = -25;
			pos.x = Screen.width - pos.x;
			pos.y = Screen.height - pos.y;
			pos = Camera.main.ScreenToWorldPoint(pos);
			//RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, pos - Camera.main.transform.position);
			RaycastHit2D hit = Physics2D.Raycast(pos, new Vector3(0,0,1));

			if(hit.collider != null && hit.collider.tag.Equals("Tile")) {
				Debug.Log(hit.collider.name);
			}
			else {
				int xPos = Mathf.RoundToInt (pos.x);
				int yPos = Mathf.RoundToInt (pos.y);

				GameObject.Instantiate (equipped, new Vector3(xPos, yPos, 0), Quaternion.identity);
			}
		} else if (placingObject && Time.time - placeTime > 0.25) {
			placingObject = false;
		}

	}

	private void equipItem(string name) {
		Debug.Log ("Equipping: " + name);
		for (int i = 0; i < equipables.Length; i++) {
			if(equipables[i].Equals(name)) {
				hitting = true;
				placing = false;
				equipped = getGameObject(name);
				return;
			}
		}
		for (int i = 0; i < placeables.Length; i++) {
			if(placeables[i].Equals(name)) {
				placing = true;
				hitting = false;
				equipped = getGameObject(name);
				return;
			}
		}
		Debug.Log ("No equipable object: " + name);
	}

	public void addItem(string name) {
		for(int i = 0; i < images.Length; i++) {
			if(information[i].Equals(name)){
				increaseNumberOfItems(name, i);
				return;
			}
			else if(information[i].Equals("")) {
				if(!menuOpen && i > 9)
					images[i].GetComponent<Image>().sprite = blank;
				else
					images[i].GetComponent<Image>().sprite = getSprite(name);
				information[i] = name;
				increaseNumberOfItems(name, i);
				return;
			}
		}
	}

	private void increaseNumberOfItems(string name, int num) {
		intInfo [num]++;
		if(!menuOpen && num > 9)
			imageNums [num].GetComponent<Text> ().text = "";
		else
			imageNums [num].GetComponent<Text> ().text = intInfo [num] + "";
	}

	private Sprite getSprite(string name) {
		if (name.Equals ("Copper")) {
			return copper;
		} else if (name.Equals ("Dirt")) {
			return dirt;
		} else if (name.Equals ("")) {
			return blank;
		} else if (name.Equals ("Wooden Pickaxe")) {
			return woodenPickaxe;
		} else {
			Debug.Log("Name: " + name + " could not be found");
			return blank;
		}
	}

	private GameObject getGameObject(string name) {
		if (name.Equals ("Copper")) {
			return copperObj;
		} else if (name.Equals ("Ground")) {
			return dirtObj;
		} else if (name.Equals ("Wooden Pickaxe")) {
			return woodenPickaxeObj;
		} else {
			Debug.Log("Name: " + name + " could not be found for equip");
			return null;
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
	}

	private void showInventory() {
		GetComponent<Image> ().sprite = fullInventory;

		for(int i = 9; i < images.Length; i++) {
			images[i].GetComponent<Image>().sprite = getSprite(information[i]);
			if(intInfo[i] != 0)
				imageNums[i].GetComponent<Text>().text = intInfo[i] + "";
		}
	}

	// Toggles, this is called from the inventory images
	void toggleChanged() {

		for (int i = 0; i < images2.Length; i++) {
			if(toggles[i].isOn && toggles[i] != selected) {

				if(!images2[i].sprite.name.Equals("Clear")) {

					if(selected != null) {
						ColorBlock cb2 = selected.colors;
						cb2.normalColor = Color.white;
						selected.colors = cb2;
						selected.isOn = false;
					}

					equipItem(images2[i].sprite.name);
					
					// Make selected highlighted green
					ColorBlock cb = toggles[i].colors;
					cb.normalColor = Color.green;
					toggles[i].colors = cb;

					selected = toggles[i];
					return;
				}
			}
		}
	}

}
