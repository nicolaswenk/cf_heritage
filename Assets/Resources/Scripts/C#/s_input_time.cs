using UnityEngine;
using System.Collections;

public class s_input_time : MonoBehaviour
{

	public GameObject dot;
	public GameObject zone_ok;
	public GUIText g_text_todo;
	public GUIText g_text_ok;
	private float[] breathing = new float[1600];
	private float clock;
	private float clockEnd;
	private float clockExhaleEnd;
	private int i_time = 0;
	private int i_arrows_todo = 15;
	private bool b_breath_start = true;
	private bool b_breath_end = true;

	void Start ()
	{
		clock = Time.fixedTime + 0.05f;													// time between each dot of the scheme
	}
	
	void Update ()
	{
		lng.Translate ();
		clockEnd = clock - Time.fixedTime;
		
		if (clockEnd < 0f) {
			if (i_time > 20) {																// if not during the first second (launch)
				breathing [i_time] = s_input.f_pressure;										// saving pressure in an array
				Instantiate (dot, new Vector3 (-8.0f + (i_time * 0.01f), (s_input.f_pressure * 3) - 4.6f, -1), Quaternion.Euler (0, 0, 0)); // draw scheme
				clock = Time.fixedTime + 0.05f;
			}
			i_time++;
		}
		
		if ((clockExhaleEnd < Time.fixedTime) && (i_time > 20)) { 							// if exhalation done long enough and not during launch
			g_text_ok.text = ":-)";															// visual feedback
			
			if (b_breath_start == true) {														// do this action once
				i_arrows_todo -= 1;
				Actions ();
				b_breath_start = false;
			}
		}
		if (i_arrows_todo > 1)																// visual feedback
			g_text_todo.text = i_arrows_todo.ToString () + lng.t [14];
		else
			g_text_todo.text = i_arrows_todo.ToString () + lng.t [15];
		
		if ((s_input.f_pressure < 0.3081f) || (s_input.f_pressure > 0.7843f)) {				// if pressure too low or too high, restart
			g_text_ok.text = " ";
			b_breath_start = true;
			b_breath_end = true;
			clockExhaleEnd = Time.fixedTime + 3f;											// exhalation duration
		} else {
			if ((b_breath_end == true) && (i_time > 20)) { 										// visual feedback to show how long and how much to exhale
				Instantiate (zone_ok, new Vector3 (-7.7f + (i_time * 0.01f), -2.8f, -1), Quaternion.Euler (-90, 0, 0));
				b_breath_end = false;
			}
		}

		if (i_arrows_todo == 0)
			Application.LoadLevel (2);
	}

	
	
	public static int arrow_up = 0;
	public static int arrow_down = 0;
	public static int arrow_left = 0;
	public static int arrow_right = 0;
	
	public GameObject fleche;
	private int turn = 0;
	private int turn_arrow = 0;

	void Actions ()
	{
		switch (turn) {
		case 0:
			turn_arrow = Random.Range (1, 5);
			break;
		case 1:
			turn_arrow = 1;
			break;
		case 2:
			turn_arrow = 3;
			break;
		case 3:
			turn_arrow = Random.Range (1, 5);
			break;
		case 4:
			turn_arrow = 4;
			break;
		case 5:
			turn_arrow = Random.Range (1, 5);
			break;
		case 6:
			turn_arrow = Random.Range (1, 5);
			break;
		case 7:
			turn_arrow = 4;
			break;
		case 8:
			turn_arrow = 2;
			break;
		case 9:
			turn_arrow = Random.Range (1, 5);
			break;
		case 10:
			turn_arrow = 1;
			break;
		case 11:
			turn_arrow = Random.Range (1, 5);
			break;
		case 12:
			turn_arrow = 1;
			break;
		case 13:
			turn_arrow = 4;
			break;
		case 14:
			turn_arrow = Random.Range (1, 5);
			break;
		}
		
		switch (turn_arrow) {
		case 1:
			Instantiate (fleche, new Vector3 (Random.Range (5.3f, 7.4f), Random.Range (-0.5f, 4.2f), -1), Quaternion.Euler (0, 90, 270));
			arrow_up++;
			break;
		case 2:
			Instantiate (fleche, new Vector3 (Random.Range (5.3f, 7.4f), Random.Range (-0.5f, 4.2f), -1), Quaternion.Euler (0, 270, -270));
			arrow_down++;
			break;
		case 3:
			Instantiate (fleche, new Vector3 (Random.Range (5.3f, 7.4f), Random.Range (-0.5f, 4.2f), -1), Quaternion.Euler (270, 0, 0));
			arrow_left++;
			break;
		case 4:
			Instantiate (fleche, new Vector3 (Random.Range (5.3f, 7.4f), Random.Range (-0.5f, 4.2f), -1), Quaternion.Euler (90, 180, 0));
			arrow_right++;
			break;
		}
		turn++;
	}
}
