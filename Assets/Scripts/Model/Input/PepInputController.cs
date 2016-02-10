using System;
using System.IO.Ports;
using UnityEngine;
using System.Collections;


/// <summary>
/// This class manages the device that gets the pressure / flow from the PEP.
/// The input selection is done in BuildAndStart() in Assets/Scripts/Controller/LevelController.cs.
/// </summary>
public class PepInputController : InputController_I
{
    public static string arduinoValue;
    public static float pepValue = 0f;
    SerialPort stream = new SerialPort("COM4", 9600);
    private float initPepValue = 0.0f;
    


    public PepInputController()
    {
        stream.Open();
        streamIn();
        initPepValue = pepValue;
        Debug.Log("HEY_PEP");
        //	stream.ReadTimeout = 100; // milliseconds
        //InvokeRepeating("streamIn", 0.01f, 0.01f); // start delay, repeat delay in seconds
    }

    void streamIn()
    {
        if (stream.IsOpen)
        {
            try
            {
                arduinoValue = stream.ReadLine(); // reads serial port
                pepValue = float.Parse(arduinoValue); // should be from 0 to 100
                pepValue = pepValue / 100.0f;
                pepValue -= initPepValue;
                Debug.Log(pepValue);
            }
            catch (System.Exception) // exit the reading if no value to avoid infinite run
            {
                Debug.LogError("exception Arduino");
            }
        }
    }

	/// <summary>
	/// Gets the strength of expiration. 0.0f means that the patient is not blowing.
	/// </summary>
    public float GetStrength()
    {
        return pepValue;
	}
	
	/// <summary>
	/// Gets the BreathingState (expiration, inspiration, holding breath, ...).
	/// </summary>
	public BreathingState GetInputState(){
		if (pepValue > 0.1f) {
			return BreathingState.EXPIRATION;
		} else {
			return BreathingState.HOLDING_BREATH;
		}
	}
	
	/// <summary>
	/// Determines whether the patient is holding the moving input or not (if the right arrow of the keyboard is down or not).
	/// </summary>
	public bool IsMoving(){
		return Input.GetKey (KeyCode.RightArrow);
	}

    void InputController_I.Update()
    {
        streamIn();
    }
}