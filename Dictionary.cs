using UnityEngine;
using System.Collections;

public class Dictionary : MonoBehaviour {

	public static Hashtable dictionary;
	public static bool initialized = false;

	public GameObject dirt, copper, collectable, dirtWGrass, sand, leaves, log, torch, light;

	// Use this for initialization
	void Start () {

		// Adding terrain
		dictionary = new Hashtable ();
		dictionary.Add ("Dirt", dirt);
		dictionary.Add ("Copper", copper);
		dictionary.Add ("Collectable", collectable);
		dictionary.Add ("DirtWGrass", dirtWGrass);
		dictionary.Add ("Sand", sand);
		dictionary.Add ("Log", log);
		dictionary.Add ("Leaves", leaves);
		dictionary.Add ("Torch", torch);
		dictionary.Add ("Light", light);

		// Adding weapons


		// Adding armour


		// Adding crafting objects


	}

	public static GameObject get(string name) {
		return (GameObject)dictionary [name];

	}
		
}
