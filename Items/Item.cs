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

		// If this is a recently placed tile, check the distance between each light source,
		// and if that distance is within the light up distance, light up!
		if (CompareTag ("Tile") && Tweakables.darknessActivated) {
			GetComponent<SpriteRenderer>().color = Color.black;
			foreach(GameObject go in LightSource.lightSources) {
				if((go.transform.position - transform.position).magnitude < go.GetComponent<LightSource>().blockRange) {
					go.GetComponent<LightSource>().lightUp();
				}
			}

			// Throw raycasts to see if wall exists and if above a certain height, if so light up

			// Put raycasts in the up direction to see if there is a clear shot to the sky, if so
			// make the block bright

			// If tiles are around this and there is no wall, still have it black

			// once a wall is destroyed make it act as a light source above a certain point.
		}
	}


	public void selectItem() {
		if (Inventory.equipped != null && Inventory.equipped.GetComponent<Item> ().CompareTag("Light")) {
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

		if (CompareTag("Light")) {
			GameObject lightPrefab = Dictionary.get("Torch").GetComponentInChildren<Light>().gameObject;
			GameObject light = (GameObject) GameObject.Instantiate(lightPrefab);
			light.transform.SetParent(GameObject.Find("Player").transform);
			light.transform.localPosition = new Vector3(0.1f,0,-1);
			light.GetComponent<LightSource>().onPlayer = true;
		}
	}

	public void drop() {

		if(invName.Equals(""))
			return;

		GameObject collectable = (GameObject)Dictionary.get ("Collectable");
		
		collectable.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
		collectable.GetComponent<Collect> ().objectThisRepresents = Dictionary.get (invName);
		GameObject.Instantiate (collectable, transform.position, Quaternion.identity);
	}



}
