using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float acceleration, currentSpeed;
	private float targetSpeed = 5;
	private float jumpSpeed = 5;
	private bool grounded = false;
	private GameObject inventoryUI;
	private float onSomethingTest;
	// Use this for initialization
	void Start () {
		inventoryUI = GameObject.Find ("Inventory");
		onSomethingTest = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 speed = new Vector2 (this.GetComponent<Rigidbody2D> ().velocity.x, this.GetComponent<Rigidbody2D> ().velocity.y);
		if (Input.GetKey (KeyCode.A)) {
			speed.x = IncrementTowards(speed.x, -1*targetSpeed, acceleration);
		}
		if (Input.GetKey(KeyCode.D)) {
			speed.x = IncrementTowards(speed.x, targetSpeed, acceleration);
		}

		if (Input.GetKey(KeyCode.Space) && grounded) {
			speed.y = jumpSpeed;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			inventoryUI.GetComponent<InventoryUI>().showOrHideInventory();
		}

		this.GetComponent<Rigidbody2D> ().velocity = speed;

		if (Input.GetMouseButton(0)){
			GetComponent<Animator>().SetBool( "mining", true );
		}else{
			GetComponent<Animator>().SetBool( "mining", false );
		}

		if (Time.time - onSomethingTest > 0.25) {
			OnTriggerExit2D(null);
			onSomethingTest = Time.time;
		}

	}

	//Increase n towards target speeeeed
	private float IncrementTowards(float currentSpeed, float targetSpeed, float acceleration) {

		if (currentSpeed == targetSpeed) {
			return currentSpeed; //return if at target
		} else {
			float dir = Mathf.Sign (targetSpeed - currentSpeed); // which way are we increasing?
			currentSpeed += acceleration * Time.deltaTime * dir; //inc speed, accel * direction
			return (dir == Mathf.Sign(targetSpeed - currentSpeed))? currentSpeed: targetSpeed;// if n is passed target return target
		}
		
	}
	
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
