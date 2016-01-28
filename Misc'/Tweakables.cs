using UnityEngine;
using System.Collections;

public class Tweakables : MonoBehaviour {

    // Static variables for scripts to access
    public static bool darknessActivated = true;
    public static float AI_DELAY;
    public static float scale;

    // Regular variables for unity editor change.
	public bool darkness_activated;
    public float AIDELAY;
    public float difficultyScale;

	// Use this for initialization
	void Start () {
		darknessActivated = darkness_activated;
        AI_DELAY = AIDELAY;
        scale = difficultyScale;

        if (darknessActivated) {
            RenderSettings.ambientLight = Color.white;
            RenderSettings.ambientIntensity = 0f;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
