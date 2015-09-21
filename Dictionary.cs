using UnityEngine;
using System.Collections;

public class Dictionary : MonoBehaviour {

	public static Hashtable dictionary;
	public static bool initialized = false;

	//public GameObject dirt, copper, collectable, dirtWGrass, sand, leaves, log, torch, light, rock, copperOre, coalOre, ironOre, titaniumOre, goldOre, platinumOre, coal, iron, titanium, gold, platinum;
	public GameObject collectable;

	// Use this for initialization
	void Start () {

		dictionary = new Hashtable();
		// Adding terrain

		create (new Block("Dirt"));
		
		Block dirtWGrass = new Block("DirtWGrass");
		dirtWGrass.dropName = "Dirt";
		create (dirtWGrass);

		Block leaves = new Block("Leaves");
		leaves.isTrigger = true;
		leaves.dropName = "";
		create (leaves);

		Block coalOre = new Block("CoalOre");
		coalOre.dropName = "Coal";
		create(coalOre);
		
		create(new Block("Rock"));

		Block log = new Block("Log");
		log.isTrigger = true;
		create(log);

		dictionary.Add ("Collectable", collectable);

		// Adding weapons


		// Adding armour


		// Adding crafting objects

		
	}

	public void create(Block b){
	
		GameObject block = Instantiate(Resources.Load<GameObject>("Block"));

		block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Natural Sprites/" + b.sprite);


		Item it = block.GetComponent<Item>();
		it.item = b.item;
		it.invName = b.dropName;
		it.weapon = b.weapon;
		it.maxStackSize = b.maxStackSize;
		it.numberOfDrops = b.numberOfDrops;
		it.equipped = Resources.Load<Sprite>("Natural Sprites/" + b.equippedSprite);
		it.inventorySprite = Resources.Load<Sprite>("Natural Sprites/" + b.inventorySprite);
		it.name = b.name;
		it.strength = b.strength;

		Mining mi = block.GetComponent<Mining>();
		mi.strength = it.strength;

		block.GetComponent<Collider2D>().isTrigger = b.isTrigger;

		dictionary.Add (b.name, block);
	}

	public static GameObject get(string name) {
		return (GameObject)dictionary [name];

	}


	//STRCUTS FOR SHITTY NATURAL PREFABS

	public struct Block{
		public string sprite;
		public string inventorySprite;
		public string equippedSprite;
		public int strength;
		public bool item;
		public bool weapon;
		public int maxStackSize;
		public int numberOfDrops;
		public bool isTrigger;
		public bool drops;
		public string name;
		public string dropName;

		public Block(string name) {
			this.sprite = name;
			this.inventorySprite = name;
			this.equippedSprite = name;
			this.strength = 1;
			this.item = true;
			this.weapon = false;
			this.maxStackSize = 99;
			this.numberOfDrops = 1;
			this.isTrigger = false;
			this.drops = true;
			this.dropName = name;
			this.name = name;
		}
	}
}
