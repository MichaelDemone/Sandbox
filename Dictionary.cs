using UnityEngine;
using System.Collections;

public class Dictionary : MonoBehaviour {

	public static Hashtable dictionary;
	public static bool initialized = false;

	public GameObject dirt, copper, collectable;

	// Use this for initialization
	void Start () {

		// Adding terrain
		dictionary = new Hashtable ();
		dictionary.Add ("Dirt", dirt);
		dictionary.Add ("Copper", copper);
		dictionary.Add ("Collectable", collectable);


		// Adding weapons


		// Adding armour


		// Adding crafting objects


	}

	public static GameObject get(string name) {
		return (GameObject)dictionary [name];

	}
		
}
