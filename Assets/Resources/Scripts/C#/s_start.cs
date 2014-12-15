using UnityEngine;
using System.Collections;

public class s_start : MonoBehaviour
{
	
	public static int i_level;
	public static bool b_end;
	private static float top;
	private static float left;
	private static float width;
	public GUISkin stylesheet;
	public GameObject[] grounds;

	private static Vector3 worldPoint;
	private static float speedGrounds = 0.05f;
	private static float speedMountains = 2f;

	void Start ()
	{
		worldPoint = Camera.main.ViewportToWorldPoint (new Vector3 (1.0f, 1.0f));
		top = worldPoint.y;
		left = -worldPoint.x;
		width = -left * 2.0f;

		//Camera.main.aspect = 4 / 3f;
		grounds = new GameObject[4];
		grounds [0] = GameObject.Find ("ground0");
		grounds [1] = GameObject.Find ("ground1");
		grounds [2] = GameObject.Find ("ground2");
		grounds [3] = GameObject.Find ("ground3");

		for (int i = 0; i < grounds.Length; i++) {
			grounds [i].transform.position = new Vector3 (i * Random.Range (5, 15), -top + (grounds [i].renderer.bounds.size.y / 2));
		}
	}

	void Update ()
	{
		lng.Translate ();

		if (Input.GetKey (KeyCode.M))
			AudioListener.pause = !AudioListener.pause;
		if (Input.GetKey (KeyCode.Q))
			Application.Quit ();

		for (int i = 0; i < grounds.Length; i++) {
			float x = grounds [i].transform.position.x - speedGrounds;
			float y = grounds [i].transform.position.y;

			if (x < left - 10)
				x = -left + Random.Range (5, 15);

			grounds [i].transform.position = new Vector3 (x, y, 0);
		}
	}
	
	void OnGUI ()
	{
		GUI.skin = stylesheet;
		
		GUI.TextField (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 150, 300, 40), s_webresults.user + " @ Globule", "title_1");

		if (lng.s_lng == "fr") {
			if (GUI.Button (new Rect (Screen.width / 2 + 130, Screen.height / 2 - 150, 80, 30), "English", "small"))
				lng.s_lng = "en";
		} else {
			if (GUI.Button (new Rect (Screen.width / 2 + 130, Screen.height / 2 - 150, 80, 30), "Français", "small"))
				lng.s_lng = "fr";
		}

		if (b_end == false) {
			GUI.TextArea (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 80, 600, 120), lng.t [1]);
		} else {
			GUI.TextArea (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 80, 600, 120), lng.t [2]);
			b_end = false; 
		}
		
		if (GUI.Button (new Rect (Screen.width / 2 - 300, Screen.height / 2 + 50, 130, 40), lng.t [0])) { 
			audio.Play ();
			i_level = 1;
			Application.LoadLevel (1);
		}
		
		if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 130, 40), lng.t [3] + "\n\n" + lng.t [10], "link")) 
			Application.ExternalEval ("window.open('http://www.fibrosekystique.net','fibrosekystique.net','width=640,height=480,left=0,top=0')");
		
		if (GUI.Button (new Rect (Screen.width / 2 + 110, Screen.height / 2 + 50, 130, 40), lng.t [4] + "\nDavid Arango\nFabio Balli\nJeremy Bouchard\nYannick Gervais\nFlorian Moncomble", "link")) 
			Application.ExternalEval ("window.open('http://www.fibrosekystique.net','fibrosekystique.net','width=640,height=480,left=0,top=0')");

	}
}
