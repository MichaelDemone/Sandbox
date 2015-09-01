using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerControl : MonoBehaviour {


	//Player handling
	public float gravity = 9.8f;
	public float speed = 3;
	public float acceleration = 2;
	public float jumpHeight = 10;

	public bool goingRight;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	// Update is called once per frame
	void Update () {

		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		if (playerPhysics.grounded){
			amountToMove.y = 0;

			//Jump
			if (Input.GetButtonDown("Jump")){
				amountToMove.y = jumpHeight;
			}
		}else{
			amountToMove.y = -1*gravity;
		}

		amountToMove.x = currentSpeed;

		playerPhysics.Move (amountToMove * Time.deltaTime);
	}

	//Increase n towards target speeeeed
	private float IncrementTowards(float currentSpeed, float targetSpeed, float acceleration) {

		if (currentSpeed == targetSpeed) {
			return currentSpeed; //return if at target
		} else {
			float dir = Mathf.Sign (targetSpeed - currentSpeed); // which way are we increasing?
			currentSpeed += acceleration * Time.deltaTime * dir; //inc speed, accel * direction

			if (dir > 0f){
				goingRight = true;
			}else{
				goingRight = false;
			}

			return (dir == Mathf.Sign(targetSpeed - currentSpeed))? currentSpeed: targetSpeed;// if currentSpeed is passed targetspeed return targetspeed
		}


	}


}
