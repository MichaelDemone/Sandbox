using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float acceleration, currentSpeed;
	private float jumpSpeed = 5;
	private bool grounded = false, lookingRight = true;
	private GameObject inventoryUI;
	private float onSomethingTest;

	// Use this for initialization
	void Start () {
		inventoryUI = GameObject.Find ("Inventory");
		onSomethingTest = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
		//lets see if the player is in the BuildingBuilding? if so, set default values and remove gravity!
		if (Application.loadedLevel == 2){

			GameObject Player = GameObject.Find("Player");
			this.GetComponent<Rigidbody2D>().isKinematic = true;
			Vector3 position = new Vector3(4.5f,6f,0);
			Vector3 scale = new Vector3(4,4,1);
			this.transform.localScale = scale;
			this.transform.position = position;

		}


		Vector2 speed = new Vector2 (this.GetComponent<Rigidbody2D> ().velocity.x, this.GetComponent<Rigidbody2D> ().velocity.y);

		if (Input.GetKey (KeyCode.A)) {
			speed.x = -5;

			// Change direction
			if(lookingRight) {
				lookingRight = false;
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}

		} else if (Input.GetKey(KeyCode.D)) {
			speed.x = 5;

			// Change direction
			if(!lookingRight) {
				lookingRight = true;
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else{
			speed.x = 0;
		}

		if (Input.GetKey(KeyCode.Space) && grounded) {
			speed.y = jumpSpeed;
		}

		this.GetComponent<Rigidbody2D> ().velocity = speed;
		GetComponent<Animator>().SetBool("moving",Mathf.Abs(speed.x) > 1);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			inventoryUI.GetComponent<InventoryUI>().showOrHideInventory();
		}

		if (Time.time - onSomethingTest > 0.25) {
			OnTriggerExit2D(null);
			onSomethingTest = Time.time;
		}

	}

	//Increase n towards target speeeeed
	/*private float IncrementTowards(float currentSpeed, float targetSpeed, float acceleration) {

		if (currentSpeed == targetSpeed) {
			return currentSpeed; //return if at target
		} else {
			float dir = Mathf.Sign (targetSpeed - currentSpeed); // which way are we increasing?
			currentSpeed += acceleration * Time.deltaTime * dir; //inc speed, accel * direction
			return (dir == Mathf.Sign(targetSpeed - currentSpeed))? currentSpeed: targetSpeed;// if n is passed target return target
		}
		
	}*/
	
	private void OnTriggerEnter2D(Collider2D other) {
		if(!other.isTrigger)
			grounded = true;
	}
	
	private void OnTriggerStay2D(Collider2D other) {
		if(!other.isTrigger)
			grounded = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}






}
