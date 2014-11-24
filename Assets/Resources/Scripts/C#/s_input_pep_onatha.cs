using UnityEngine;
using System.Collections;

public class s_input_pep_onatha : MonoBehaviour
{

	private float f_pressure_inter;
	
	// Edit > Project Settings > Input
	// Name			Breath
	// Gravity		0
	// Dead			0
	// Sensitivity	1
	// Type 		Joystick Axis
	// Axis			3rd axis
	// Joy Num		Get motion from all

	void Start ()
	{
	}

	void Update ()
	{
		f_pressure_inter = Input.GetAxisRaw ("Breath");
		s_input.f_pressure = (f_pressure_inter + 1) / 2;
	}
}
