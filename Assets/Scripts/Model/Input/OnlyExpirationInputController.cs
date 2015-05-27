using System;
using UnityEngine;

/// <summary>
/// This abstract class is made to manage any device that can only detect when the patient is blowing.
/// So it has to deduce the two other states : inspiration and holding breath.
/// </summary>
public abstract class OnlyExpirationInputController : InputController_I
{	
	/// <summary>
	/// The last breathing state (from the last time we call <see cref="Update"/>).
	/// </summary>
	private BreathingState lastState=BreathingState.HOLDING_BREATH;

	/// <summary>
	/// The actual breathing state.
	/// </summary>
	private BreathingState state=BreathingState.HOLDING_BREATH;

	/// <summary>
	/// The date time for the last expiration ending (to know when we are supposed to stop considering being inpiring).
	/// </summary>
	private DateTime endExpirationTime;

	/// <summary>
	/// The exercice to follow (used to know the differents speed in each state).
	/// </summary>
	private DecreasingDrainageAutogene exercice;

	public OnlyExpirationInputController(DecreasingDrainageAutogene exercice){
		this.exercice = exercice;
	}
	
	/// <summary>
	/// Gets the strength of expiration or inspiration (depends on the BreathingState).
	/// 0.0f, it means that the patient is not blowing or inspiring.
	/// Returns 1.0f if patient should be inspiring and <see cref="GetExpirationStrength"/> otherwise.
	/// </summary>
	public float GetStrength(){
		switch (state) {
		case BreathingState.INSPIRATION:
				return 1.0f;
		case BreathingState.EXPIRATION:
				return GetExpirationStrength ();
			default:
				return 0.0f;
		}
	}

	/// <summary>
	/// Gets the BreathingState :
	/// expiration if patient is blowing and inspiration is over.
	/// inspiration if patient wasn't blowing for less than inspiration time.
	/// holding breath if the patient isn't blowing for more than the inpiration time.
	/// </summary>
	public BreathingState GetInputState(){
		lastState = state;
		
		if (isBlowing () && state!=BreathingState.INSPIRATION) {
			state= BreathingState.EXPIRATION;
		} else {
			if(lastState == BreathingState.EXPIRATION){
				endExpirationTime=DateTime.Now;
			}
			double secondsSinceEndExpiration=(DateTime.Now-endExpirationTime).TotalSeconds;
			if(secondsSinceEndExpiration <= exercice.ActualBreathing.InspirationTime){
				state=BreathingState.INSPIRATION;
			}
			else {
				state=BreathingState.HOLDING_BREATH;
			}
		}

		return state;
	}

	/// <summary>
	/// Returns whether the patient is blowing or not.
	/// </summary>
	/// <returns><c>true</c>, if the patient is blowing, <c>false</c> otherwise.</returns>
	protected abstract bool isBlowing();

	/// <summary>
	/// Determines whether the patient is holding the moving input or not.
	/// </summary>
	public abstract bool IsMoving();

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Gets the strength of expiration.
	/// 0.0f, it means that the patient is not blowing.
	/// </summary>
	public abstract float GetExpirationStrength();
}


