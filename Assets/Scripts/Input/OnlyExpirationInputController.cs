//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18444
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public abstract class OnlyExpirationInputController : IOController_I
{
	protected abstract bool isBlowing();
	public abstract bool IsMoving();
	public abstract void Update();
	public abstract float GetExpirationStrength();
	
	private InputState lastState=InputState.HOLDING_BREATH;
	private InputState state=InputState.HOLDING_BREATH;
	private DateTime endExpirationTime;

	private ParameterManager parameterManager;

	private DrainageAutogene exercice;

	public OnlyExpirationInputController(ParameterManager parameterManager, DrainageAutogene exercice){
		this.parameterManager = parameterManager;
		this.exercice = exercice;
	}
	
	
	public float GetStrength(){
		switch (state) {
			case InputState.INSPIRATION:
				return 1.0f;
			case InputState.EXPIRATION:
			case InputState.STRONG_EXPIRATION:
				return GetExpirationStrength ();
			default:
				return 0.0f;
		}
	}

	
	public InputState GetInputState(){
		lastState = state;
		
		if (isBlowing () && state!=InputState.INSPIRATION) {
			if(GetStrength()>=parameterManager.StrongBreathValue){
				state= InputState.STRONG_EXPIRATION;
			}
			else{
				state= InputState.EXPIRATION;
			}
		} else {
			if(lastState == InputState.EXPIRATION || lastState == InputState.STRONG_EXPIRATION){
				endExpirationTime=DateTime.Now;
			}
			double secondsSinceEndExpiration=(DateTime.Now-endExpirationTime).TotalSeconds;
			if(secondsSinceEndExpiration <= exercice.ActualRespiration.InspirationTime){
				state=InputState.INSPIRATION;
			}
			else {
				state=InputState.HOLDING_BREATH;
			}
		}

		return state;
	}
}


