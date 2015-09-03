using UnityEngine;
using System.Collections;
using System.IO.Ports;

/// <summary>
/// This class manages the device that gets the pressure / flow from the PEP.
/// The input selection is done in BuildAndStart() in Assets/Scripts/Controller/LevelController.cs.
/// </summary>
public class PepInputController : MonoBehaviour {
{
	public static float pepValue = 0f;
	/// <summary>
	/// Open to bluetooth port. For Windows OS : COM3 or COM4, for Mac OS : /dev/tty.usbmodem621.
	/// </summary>
	SerialPort stream = new SerialPort("/dev/tty.usbmodem621", 9600);
		
	/// <summary>
	/// Initialize the call to the device. Timeout is the delay if data is not received, Invokes frees memory by reading the port each 0.5 sec.
	/// </summary>
	public void Start(){
		stream.Open();
		stream.ReadTimeout = 5;
		InvokeRepeating("streamIn", 0.5f, 0.5f);
	}

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public void Update(){
		string pepRaw = stream.ReadLine();
		pepValue = float.Parse (pepRaw);
	}

	/// <summary>
	/// Gets the strength of expiration. 0.0f means that the patient is not blowing.
	/// </summary>
	public float GetStrength(){
		return pepValue;
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