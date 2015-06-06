using System;
using UnityEngine;

/// <summary>
/// This class simulated a Flutter (for testing purposes). If we press the space key, it's like we are blowing as awaited and
/// if we press Ctrl+space it's like we are blowing strongly. 
/// </summary>
public class KeyboardSimpleInputController:InputController_I
{
	/// <summary>
	/// Represents the state of the space key which simulate the patient blowing.
	/// </summary>
	private bool isSpaceDown=false;
	
	/// <summary>
	/// The expiration speed of a strong expiration.
	/// </summary>
	private float strongBreathValue;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="KeyboardIOController"/> class.
	/// </summary>
	/// <param name="strongBreathValue">The expiration speed of a strong breath.</param>
	public KeyboardSimpleInputController (float strongBreathValue)
	{
		this.strongBreathValue = strongBreathValue;
	}
	
	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public void Update(){
		this.isSpaceDown=Input.GetKey (KeyCode.Space);
	}
	
	
	/// <summary>
	/// Gets the BreathingState (expiration, inspiration, holding breath, ...).
	/// </summary>
	public BreathingState GetInputState(){
		if (this.isSpaceDown) {
			return BreathingState.EXPIRATION;
		} else {
			return BreathingState.HOLDING_BREATH;
		}
	}
	
	/// <summary>
	/// Gets the strength of expiration (1.0f if the space key is down, <see cref="strongBreathValue"/> if Ctrl+space is down and 0.0f otherwise).
	/// 0.0f, it means that the patient is not blowing.
	/// </summary>
	public float GetStrength(){
		
		if (this.isSpaceDown) {
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
				return strongBreathValue;
			}
			else{
				return 1.0f;
			}
		} else {
			return 0.0f;
		}
	}
	
	/// <summary>
	/// Determines whether the patient is holding the moving input or not (if the right arrow of the keyboard is down or not).
	/// </summary>
	public bool IsMoving(){
		return Input.GetKey (KeyCode.RightArrow);
	}
}

