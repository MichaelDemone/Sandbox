using UnityEngine;
using System.Collections;

public class CreateMap : MonoBehaviour {
	
	public static Hashtable map;//the map
	public int seed = 1234;
	
	protected static GameObject player, ground;
	protected static float maxY = 0, minY = 0;
	protected static float previousYPosR = 0, previousYPosL = 0;
	protected static float widthOfGroundPiece = 1;
	protected const float MINIMUM_Y = -1000, MINIMUM_GROUND_Y = 0;
	protected static int distanceBetweenLoads = 1;
	private int lastXTransition = 0, lastYTransition = 0;
	private bool rightLastTransition = true, upLastTransition = false;
	private const int WIDTH_OF_BOXES = 20, HEIGHT_OF_BOXES = 60;
	private const int DIST_TO = 30;
	
	// Use this for initialization
	void Start () {

		ground =  GameObject.Find("Ground");
		player = GameObject.Find ("Player");
		lastXTransition = Mathf.RoundToInt(player.transform.position.x);
		lastYTransition = Mathf.RoundToInt(player.transform.position.y);

		WalkingGeneration.cameraSizeX = 2*Camera.main.orthographicSize;
		WalkingGeneration.cameraSizeY = Camera.main.orthographicSize;

		player.transform.position = new Vector3 (0, player.transform.position.y);
		if (map == null) {
			
			// Put the correct data in the map

			map = new Hashtable ();
			Random.seed = seed;
			
			Vector3 position = new Vector3();
			// This is the starting position
			previousYPosR = 10;
			previousYPosL = 10;
			
			// Create map from -1000 to 1000
			for (float i = -1000; i < 1000; i += widthOfGroundPiece) {

				// Pick whether the block goes up or down
				previousYPosR += selectDirection();
				
				for (float j = -20; j < previousYPosR; j += widthOfGroundPiece) {
					position = new Vector3 (i, j, 0);
					map.Add (position, "Dirt");
				}
				
				// Put grassy block on top
				map.Remove (position);
				map.Add (position, "DirtWGrass");
				
				if (previousYPosR > maxY) maxY = previousYPosR;
			}

		} else {
			Debug.Log("Loading map");
			maxY = 100;
		}

		player.transform.position = new Vector3 (10, maxY);
		float x = Mathf.RoundToInt(player.transform.position.x);
		float y = Mathf.RoundToInt (player.transform.position.y);
		for (float i = x - 50; i < x + 50; i += widthOfGroundPiece) 
			for (float j = y -50; j < y + 50; j += widthOfGroundPiece)
				WalkingGeneration.loadPeice (i, j, 0);

		GenerateTrees.placeTrees((int) maxY, 1.95f);

		for (float i = -2*distanceBetweenLoads + widthOfGroundPiece; i < distanceBetweenLoads; i += widthOfGroundPiece){
			for (float j = minY; j < maxY; j += widthOfGroundPiece){
				WalkingGeneration.loadPeice(i,j,0);

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		int xPos = Mathf.RoundToInt (player.transform.position.x);
		int yPos = Mathf.RoundToInt (player.transform.position.y);
		bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;
		bool goingUp = player.GetComponent<Rigidbody2D>().velocity.y > 0;

		// Check if player should load in the X direction
		if (xPos > lastXTransition + distanceBetweenLoads || xPos < lastXTransition - distanceBetweenLoads) {
			
			for(int i = lastXTransition; i < xPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = i;
			}

			for(int i = lastXTransition; i > xPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(i, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = i;
			}
			
			if (xPos != lastXTransition || rightLastTransition != goingRight) {
				WalkingGeneration.loadPeices(xPos, yPos, goingRight, goingUp, true, false);
				rightLastTransition = goingRight;
				lastXTransition = xPos;
			}
		}
		
		// Check if player should load in the X direction
		else if (yPos > lastYTransition + distanceBetweenLoads || yPos < lastYTransition - distanceBetweenLoads) {

			
			for(int i = lastYTransition; i < yPos; i += distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = i;
			}
			for(int i = lastYTransition; i > yPos; i -= distanceBetweenLoads) {
				WalkingGeneration.loadPeices(xPos, i, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = i;
			}
			
			if (yPos != lastYTransition || goingUp != upLastTransition) {
				WalkingGeneration.loadPeices(xPos, yPos, goingRight, goingUp, false, true);
				upLastTransition = goingUp;
				lastYTransition = yPos;
			}
		}


		Collider2D[] collidersY;
		Collider2D[] collidersX;
		//Debug.DrawLine (new Vector2 (xPos + HEIGHT_OF_BOXES / 2, yPos + DIST_TO + WIDTH_OF_BOXES), new Vector2(xPos - HEIGHT_OF_BOXES/2, yPos + DIST_TO));
		//Debug.DrawLine (new Vector2(xPos + HEIGHT_OF_BOXES/2, yPos - DIST_TO - WIDTH_OF_BOXES), new Vector2(xPos - HEIGHT_OF_BOXES/2, yPos - DIST_TO));
		//Debug.DrawLine (new Vector2(xPos + DIST_TO, yPos - HEIGHT_OF_BOXES/2), new Vector2(xPos + DIST_TO + WIDTH_OF_BOXES, yPos + HEIGHT_OF_BOXES/2));
		//Debug.DrawLine (new Vector2(xPos - DIST_TO, yPos - HEIGHT_OF_BOXES/2), new Vector2(xPos - DIST_TO - WIDTH_OF_BOXES, yPos + HEIGHT_OF_BOXES/2));

		if(!goingUp)
			collidersY = Physics2D.OverlapAreaAll(new Vector2(xPos + HEIGHT_OF_BOXES/2, yPos + DIST_TO + WIDTH_OF_BOXES), 
			                                      new Vector2(xPos - HEIGHT_OF_BOXES/2, yPos + DIST_TO));
		else
			collidersY = Physics2D.OverlapAreaAll(new Vector2(xPos + HEIGHT_OF_BOXES/2, yPos - DIST_TO - WIDTH_OF_BOXES), 
			                                      new Vector2(xPos - HEIGHT_OF_BOXES/2, yPos - DIST_TO));
		if(!goingRight)
			collidersX = Physics2D.OverlapAreaAll(new Vector2(xPos + DIST_TO, yPos - HEIGHT_OF_BOXES/2), 
			                                      new Vector2(xPos + DIST_TO + WIDTH_OF_BOXES, yPos + HEIGHT_OF_BOXES/2));
		else
			collidersX = Physics2D.OverlapAreaAll(new Vector2(xPos - DIST_TO, yPos - HEIGHT_OF_BOXES/2), 
			                                      new Vector2(xPos - DIST_TO - WIDTH_OF_BOXES, yPos + HEIGHT_OF_BOXES/2));

		foreach (Collider2D col in collidersX) {
			if(col.CompareTag("Tile"))
				WalkingGeneration.unloadPeice(col.gameObject);
		}
		foreach (Collider2D col in collidersY) {
			if(col.CompareTag("Tile"))
				WalkingGeneration.unloadPeice(col.gameObject);
		}
	}
	
	//adds or subtracts 1 or 2 blocks to the height
	protected int selectDirection() {
		return (int) Random.Range (-1.95f, 1.95f);
	}
}
