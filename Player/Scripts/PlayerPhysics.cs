using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]

public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;

	public bool grounded;

	private BoxCollider2D boxCollider;
	private Vector2 playerSize;
	private Vector2 offset;

	private float tinyBuffer = .005f;

	Ray2D ray;
	RaycastHit2D hit;

	void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		playerSize = boxCollider.size*5;
		offset = boxCollider.offset;
	}

	public void Move(Vector2 moveAmount){

		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;

		grounded = false;
		for (int i = 0; i<3; i++){
			float dir  = Mathf.Sign(deltaY);
			float x = position.x + offset.x - playerSize.x/2 + playerSize.x/2* i ; // Side of collider
			float y = position.y + offset.y + playerSize.y/2 * dir; // Bottom of collider


			ray = new Ray2D(new Vector2(x,y), new Vector2(0,dir));

			Debug.DrawRay(ray.origin,ray.direction);
			
			if (hit = Physics2D.Raycast (ray.origin,ray.direction, Mathf.Abs(deltaY) + tinyBuffer, collisionMask)) {

				//Get distance btwn player and ground
				float dst = Vector2.Distance (ray.origin, hit.point);

				//Stop players y movement after coming withing tinyBuffer width of a collider
				if (dst > tinyBuffer) {
					deltaY = (moveAmount.y);

				} else{
					deltaY = 0;
				}
				grounded = true;
				break;
			}
		}

		Vector2 finalMajigger = new Vector2(deltaX,deltaY);

		transform.Translate(finalMajigger);
	}

}
