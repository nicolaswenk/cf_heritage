﻿using UnityEngine;
using System.Collections;

public class s_next : MonoBehaviour
{
	
	public static bool b_reached;
	public GUISkin stylesheet;

	
	void Start ()
	{
		gameObject.SetActive (false);
	}
	
	void Update ()
	{
	}

	void OnGUI ()
	{
		GUI.skin = stylesheet;

		if (GUI.Button (new Rect (Screen.width / 2 + 150, Screen.height / 2 + 100, 130, 40), lng.t [8])) { 
			audio.Play ();
			
			if (Application.loadedLevelName == "3_path")
				Application.LoadLevel (1);
			else if (Application.loadedLevelName == "4_path")
				Application.LoadLevel (1);
			else if (Application.loadedLevelName == "5_path") {
				s_start.b_end = true;
				Application.LoadLevel (1);	
			}
		}
	}
}
