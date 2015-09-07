using UnityEngine;
using System.Collections;

public class WalkingGeneration : CreateMap {
	
	private static float yMin, yMax, xMin, xMax;

	public static float cameraSizeX = 60, cameraSizeY = 80;
	public static int bufferSize = 2;


	// Loading
	public static void loadPeices(int xPos, int yPos, bool goingRight, bool goingUp, bool x, bool y) {
		int yMod;
		int xMod;
		
		if (goingUp) {
			yMod = 1;
		} else {
			yMod = -1;
		}
		
		if (goingRight) {
			xMod = 1;
		} else {
			xMod = -1;
		}
		
		int loadingXPosition;
		int loadXTo;
		int loadingYPosition;
		int loadYTo;
		
		/*                  ↓↓ Loading this part
   		 _ _ _ _ _ _ _
		|	_ _ _ _	  |
		|  |	   |  |
		|  |	.  |  |
		|  |_ _ _ _|  |
		|_ _ _ _ _ _ _|
*/		
		
		if (x) {
			
			// X LOADING
			loadingXPosition = Mathf.RoundToInt (xPos + xMod * cameraSizeX);
			loadXTo = Mathf.RoundToInt (xPos + xMod * (cameraSizeX + bufferSize));
			
			loadingYPosition = Mathf.RoundToInt (yPos - cameraSizeY - bufferSize);
			loadYTo = Mathf.RoundToInt (yPos + (cameraSizeY + bufferSize));
			
			if (goingRight) {
				for (float i = loadingXPosition; i < loadXTo; i += widthOfGroundPiece) {
					for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
						loadPeice (i, j, 0);
					}
				}
			} else {
				for (float i = loadingXPosition; i > loadXTo; i -= widthOfGroundPiece) {
					for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
						loadPeice (i, j, 0);
					}
				}
			}
		}
/*		 _ _ _ _ _ _ _
		|	_ _ _ _	  | <- Loading this part
		|  |	   |  |
		|  |	.  |  |
		|  |_ _ _ _|  |
		|_ _ _ _ _ _ _|
*/
		if (y) {
			
			// Y LOADING
			loadingXPosition = Mathf.RoundToInt (xPos - cameraSizeX - bufferSize);
			loadXTo = Mathf.RoundToInt (xPos + cameraSizeY + bufferSize);
			
			loadingYPosition = Mathf.RoundToInt (yPos + yMod * cameraSizeY);
			loadYTo = Mathf.RoundToInt (yPos + yMod * (cameraSizeY + bufferSize));
			
			if (goingUp) {
				for (float j = loadingYPosition; j < loadYTo; j += widthOfGroundPiece) {
					for (float i = loadingXPosition; i < loadXTo; i += widthOfGroundPiece) {
						loadPeice (i, j, 0);
					}
				}
			} else {
				for (float j = loadingYPosition; j > loadYTo; j -= widthOfGroundPiece) {
					for (float i = loadingXPosition; i < loadXTo; i += widthOfGroundPiece) {
						loadPeice (i, j, 0);
					}
				}
			}
		}
	}


	
	public static void loadPeice(float i, float j, float k) {


		Vector3 pos = new Vector3 (i, j, k);

		if (Physics2D.OverlapPoint (pos))
			return;

		string tile = (string) map[pos];
		if(tile != null) {
			GameObject obj = (GameObject) GameObject.Instantiate(Dictionary.get (tile), pos, Quaternion.identity);
			obj.transform.SetParent(ground.transform);
		}
	}

	// Unloading
	public static void unloadPeices(int xPos, int yPos, bool goingRight, bool goingUp, bool x, bool y) {

	}

	public static void unloadPeice(GameObject gm) {
		Vector3 pos = gm.transform.position;
		Item script = gm.GetComponent<Item>();
		if (map.Contains (pos)) {
			if(map[pos].Equals(script.name))
				map.Remove(pos);
			else
				return;
		}
		map.Add (pos, script.name);
		GameObject.Destroy (gm);
		
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
