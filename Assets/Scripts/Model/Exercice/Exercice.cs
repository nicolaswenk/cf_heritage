using System;
using System.Collections.Generic;

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
	/// <summary>The minimum time (in sec) the exercice should last (repeating the breathings sequence).</summary>
	protected float minTime;
	/// <summary>The percentage of an expiration the patient have to do to validate the actual breathing and passing to the next one.</summary>
	protected float factorMinExpirationForValidation=0.75f;
	//TODO Doc
	private float volumeMaxCalibrated=10.0f;
	
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
	/// <param name="deltaTime">The time in seconds elapsed since the last call.</param>
	public void CheckProgress(InputController_I inputController, float deltaTime){
		volume=ActualBreathing.CheckProgress (inputController.GetInputState(), inputController.GetStrength(), volume, volumeMaxCalibrated, deltaTime);
		//The breathing has ended (already back to expiration) and the volume value isn't up to date.
		//We have to do a new "CheckProgress" on a non-ended breathing.
		if (ActualBreathing.IsEnded){

			//TODO: Save the breathings data here.

			if(ActualBreathing.ExpirationPercentage >= factorMinExpirationForValidation) {
				ActualBreathing.ResetTo(BreathingState.INSPIRATION);
				indexActualBreathing++;
				indexActualBreathing%=breathings.Count;
			}
			else{
				ActualBreathing.ResetTo(BreathingState.EXPIRATION);
			}
			volume=ActualBreathing.CheckProgress (inputController.GetInputState(), inputController.GetStrength(), volume, volumeMaxCalibrated, deltaTime);
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
	/// Gets the breathing state of the patient (whether he's expiring, inspiring, holding breath, ...)
	/// </summary>
	public BreathingState State {
		get{ return ActualBreathing.State;}
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

	public float VolumeMaxCalibrated{
		set{ volumeMaxCalibrated = value;}
	}
}


