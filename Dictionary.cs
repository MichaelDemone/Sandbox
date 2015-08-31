using UnityEngine;
using System.Collections;

public class Dictionary : MonoBehaviour {

	public static Hashtable dictionary;
	public static bool initialized = false;

	public GameObject dirt, copper, collectable, dirtWGrass;

	// Use this for initialization
	void Start () {

		// Adding terrain
		dictionary = new Hashtable ();
		dictionary.Add ("Dirt", dirt);
		dictionary.Add ("Copper", copper);
		dictionary.Add ("Collectable", collectable);
		dictionary.Add ("DirtWGrass", dirtWGrass);

		// Adding weapons


		// Adding armour


		// Adding crafting objects


	}

	public static GameObject get(string name) {
		return (GameObject)dictionary [name];

	}
		
}
