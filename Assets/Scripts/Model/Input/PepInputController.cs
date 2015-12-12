using System;
using System.IO.Ports;
using UnityEngine;


/// <summary>
/// This class manages the device that gets the pressure / flow from the PEP.
/// The input selection is done in BuildAndStart() in Assets/Scripts/Controller/LevelController.cs.
/// </summary>
public class PepInputController : InputController_I
{
	public static float pepValue = 0f;

	public void Start(){
	}

	public void Update(){
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