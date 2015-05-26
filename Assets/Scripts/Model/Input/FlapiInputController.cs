using System;
using FlapiUnity;
using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the Flutter as input device using the Flapi library.
/// </summary>
public class FlapiInputController : OnlyExpirationInputController
{
	/// <summary>
	/// The audio source of the Flutter.
	/// </summary>
	private AudioSource audio;

	public FlapiInputController (DecreasingDrainageAutogene exercice, AudioSource audio):base(exercice)
	{
		this.audio = audio;
	}

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public override void Update(){		
		Flapi.Analyze (audio);
	}

	/// <summary>
	/// Returns whether the patient is blowing or not.
	/// </summary>
	/// <returns><c>true</c>, if the patient is blowing, <c>false</c> otherwise.</returns>
	protected override bool isBlowing(){
		Debug.Log (Flapi.blowing + " - " + Flapi.frequency);
		return Flapi.blowing;
	}

	/// <summary>
	/// Determines whether the patient is holding the moving input or not (if the right arrow of the keyboard is down or not).
	/// </summary>
	public override bool IsMoving ()
	{
		return Input.GetKey (KeyCode.RightArrow);
	}

	/// <summary>
	/// Gets the strength of expiration which is related to the Flutter's frequency.
	/// 0.0f, it means that the patient is not blowing.
	/// </summary>
	public override float GetExpirationStrength(){
		return Flapi.frequency/12.0f;
	}
}

