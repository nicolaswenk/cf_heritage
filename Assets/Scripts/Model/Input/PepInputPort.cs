using System;
using System.IO.Ports;
using UnityEngine;
using System.Collections;

public class PepInputPort : MonoBehaviour {
    
    SerialPort stream = new SerialPort("\\\\.\\COM30", 9600);

	private float pepValue = 0f;

	public void Start(){
		stream.Open();
		stream.ReadTimeout = 5;
	}

	public void Update(){
		if (stream.IsOpen)
		{
			try
			{
				string arduinoValue = stream.ReadLine();								// reads serial port
                pepValue = float.Parse(arduinoValue);
                PepInputController.pepValue = pepValue;
			}
			catch(System.Exception)												// exit the reading if no value to avoid infinite run
			{	
			}
		}	
	}
}