using UnityEngine;
using System.Collections;

public class BlobMovement : MonoBehaviour {

	public float speedX, speedY, hitForce;

	private GameObject player;

	private bool goingRight = false, jumping = false, onSomething = false;

    private Rigidbody2D r;

    private float recoverTime;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
        r = GetComponent<Rigidbody2D>();
        recoverTime = Time.time;
        StartCoroutine(runAI());

	}

    void Update() {
        if (Time.time > recoverTime) {
            int mod = goingRight ? 1 : -1;
            r.velocity = new Vector2(mod*speedX, r.velocity.y);
        }
    }

    IEnumerator runAI() {
        yield return new WaitForSeconds(Random.value*Tweakables.AI_DELAY);

        while (true) {

            // Do whatcha gotta do
            intelligence();

            //Check player distance
            bool nearPlayer = (player.transform.position - transform.position).magnitude < 5;

            // Check time until next delay
            float delay = Tweakables.AI_DELAY;

            delay = nearPlayer ? delay /=2 : delay *=2;

            yield return new WaitForSeconds(delay);
        }
    }

    void intelligence() {

        // Find and approach player
        Vector3 pos = transform.position;

        // Find what diretion to go
        Vector2 spaceBetween = pos - player.transform.position;
        goingRight = spaceBetween.x < 0;

        // Check to see if it is running into a wall
        bool shouldJump = checkHittingWall();

        if (shouldJump) {
            r.velocity = new Vector2(0, speedY);
        }

        // Possibly implement super jump to get out of holes
    }

    
    public void hit(int speedX, int damage, int stunTime) {

        if (recoverTime > Time.time)
            return;

        r.velocity = new Vector2(speedX, r.velocity.y);
        recoverTime = stunTime + Time.time;
        GetComponent<EnemyStats>().health -= damage;
        if(GetComponent<EnemyStats>().health <= 0) {
            Destroy(this.gameObject, 1);
        }
    }

    public bool checkHittingWall() {
        Vector2 p = transform.position;

        int mod = goingRight ? 1 : -1;

        Collider2D col = Physics2D.OverlapCircle(new Vector2(p.x+mod, p.y), 0.2f);

        return col != null && !col.isTrigger;
    }

    /*********** TRIGGERS FOR SEEING IF IT'S ON SOMETHING ************************/
    void OnTriggerEnter2D(Collider2D other) {
        onSomething = !other.isTrigger || onSomething;
	}

	void OnTriggerStay2D(Collider2D other) {
		onSomething = !other.isTrigger || onSomething;
	}

	void OnTriggerExit2D(Collider2D other) {
		onSomething = false;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.collider.CompareTag("Player")) {
            int mod = goingRight ? 1 : -1;
            
            player.GetComponent<Movement>().hit(mod*speedX*2, 0.5f);
		}
	}
}
