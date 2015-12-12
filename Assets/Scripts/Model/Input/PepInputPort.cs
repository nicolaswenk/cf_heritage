using System;
using System.IO.Ports;
using UnityEngine;
using System.Collections;

public class PepInputPort : MonoBehaviour {
    
    SerialPort stream = new SerialPort("\\\\.\\COM30", 9600);

	// for PC "COM4" or "\\\\.\\COM30" Mac OS : "/dev/tty.usbmodem1421"

	private float pepValue = 0f;

	public void Start(){
		stream.Open();
	//	stream.ReadTimeout = 100; // milliseconds
        InvokeRepeating("streamIn", 0.01f, 0.01f); // start delay, repeat delay in seconds
	}

	public void Update(){
	}

    void streamIn()
    {
        if (stream.IsOpen)
        {
            try
            {
                string arduinoValue = stream.ReadLine(); // reads serial port
                PepInputController.arduinoValue = arduinoValue;
                pepValue = float.Parse(arduinoValue); // should be from 0 to 100
                pepValue = pepValue / 100;
                PepInputController.pepValue = pepValue;
            }
            catch (System.Exception) // exit the reading if no value to avoid infinite run
            {
            }
        }	
    }
}