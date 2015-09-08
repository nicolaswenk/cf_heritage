using System;
using System.IO.Ports;
using UnityEngine;


/// <summary>
/// This class manages the device that gets the pressure / flow from the PEP.
/// The input selection is done in BuildAndStart() in Assets/Scripts/Controller/LevelController.cs.
/// </summary>
public class PepInputController : InputController_I
{
	/// <summary>
	/// Open the port. For Mac = /dev/tty.usbmodem621, PC = COM3 or COM4, if COM higher than 9, "\\\\.\\COM25"
	/// </summary>
	SerialPort stream = new SerialPort("\\\\.\\COM25", 9600);

	private int x = 0;
	private float pepValue = 0f;
		
	/// <summary>
	/// Initialize the call to the device. Timeout is the delay if data is not received..
	/// </summary>
	public void Start(){
		stream.Open();
		stream.ReadTimeout = 5;
	}

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public void Update(){
		if (stream.IsOpen)
		{
			try
			{
				string value = stream.ReadLine();								// reads serial port
				pepValue = float.Parse(value);
				x++;
			}
			catch(System.Exception)												// exit the reading if no value to avoid infinite run
			{	
			}
		}	
	}

	/// <summary>
	/// Gets the strength of expiration. 0.0f means that the patient is not blowing.
	/// </summary>
	public float GetStrength(){
		return pepValue;
		Debug.Log (pepValue);
	}
	
	/// <summary>
	/// Gets the BreathingState (expiration, inspiration, holding breath, ...).
	/// </summary>
	public BreathingState GetInputState(){
		if (pepValue > 0.3f) {
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
}