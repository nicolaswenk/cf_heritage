using System;
using UnityEngine;

/// <summary>
/// This class simulated a Flutter (for testing purposes). If we press the space key, it's like we are blowing as awaited and
/// if we press Ctrl+space it's like we are blowing strongly. 
/// </summary>
public class KeyboardInputController:OnlyExpirationInputController
{
	/// <summary>
	/// Represents the state of the space key which simulate the patient blowing.
	/// </summary>
	private bool isSpaceDown=true;

	/// <summary>
	/// The expiration speed of a strong expiration.
	/// </summary>
	private float strongBreathValue;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeyboardIOController"/> class.
	/// </summary>
	/// <param name="exercice">Exercice.</param>
	/// <param name="strongBreathValue">The expiration speed of a strong breath.</param>
	public KeyboardInputController (float strongBreathValue)
	{
		this.strongBreathValue = strongBreathValue;
	}

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public override void Update(){
		this.isSpaceDown=Input.GetKey (KeyCode.Space);
	}

	/// <summary>
	/// Returns whether the patient is blowing or not (if the space bar is down or not).
	/// </summary>
	/// <returns><c>true</c>, if the patient is blowing, <c>false</c> otherwise.</returns>
	protected override bool isBlowing(){
		return this.isSpaceDown;
	}

	/// <summary>
	/// Gets the strength of expiration (1.0f if the space key is down, <see cref="strongBreathValue"/> if Ctrl+space is down and 0.0f otherwise).
	/// 0.0f, it means that the patient is not blowing.
	/// </summary>
	public override float GetExpirationStrength(){

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
	public override bool IsMoving(){
		return Input.GetKey (KeyCode.RightArrow);
	}
}

