using UnityEngine;
using System.Collections;

public class GenerateTrees {

	public static void treeLocation(int maxY, float biomeProb){

		int counter = 0;

		for (int i = -1000; i < 1000; i++){
			for (int j = 0; j < maxY; j++){

				object currentTile = CreateMap.map[new Vector3(i,j,0)];
				if (currentTile != null){
					if (currentTile.Equals("DirtWGrass")  &&  counter > 5){
						if((int) Random.Range (-1.95f, biomeProb) == 1){
							placeTrees(new Vector3(i,j+1,0));
							counter = 0;
						}
					}else if (currentTile.Equals ("DirtWGrass")){
						counter++;
					}
				}
			}
		}
	}

	public static void placeTrees(Vector3 location){

		CreateMap.map.Add(location, "Log");
		CreateMap.map.Add(new Vector3(location.x,location.y+1,location.z), "Log");
		CreateMap.map.Add(new Vector3(location.x,location.y+2,location.z), "Log");
		CreateMap.map.Add(new Vector3(location.x,location.y+3,location.z), "Log");
		CreateMap.map.Add(new Vector3(location.x,location.y+4,location.z), "Log");

		CreateMap.map.Add(new Vector3(location.x-1,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+1,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x-2,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+2,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x-3,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+3,location.y+3,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x,location.y+3,location.z-1), "Leaves");
		CreateMap.map.Add(new Vector3(location.x,location.y+4,location.z-1), "Leaves");
		CreateMap.map.Add(new Vector3(location.x-1,location.y+4,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+1,location.y+4,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x-2,location.y+4,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+2,location.y+4,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x,location.y+5,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x-1,location.y+5,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x+1,location.y+5,location.z), "Leaves");
		CreateMap.map.Add(new Vector3(location.x,location.y+6,location.z), "Leaves");
	}
}
