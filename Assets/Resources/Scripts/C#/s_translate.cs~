using UnityEngine;
using System.Collections;

public class s_translate : MonoBehaviour {

	public GUIText g_text;

	void Start() {
	}

	void Update () {
		
		switch (Application.loadedLevelName) {
		case "niveauTestCube01":
			g_text.text = lng.t[5];
			break;
		case "niveauTestCube02":
			g_text.text = lng.t[6];
			break;
		case "niveauTestCube03":
			break;
		}
		lng.Translate();
	}
}
