using UnityEngine;
using System.Collections;

public class WalkingGeneration : CreateMap {
	
	//private static int playerXPosition;

	// Loading
	public static void loadPeices(int xPos) {

		int playerXPosition = xPos;
		bool goingRight = player.GetComponent<Rigidbody2D> ().velocity.x > 0;
		
		if(goingRight) {
			int loadingPosition = playerXPosition + distanceBetweenLoads;
			int loadTo = playerXPosition + 2*distanceBetweenLoads;

			for (float i = loadingPosition; i < loadTo; i += widthOfGroundPiece) {
				for  (float j = minY; j < maxY; j += widthOfGroundPiece) {
					loadPeice(i,j,0);
				}
			}

		} else {
			int loadingPosition = playerXPosition - distanceBetweenLoads;
			int loadTo = playerXPosition - 2*distanceBetweenLoads;

			for (float i = loadingPosition; i > loadTo; i -= widthOfGroundPiece) {
				for  (float j = minY; j < maxY; j += widthOfGroundPiece) {
					loadPeice(i,j,0);
				}
			}
		}
	}
	
	public static void loadPeice(float i, float j, float k) {
		Vector3 pos = new Vector3 (i, j, k);
		string tile = (string) map[pos];
		if(tile != null) {
			GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), pos, Quaternion.identity);
			obj.transform.SetParent(ground.transform);
		}
	}

	// Unloading
	public static void unloadPeices(int xPos) {

		int playerXPosition = xPos;
		bool goingRight = player.GetComponent<Rigidbody2D>().velocity.x > 0;

		if(goingRight) {
			float startingPos = playerXPosition - distanceBetweenLoads;
			float endingPos = playerXPosition - 2*distanceBetweenLoads;

			for (float i = startingPos; i > endingPos; i -= widthOfGroundPiece) {
				for(float j = minY; j < maxY; j += widthOfGroundPiece) {
					unloadPeice(i,j,0);
				}
			}
		} else {
			float startingPos = playerXPosition + distanceBetweenLoads;
			float endingPos = playerXPosition + 2*distanceBetweenLoads;

			for (float i = startingPos; i < endingPos; i += widthOfGroundPiece) {
				for(float j = minY; j < maxY; j += widthOfGroundPiece) {
					unloadPeice(i,j,0);
				}
			}
		}
	}

	// Saves point to the map
	public static void unloadPeice(float i, float j, float k) {
		Vector3 pos = new Vector3 (i, j, k);
		Collider2D box = Physics2D.OverlapPoint(pos);
		
		if(map.Contains(pos)) {
			map.Remove(pos);
		}
		
		Item script;
		if(box != null && (script = box.gameObject.GetComponent<Item>()) != null) {
			map.Add(pos, script.name);
			GameObject.Destroy(box.gameObject);
		}
	}

	// Are there blocks in a given line/area?
	private static bool areBlocksAt(float beginXPos, float endXPos, float beginYPos, float endYPos) {
		
		Vector2 pos = new Vector2();
		
		for (float i = beginXPos; i < endXPos; i+= widthOfGroundPiece) {
			for(float j = beginYPos; j < endYPos; j+= widthOfGroundPiece) {
				pos.x = i;
				pos.y = j;
				if(Physics2D.OverlapPoint (pos) != null)
					return true;
			}
		}
		return false;
	}
}
