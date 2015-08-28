using UnityEngine;
using System.Collections;

public class BlobMovement : MonoBehaviour {

	public float speedX, speedY, hitForce;

	private float lastCallTime, jumpTime;
	private GameObject player;
	private Vector3 previousPosition;
	private bool goingRight = false, jumping = false, onSomething = false;


	// Use this for initialization
	void Start () {
		lastCallTime = Time.time;
		player = GameObject.Find ("Player");
		previousPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (!jumping && !onSomething) {
			return;
		}

		if (!jumping && Time.time - lastCallTime > 1) {
			previousPosition = approachPlayer (gameObject, player, previousPosition);
			lastCallTime = Time.time;
		} else if (!jumping) {

			if(goingRight) {
				Vector2 vel = GetComponent<Rigidbody2D>().velocity;
				GetComponent<Rigidbody2D>().velocity = new Vector2 (speedX, vel.y);
			}
			else {
				Vector2 vel = GetComponent<Rigidbody2D>().velocity;
				GetComponent<Rigidbody2D>().velocity = new Vector2 (-speedX, vel.y);
			}
		}

		if (jumping) {

			Vector2 vel = new Vector2(0,5);
			if(Time.time - jumpTime < 0.15f) {
				vel.x = 0;
				vel.y = speedY;
				GetComponent<Rigidbody2D>().velocity = vel;
				return;
			}

			if(goingRight) {
				vel.x = speedX; 
			}
			else {
				vel.x = -speedX;
			}

			GetComponent<Rigidbody2D>().velocity = vel;

			if(Time.time - jumpTime > 0.2f)
				jumping = false;
			return;
		}

		// Possibly implement super jump to get out of holes
	}

	public Vector2 approachPlayer(GameObject enemy, GameObject player, Vector3 previousEnemyPosition) {

		Vector2 velocity = new Vector2 ();
		Vector3 enemyPos = enemy.transform.position;
		// Find what diretion to go
		Vector2 spaceBetween = enemyPos - player.transform.position;

		// Go right
		if (spaceBetween.x < 0) {
			velocity.x = speedX;
			goingRight = true;
		}
		// Go left
		else { 
			velocity.x = -speedX;
			goingRight = false;
		}

		if ((previousEnemyPosition - enemyPos).magnitude < 1) {
			jumping = true;
			jumpTime = Time.time;
		} else
			jumping = false;

		enemy.GetComponent<Rigidbody2D> ().velocity = velocity;

		return enemyPos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		onSomething = true;
	}

	void OnTriggerStay2D(Collider2D other) {
		onSomething = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		onSomething = false;
	}

	void OnCollisionEnter2D(Collision2D other) {
		onSomething = true;
		if(other.collider.CompareTag("Player")) {
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2 (hitForce, 0));
		}
	}
	
	void OnCollisionStay2D(Collision2D other) {
		onSomething = true;
		if(other.collider.CompareTag("Player")) {
		}
	}

}
