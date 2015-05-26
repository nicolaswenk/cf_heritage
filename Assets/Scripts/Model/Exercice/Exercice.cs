using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describe the behaviour of a therapeutical exercice.
/// An exercice is basically a sequence of breathings which loops for at least
/// the minimum defined time <see cref="minTime"/> .
/// All other exercices we want to implement in the SG must inherits this class.
/// </summary>
public abstract class Exercice
{
	/// <summary>The list of breathings in a cycle of this exercice.</summary>
	protected List<Breathing> breathings;
	/// <summary>The index of the breathing in <see cref="breathings"/> the patient is actually doing.</summary>
	protected int indexActualBreathing;
	/// <summary>The state of the patient (whether he's expiring, inspiring, holding breath, ...).</summary>
	protected InputState state=InputState.HOLDING_BREATH;
	/// <summary>The last state of the patient (from the las <see cref="CheckProgress"/> call) .</summary>
	protected InputState lastState=InputState.HOLDING_BREATH;
	/// <summary>The percentage of an expiration the patient have to do to validate the actual breathing and passing to the next one.</summary>
	protected float factorMinExpirationForValidation=0.75f;
	/// <summary>The minimum time (in sec) the exercice should last (repeating the breathings sequence).</summary>
	protected float minTime;
	
	/// <summary>
	/// 0 -> Empty lungs ;
	/// 1 -> Full lungs.
	/// </summary>
	protected float volume=1.0f;
	
	/// <summary>
	/// Using the input contoller, this methods update the computed volume the pation should have in his lungs.
	/// If the patient reach correctly the end volume, we go to the nexte breathing.
	/// </summary>
	/// <param name="inputController">Input controller.</param>
	public void CheckProgress(InputController_I inputController){
		lastState = state;
		state = inputController.GetInputState ();
		
		//We expire
		if (state == InputState.EXPIRATION || state == InputState.STRONG_EXPIRATION) {
			volume-=inputController.GetStrength()*Time.deltaTime*ActualBreathing.ExpirationSpeed;
			if(volume<ActualBreathing.EndVolume){
				volume=ActualBreathing.EndVolume;
			}
		}
		
		//We inspire
		if (state == InputState.INSPIRATION) {
			volume+=inputController.GetStrength()*Time.deltaTime*ActualBreathing.InspirationSpeed;
			if(volume>ActualBreathing.MaxVolume){
				volume=ActualBreathing.MaxVolume;
			}
			//We started to inspire
			if (lastState != state) {
				float volumeExpired=ActualBreathing.MaxVolume-volume;
				float volumeToExpire=ActualBreathing.MaxVolume-ActualBreathing.EndVolume;
				
				//We expire enough to validation this breathing.
				if(volumeExpired>=factorMinExpirationForValidation*volumeToExpire){
					indexActualBreathing++;
					indexActualBreathing%=breathings.Count;
				}
			}
		}
	}
	
	/// <summary>
	/// Gets the volume transition the patient will have to do (increasing volume, decreasing volume or none).
	/// </summary>
	public Transition GetTransition(){
		float v1=breathings[indexActualBreathing].EndVolume;
		float lastEndVolume;
		if (indexActualBreathing > 0) {
			lastEndVolume=breathings[indexActualBreathing-1].EndVolume;
		}else{
			lastEndVolume=breathings[breathings.Count-1].EndVolume;
		}
		
		Transition transition= Transition.NONE;
		if (v1 < lastEndVolume) {
			transition=Transition.VOLUME_DECREASE;
		} else if(v1 > lastEndVolume) {
			transition=Transition.VOLUME_INCREASE;
		}
		lastEndVolume = v1;
		return transition;
	}
	
	/// <summary>
	/// Gets the min (end) volume of the breathing the patient is actually doing.
	/// </summary>
	public float ActualMin {
		get{ return this.breathings [this.indexActualBreathing].EndVolume;}
	}
	
	/// <summary>
	/// Gets the max volume of the breathing the patient is actually doing.
	/// </summary>
	public float ActualMax {
		get{ return this.breathings [this.indexActualBreathing].MaxVolume;}
	}
	
	/// <summary>
	/// Gets the actual computed pulmonary volume of the player.
	/// This variable is update each time we call <see cref="CheckProgress"/> after the inputController.
	/// </summary>
	public float Volume {
		get{ return this.volume;}
	}
	
	/// <summary>
	/// Gets the breathing the patient is actually doing.
	/// </summary>
	public Breathing ActualBreathing {
		get{ return breathings [indexActualBreathing];}
	}
	
	/// <summary>
	/// Gets the list of breathings in a cycle of this exercice.
	/// </summary>
	public List<Breathing> Breathings {
		get{ return breathings;}
	}
	
	/// <summary>
	/// Gets the state of the patient (whether he's expiring, inspiring, holding breath, ...)
	/// </summary>
	public InputState State {
		get{ return state;}
	}
	
	/// <summary>
	/// Gets the ideal speed of the expiration for the actual breathing (the time in seconds needed from full lungs to empty ones if the patient is expiring as awaited).
	/// </summary>
	public float IdealExpirationSpeed {
		get { return ActualBreathing.ExpirationSpeed;}
	}
	
	/// <summary>
	/// Gets the ideal speed of the inspiration for the actual breathing (the time in seconds needed from empty lungs to full ones if the patient is inspiring as awaited).
	/// </summary>	
	public float IdealInspirationSpeed{
		get { return ActualBreathing.InspirationSpeed;}
	}

	/// <summary>
	/// Gets the duration (in seconds) of the exerice (based on the duration of its breathings).
	/// </summary>
	public float Duration {
		get {
			float duration = 0.0f;
			foreach(Breathing breathing in breathings){
				duration += breathing.Duration;
			}
			return duration;
		}
	}
}


