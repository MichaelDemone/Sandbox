﻿using UnityEngine;
using System.Collections;

public class Collect: MonoBehaviour {

	public GameObject objectThisRepresents;
	public int amount = 1;

	private bool registered = false;
	bool beingDestroyed = false;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player") && !registered) {
			registered = true;
			Inventory.addItem(objectThisRepresents, gameObject);
		}
		if (other.GetComponent<Collect> () != null) {
			bool sameType = other.GetComponent<Collect> ().objectThisRepresents == objectThisRepresents;
			if (!beingDestroyed && other.CompareTag ("Collectable") && sameType) {
				other.GetComponent<Collect> ().SendMessage ("BeingDestroyed");
				amount += other.GetComponent<Collect> ().amount;
				GameObject.Destroy (other.gameObject);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag ("Player") && !registered) {
			registered = true;
			Inventory.addItem(objectThisRepresents, gameObject);
		}
	}

	void BeingDestroyed() {
		beingDestroyed = true;
	}
}
