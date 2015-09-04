using UnityEngine;
using System.Collections;

public class GenerateTrees {

	public static void placeTrees(int maxY){

		int counter = 0;

		for (int i = -1000; i < 1000; i++){
			for (int j = 0; j < maxY; j++){

				object currentTile = CreateMap.map[new Vector3(i,j,0)];
				if (currentTile != null){
					if (currentTile.Equals("DirtWGrass")  &&  counter > 5){
						CreateMap.map.Add(new Vector3(i,j+1,0), "Log");
						counter = 0;
					}else{
						counter++;
					}
				}
			}
		}
	}
}
