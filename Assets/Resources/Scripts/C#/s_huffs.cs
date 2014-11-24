using UnityEngine;
using System.Collections;

public class s_huffs : MonoBehaviour
{

	public GUISkin stylesheet;

	void Start ()
	{
	}

	void Update ()
	{
		lng.Translate ();
	}

	void OnGUI ()
	{
		GUI.skin = stylesheet;
		
		GUI.TextField (new Rect (40, 20, 300, 40), lng.t [9], "title_1");
		
		if (GUI.Button (new Rect (530, 360, 130, 40), lng.t [8])) { 
			switch (s_start.i_level) {
			case 1:
				s_start.i_level = 3;
				Application.LoadLevel (3);
				break;
			case 3:
				s_start.i_level = 4;
				Application.LoadLevel (4);
				break;
			case 4:
				s_start.i_level = 5;
				Application.LoadLevel (5);
				break;
			case 5:
				s_start.i_level = 0;
				Application.LoadLevel (0);
				break;
			}
		}
	}
}
