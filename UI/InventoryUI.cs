using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour {

    public Sprite blank, hotbar, fullInventory;
    public Text equippedName;
    public static GameObject[] images, imageNums;
    public static GameObject[,,] tileImages = new GameObject[81, 7, 7];
    private bool menuOpen = false;

    // Use this for initialization
    void Start() {

        //Keeps inventory when changing inbetween scenes
        DontDestroyOnLoad(GameObject.Find("UI"));
        DontDestroyOnLoad(GameObject.Find("Player"));

        // Initializing images
        ArrayList imagesTemp = new ArrayList();
        ArrayList imageNumsTemp = new ArrayList();

        GameObject image;
        GameObject imageNum;

        image = GameObject.Find("Inventory Image (0)");
        image.GetComponent<RectTransform>().sizeDelta.Set(24, 24);
        float width = image.GetComponent<RectTransform>().rect.width;
        float spacing = (GetComponent<RectTransform>().sizeDelta.x - image.GetComponent<RectTransform>().sizeDelta.x * 9) / 9f - 1f;

        float lastY = image.GetComponent<RectTransform>().localPosition.y;
        float lastX = image.GetComponent<RectTransform>().localPosition.x;
        float firstX = lastX;

        for (int i = 0 ; ; i++) {
            image = GameObject.Find("Inventory Image (" + i + ")");
            imageNum = GameObject.Find("Item Number (" + i + ")");

            // Add the number to the array and put the image into it's proper spot.
            if (image != null) {
                imagesTemp.Add(image);
                imageNumsTemp.Add(imageNum);
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
                image.GetComponent<RectTransform>().localPosition = new Vector3(lastX, lastY, 0);
                imageNum.transform.SetParent(image.transform);
                imageNum.transform.localPosition = new Vector2(6, -6);
                imageNum.GetComponent<RectTransform>().sizeDelta = new Vector2(12, 12);
            } else
                break;

            lastX += width + spacing;

            if ((i+1) % 9 == 0) {
                lastY -= width + spacing;
                lastX = firstX;
            }

            // Add the smaller sprite array for crafting from each image
            foreach (Transform t in image.GetComponentsInChildren<Transform>()) {
                if (!t.name.Contains("Pos")) {
                    continue;
                }

                string name = t.name.Replace("Pos (", "").Replace(")", "");
                int columnNum = int.Parse(name);

                name = t.parent.name.Replace("Row (", "").Replace(")", "");
                int rowNum = int.Parse(name);

                tileImages[i, rowNum, columnNum] = t.gameObject;
            }
        }

        images = new GameObject[imagesTemp.Count];
        for (int i = 0 ; i < imagesTemp.Count ; i++) {
            images[i] = (GameObject) imagesTemp.ToArray()[i];
        }

        imageNums = new GameObject[imageNumsTemp.Count];
        for (int i = 0 ; i < imageNumsTemp.Count ; i++) {
            imageNums[i] = (GameObject) imageNumsTemp.ToArray()[i];
        }
    }

    // Update is called once per frame
    void Update() {
        if (menuOpen && Inventory.equipped != null) {
            equippedName.text = Inventory.equipped.GetComponent<Item>().name;
        } else if (menuOpen) {
            equippedName.text = "";
        }
    }

    public void equip(int slotNumber, int oldEquipped) {
        
        for (int i = 0 ; i < 7 ; i++) {
            for(int j = 0 ; j < 7 ; j++) {
                tileImages[oldEquipped, i, j].GetComponent<SpriteRenderer>().color = Color.white;
                tileImages[slotNumber, i, j].GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        images[oldEquipped].GetComponent<Image>().color = Color.white;
        images[slotNumber].GetComponent<Image>().color = Color.green;
        return;
    }

    

    public void addItem(Sprite item, int inventoryPosition) {

        images[inventoryPosition].GetComponent<Image>().sprite = item;

        if (!menuOpen && inventoryPosition > 8)
            images[inventoryPosition].transform.localScale = new Vector2(0, 0);
    }

    public void addItem(SpriteRenderer[,] it, int inventoryPosition) {
        for (int i = 0 ; i < it.GetLength(0) ; i++) {
            for (int j = 0 ; j < it.GetLength(1) ; j++) { 
                tileImages[inventoryPosition, i, j].GetComponent<SpriteRenderer>().sprite = it[i, j].sprite;
            }
        }

        if (!menuOpen && inventoryPosition > 8)
            images[inventoryPosition].transform.localScale = new Vector2(0, 0);

    }


    public void changeNumOfItems(int slotNumber, int newSize) {
		if((!menuOpen && slotNumber > 8) || newSize == 1)
			imageNums [slotNumber].GetComponent<Text> ().text = "";
		else
			imageNums [slotNumber].GetComponent<Text> ().text = newSize + "";

		if (newSize == 0) {
			images[slotNumber].GetComponent<Image>().sprite = blank;
			imageNums [slotNumber].GetComponent<Text> ().text = "";
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
            images[i].transform.localScale = new Vector2(0, 0);
            imageNums[i].GetComponent<Text>().text = "";
		}

		GameObject saveWorldButton = GameObject.Find ("Menu Button");
		saveWorldButton.transform.localScale = new Vector3 (0, 0, 0);
        equippedName.text = "";
	}
	
	private void showInventory() {
		GetComponent<Image> ().sprite = fullInventory;

		for(int i = 9; i < images.Length; i++) {
            
            images[i].transform.localScale = new Vector2(1, 1);

			if(Inventory.itemAmount[i] != 0) {
				if(Inventory.itemAmount[i] != 1)
					imageNums[i].GetComponent<Text>().text = Inventory.itemAmount[i] + "";
				else
					imageNums[i].GetComponent<Text>().text = "";
			}
		}
		GameObject saveWorldButton = GameObject.Find ("Menu Button");
		saveWorldButton.transform.localScale = new Vector3 (1, 1, 0);

	}
}