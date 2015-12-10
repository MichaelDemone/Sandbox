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

		create ( new Light ("Torch"));

		Block wall = new Block ("Wall");
		wall.isWall = true;
		create (wall);


		dictionary.Add ("Collectable", collectable);

		// Adding weapons


		// Adding armour


		// Adding crafting objects

		
	}

	public void create(Block b){
	
		GameObject block = (GameObject) Instantiate(Resources.Load<GameObject>("Block"), new Vector3(0,2000,0), Quaternion.identity);


		block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Natural Sprites/" + b.sprite);
		block.GetComponent<SpriteRenderer> ().color = Color.white;


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
		mi.wall = b.isWall;

		block.GetComponent<Collider2D>().isTrigger = b.isTrigger || b.isWall;

		dictionary.Add (b.name, block);
	}

	public void create(Light l) {
		GameObject light = (GameObject)Instantiate (Resources.Load <GameObject> ("Torch"), new Vector3 (0, 2000, 0), Quaternion.identity);

		Item it = light.GetComponent<Item> ();
		it.name = l.name;
		it.invName = l.name;
		it.item = true;
		it.weapon = false;
		it.maxStackSize = l.maxStack;
		it.numberOfDrops = 1;
		it.equipped = Resources.Load<Sprite> ("Lighting/" + l.sprite);
		it.inventorySprite = it.equipped;

		LightSource ls = light.GetComponentInChildren<LightSource> ();
		ls.blockRange = l.blockRange;
		ls.brightness = l.brightness;
		ls.range = l.range;

		dictionary.Add (l.name, light);
	}

    static Queue dirt = new Queue();
    static Queue wall = new Queue();
    static Queue rock = new Queue();

    public static GameObject get(string name) {
        /*
        Debug.Log("time");
        if (name.Equals("Dirt") && dirt.Count > 0)
            return (GameObject) dirt.Dequeue();
        if (name.Equals("Wall") && wall.Count > 0)
            return (GameObject) wall.Dequeue();
        if (name.Equals("Rock") && rock.Count > 0)
            return (GameObject) rock.Dequeue();
            */
        return (GameObject)dictionary [name];
	}

    public static void remove(GameObject go) {
        /*
        if(go.GetComponent<Item>().name.Equals("Dirt")) {
            dirt.Enqueue(go);
        } else if (go.GetComponent<Item>().name.Equals("Wall")) {
            wall.Enqueue(go);
        } else if (go.GetComponent<Item>().name.Equals("Rock")) {
            rock.Enqueue(go);
        } else { */
        
            Destroy(go);
        //}
        
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
		public bool isWall;

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
			this.isWall = false;
		}
	}

	public struct Light{
		public string name;
		public int maxStack;
		public string sprite;

		public int blockRange;
		public Color colour;

		// Physical light
		public float brightness;
		public int range;

		public Light(string name) {
			this.name = name;
			this.maxStack = 99;
			this.blockRange = 4;
			this.brightness = 0.3f;
			this.range = 15;
			this.colour = Color.white;
			this.sprite = name;
		}

	}
}
