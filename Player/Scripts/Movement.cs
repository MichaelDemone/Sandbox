using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float acceleration, currentSpeed;
	private float jumpSpeed = 5;
	private bool grounded = false, lookingRight = true;
	private GameObject inventoryUI;
	private float onSomethingTest, recoveryTime;
    private Rigidbody2D r;

	// Use this for initialization
	void Start () {
		inventoryUI = GameObject.Find ("Inventory");
		onSomethingTest = Time.time;
        r = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {

        // Check user inputs
        checkInputs();

        // Check to see if game thinks you are grounded when you are not (incase a on trigger
        // exit call was missed)
		if (grounded && Time.time - onSomethingTest > 0.25) {
			OnTriggerExit2D(null);
			onSomethingTest = Time.time;
		}

	}
	
    private void checkInputs() {
        Vector2 speed = r.velocity;

        // Check X movement
        if (Input.GetKey(KeyCode.A)) {
            speed.x = Time.time > recoveryTime ? -5 : speed.x;

            // Change direction
            if (lookingRight) {
                lookingRight = false;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

        } else if (Input.GetKey(KeyCode.D)) {

            speed.x = Time.time > recoveryTime ? 5 : speed.x;

            // Change direction
            if (!lookingRight) {
                lookingRight = true;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        } else {
            speed.x = Time.time > recoveryTime ? 0 : speed.x;
        }

        // Check y movement
        if (Input.GetKey(KeyCode.Space) && grounded) {
            speed.y = jumpSpeed;
        }

        r.velocity = speed;

        // Set animator based on new speed
        GetComponent<Animator>().SetBool("moving", Mathf.Abs(speed.x) > 1);

        // Check inventory
        if (Input.GetKeyDown(KeyCode.Escape)) {
            inventoryUI.GetComponent<InventoryUI>().showOrHideInventory();
        }

        // Check combat
        if(Input.GetMouseButton(0)) {
            GetComponentInChildren<equippedWeapon>().attack();
        }

    }

    public void hit(float speedX, float recoveryTime) {
        r.velocity = new Vector2(speedX, r.velocity.y);
        this.recoveryTime = recoveryTime + Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        grounded = !other.isTrigger || grounded;
	}
	
	private void OnTriggerStay2D(Collider2D other) {
        grounded = !other.isTrigger || grounded;
	}

	private void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}
}
