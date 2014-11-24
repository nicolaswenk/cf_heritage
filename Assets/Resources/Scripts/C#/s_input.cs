using UnityEngine;
using System.Collections;

public class s_input : MonoBehaviour
{
	
	public static float f_pressure;
	
	//	input_keyboard		0f or 1f, key m
	//	input_micro			0f to 1f, sound from external microphone
	//  input_pep_redbox	0f to 1f, pressure from usb prototype, with instance
	//  input_pep_redbox2	0f to 1f, pressure from usb pressure prototype, with timeout and try
	//	Input_pep_onatha	0f to 1f, pressure from usb key
	
	public GUIText g_text_cm;
	public GUIText g_text_feedback;
	private float  f_cm;

	void Start ()
	{

	}
	
	void Update ()
	{
		lng.Translate ();
		f_cm = Mathf.Round (f_pressure / 0.03922f);
		g_text_cm.text = f_cm.ToString () + " cm H2O";

		if (f_pressure < 0.3081)									// -0.38378 // < 7.8 cm H20
			g_text_feedback.text = " ";
		else if (f_pressure < 0.3922)								// -0.21569 // 7.8 to 10 cm H20
			g_text_feedback.text = lng.t [11];
		else if (f_pressure < 0.7843)  								//  0.56863 // 10 to 20 cm H20, optimal zone
			g_text_feedback.text = lng.t [13];
		else 														// > 20 cm H20, too hard
			g_text_feedback.text = lng.t [12];		
	}
}
