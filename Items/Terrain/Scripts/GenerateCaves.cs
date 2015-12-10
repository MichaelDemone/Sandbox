using UnityEngine;
using System.Collections;

public class GenerateCaves : CreateMap {

	public static void generateCave() {
		//int cavePointNum = Mathf.RoundToInt(Random.Range (1, 10));
		int cavePointNum = 1;
		if (cavePointNum == 1) {
			//int xPos = (int) Random.Range(minX, maxX);
			int xPos = (int) Random.Range (-100, 100);
			int yPos = (int) Random.Range(minY, maxY);
			int radius = Random.Range(2,10);

			for(int i = xPos - radius, num = 0; i < xPos; i += distanceBetweenLoads, num++) {
				for(int j = yPos - num; j <= yPos + num; j += distanceBetweenLoads) {
					CreateMap.map.Remove(new Vector3(i,j,STANDARD_LAYER));
				}
			}

			for(int i = xPos, num = radius; i <= xPos + radius; i += distanceBetweenLoads, num--) {
				for(int j = yPos - num; j <= yPos + num; j += distanceBetweenLoads) {
					CreateMap.map.Remove(new Vector3(i,j,STANDARD_LAYER));
				}
			}

		} else if (cavePointNum == 2) {
			
		} else {
			
		}
	}
}
