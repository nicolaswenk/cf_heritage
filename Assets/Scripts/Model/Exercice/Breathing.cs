using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the center of an exercice. All exercices are made of a sequence of breathings.
/// It describes the ideal breathing the patient should do and can check his progress.
/// </summary>
public class Breathing
{
	/// <summary>The volume at which we start the breathing (before inspiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: before inspiration; is irrelevant).</summary>
	private float startVolume;
	/// <summary>The volume at which we end the inspiration and start the holding breath. As all volume, 0 is for empty (which in this case: after inspiration; is irrelevant) lungs and 1 for full ones.</summary>
	private float maxVolume;
	/// <summary>The volume at which we end the breathing (the end of the expiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: after expiration; is irrelevant).</summary>
	private float endVolume;
	/// <summary>The time (is seconds) the patient should inspiring (from <see cref="startVolume"/> to <see cref="maxVolume"/>).</summary>
	private float inspirationTime;
	/// <summary>The time (is seconds) the patient should hold his breath after an inspiration.</summary>
	private float holdingBreathTime;
	/// <summary>The time (is seconds) the patient should expiring (from <see cref="maxVolume"/> to <see cref="endVolume"/>).</summary>
	private float expirationTime;
	/// <summary>The breathing state of the patient (whether he's expiring, inspiring, holding breath, ...).</summary>
	private BreathingState state;
	/// <summary>The last breathing state of the patient (from the las <see cref="CheckProgress"/> call) .</summary>
	private BreathingState lastState;
	/// <summary>This dictionnary store all the state's realisation percentages. We can access them with the BreathingState.</summary>
	private Dictionary<BreathingState, float> dictStatePercentages=new Dictionary<BreathingState, float>(3);
	/// <summary>This dictionnary store all the state's start time. We can access them with the BreathingState.</summary>
	private Dictionary<BreathingState, DateTime> dictStateStartTimes=new Dictionary<BreathingState, DateTime>(3);
	/// <summary>Tells if the patient has already started this breathing (false at this object creation).</summary>
	private bool hasBreathingStarted;
	/// <summary>Tells if this breathing has ended (if the expiration is over).</summary>
	private bool isBreathingEnded;
	//TODO Have some more precise value
	/// <summary>Seems to be a median value from https://van.physics.illinois.edu/qa/listing.php?id=786 </summary>
	private const float supposedPatientMaxVolume=2100.0f;

	private const float strengthToVolumeFactor=supposedPatientMaxVolume/20.0f;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Breathing"/> class. A breathing is composed of three state. The first is "inpiration", the second in "holding breath" and the last one is "expiration".
	/// The instantiated breathing start at "INSPIRATION" state
	/// </summary>
	/// <param name="startVolume">The volume at which we start the breathing (before inspiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: before inspiration; is irrelevant).</param>
	/// <param name="maxVolume">The volume at which we end the inspiration and start the holding breath. As all volume, 0 is for empty (which in this case: after inspiration; is irrelevant) lungs and 1 for full ones.</param>
	/// <param name="endVolume">The volume at which we end the breathing (the end of the expiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: after expiration; is irrelevant).</param>
	/// <param name="holdingBreathTime">The time (is seconds) the patient should hold his breath after an inspiration.</param>
	/// <param name="inspirationTime">The time (is seconds) the patient should inspiring (from <see cref="startVolume"/> to <see cref="maxVolume"/>).</param>
	/// <param name="expirationTime">The time (is seconds) the patient should expiring (from <see cref="maxVolume"/> to <see cref="endVolume"/>).</param>
	public Breathing (float startVolume, float maxVolume, float endVolume, float inspirationTime, float holdingBreathTime, float expirationTime)
	:this(startVolume, maxVolume, endVolume, inspirationTime, holdingBreathTime, expirationTime, BreathingState.INSPIRATION){
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Breathing"/> class starting at the given state.
	/// A breathing is composed of three state. The first is "inspiration", the second in "holding breath" and the last one is "expiration".
	/// </summary>
	/// <param name="startVolume">The volume at which we start the breathing (before inspiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: before inspiration; is irrelevant).</param>
	/// <param name="maxVolume">The volume at which we end the inspiration and start the holding breath. As all volume, 0 is for empty (which in this case: after inspiration; is irrelevant) lungs and 1 for full ones.</param>
	/// <param name="endVolume">The volume at which we end the breathing (the end of the expiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: after expiration; is irrelevant).</param>
	/// <param name="holdingBreathTime">The time (is seconds) the patient should hold his breath after an inspiration.</param>
	/// <param name="inspirationTime">The time (is seconds) the patient should inspiring (from <see cref="startVolume"/> to <see cref="maxVolume"/>).</param>
	/// <param name="expirationTime">The time (is seconds) the patient should expiring (from <see cref="maxVolume"/> to <see cref="endVolume"/>).</param>
	/// <param name="startState">The state at which this breathing starts.</param>
	public Breathing (float startVolume, float maxVolume, float endVolume, float inspirationTime, float holdingBreathTime, float expirationTime, BreathingState startState){
		this.startVolume = startVolume;
		this.maxVolume = maxVolume;
		this.endVolume = endVolume;
		this.inspirationTime = inspirationTime;
		this.holdingBreathTime = holdingBreathTime;
		this.expirationTime=expirationTime;
		ResetTo (startState);
	}

	/// <summary>
	/// Reset this breathing to the given initial state.
	/// </summary>
	/// <param name="stateToResetAt">State to reset at.</param>
	public void ResetTo(BreathingState stateToResetAt){		
		this.state = stateToResetAt;
		this.lastState = GetSupposedLastState (stateToResetAt);
		this.dictStateStartTimes.Clear ();
		this.dictStateStartTimes [stateToResetAt] = DateTime.Now;
		this.dictStatePercentages.Clear ();
		this.dictStatePercentages.Add (BreathingState.INSPIRATION, 0.0f);
		this.dictStatePercentages.Add (BreathingState.HOLDING_BREATH, 0.0f);
		this.dictStatePercentages.Add (BreathingState.EXPIRATION, 0.0f);
		this.hasBreathingStarted = false;
		this.isBreathingEnded = false;
	}

	/// <summary>
	/// Gets the usual state that precedes the given one.
	/// </summary>
	/// <returns>The usual state that precedes the given one.</returns>
	/// <param name="state">The state we want to know its prior.</param>
	public static BreathingState GetSupposedLastState(BreathingState state){
		switch (state) {
		case BreathingState.INSPIRATION:
			return BreathingState.EXPIRATION;
		case BreathingState.HOLDING_BREATH:
			return BreathingState.INSPIRATION;
		case BreathingState.EXPIRATION:
			return BreathingState.HOLDING_BREATH;
		}
		return BreathingState.INSPIRATION;
	}

	public float CallibrateStrength(float strength, float volumeMaxCalibrated){
		Debug.Log (volumeMaxCalibrated + " -> " + supposedPatientMaxVolume);
		float callibratedStength = strength * (supposedPatientMaxVolume/volumeMaxCalibrated);
		return callibratedStength;
	}

	/// <summary>
	/// Check the progress of the patient in this breathing and compute his actual lungs volume.
	/// </summary>
	/// <returns>The patient actual lungs volume (0 for empty and 1 for full).</returns>
	/// <param name="newState">The new breathing state (retrieve from the input controller). Can be the same as the actual state.</param>
	/// <param name="strength">The strength of the player inhale or exhale (depends on <see cref="newState"/>).</param>
	/// <param name="volumeRatio">The old volume value which will be updated and returned.</param>
	/// <param name="deltaTime">The time in seconds elapsed since the last call.</param>
	public float CheckProgress(BreathingState newState, float strength, float volumeRatio, float volumeMaxCalibrated, float deltaTime){	
		strength = CallibrateStrength (strength, volumeMaxCalibrated);
		if (isBreathingEnded) {
			return volumeRatio;
		}
		lastState = state;
		state = newState;

		if(lastState==GetSupposedLastState(state)){
			if(state==BreathingState.INSPIRATION && hasBreathingStarted){
				isBreathingEnded=true;
				return volumeRatio;
			}
			if(!dictStateStartTimes.ContainsKey(state)){
				dictStateStartTimes.Add(state, DateTime.Now);
			}
			CheckProgress(state, strength, volumeRatio, volumeMaxCalibrated, deltaTime);
		}
		else if(lastState == state){
			hasBreathingStarted=true;
			switch (state) {
				
			case BreathingState.EXPIRATION://We expire
				volumeRatio-=strength*deltaTime*ExpirationSpeed;
				if(volumeRatio<endVolume){
					volumeRatio=endVolume;
				}
				dictStatePercentages[state]=(volumeRatio-maxVolume)/(endVolume-maxVolume);
				break;
				
			case BreathingState.INSPIRATION://We inspire
				volumeRatio+=strength*deltaTime*InspirationSpeed;
				if(volumeRatio>maxVolume){
					volumeRatio=MaxVolume;
				}
				float newPercentage=(volumeRatio-startVolume)/(maxVolume-startVolume);
				dictStatePercentages[state]=newPercentage;
				break;
				
			case BreathingState.HOLDING_BREATH://We hold our breath
				dictStatePercentages[state]=((float)(DateTime.Now-dictStateStartTimes[state]).TotalSeconds)/(holdingBreathTime);
				if(dictStatePercentages[state]>1.0f){
					dictStatePercentages[state]=1.0f;
				}
				break;
			}
		}
		else{
			state=GetSupposedLastState(state);
		}

		return volumeRatio;
	}
	
	/// <summary>
	/// Gets the volume at which we start the breathing (before inspiration).
	/// As all volume, 0 is for empty lungs and 1 for full ones (which in this case: before inspiration; is irrelevant).
	/// </summary>
	public float StartVolume {
		get{ return startVolume;}
	}

	/// <summary>
	/// Gets the volume at which we end the inspiration and start the holding breath.
	/// As all volume, 0 is for empty (which in this case: after inspiration; is irrelevant) lungs and 1 for full ones.	
	/// </summary>
	public float MaxVolume {
		get{ return maxVolume;}
	}

	/// <summary>
	/// Gets the volume at which we end the breathing (the end of the expiration).
	/// As all volume, 0 is for empty lungs and 1 for full ones (which in this case: after expiration; is irrelevant).
	/// </summary>
	public float EndVolume {
		get{ return endVolume;}
	}

	/// <summary>
	/// Gets the time (is seconds) the patient should inspiring (from <see cref="startVolume"/> to <see cref="maxVolume"/>).
	/// </summary>
	public float InspirationTime {
		get { return inspirationTime;}
	}

	/// <summary>
	/// Gets the time (is seconds) the patient should hold his breath after an inspiration.
	/// </summary>
	public float HoldingBreathTime {
		get { return holdingBreathTime;}
	}

	/// <summary>
	/// Gets the time (is seconds) the patient should expiring (from <see cref="maxVolume"/> to <see cref="endVolume"/>).
	/// </summary>
	public float ExpirationTime {
		get { return expirationTime;}
	}

	/// <summary>
	/// Gets the expiration speed (the time in seconds needed while expiring to empty full lungs).
	/// </summary>
	public float ExpirationSpeed{
		get {return (maxVolume-endVolume)/expirationTime;}
	}

	/// <summary>
	/// Gets the inspiration speed (the time in seconds needed while expiring to fill empty lungs).
	/// </summary>
	public float InspirationSpeed{
		get {return (maxVolume-startVolume)/inspirationTime;}
	}

	/// <summary>
	/// Gets the duration of this breathing (in seconds).
	/// </summary>
	public float Duration{
		get{
			return inspirationTime+holdingBreathTime+expirationTime;
		}
	}
	
	/// <summary>The breathing state of the patient (whether he's expiring, inspiring, holding breath, ...).</summary>
	public BreathingState State{
		get{
			return state;
		}
	}
	
	/// <summary>The expiration's realisation percentage.</summary>
	public float ExpirationPercentage{
		get{
			return dictStatePercentages[BreathingState.EXPIRATION];
		}
	}
	
	/// <summary>The "holding breath"'s realisation percentage.</summary>
	public float HoldingBreathPercentage{
		get{
			return dictStatePercentages[BreathingState.HOLDING_BREATH];
		}
	}
	
	/// <summary>Tells if this breathing has ended (if the expiration is over).</summary>
	public bool IsEnded{
		get{
			return isBreathingEnded;
		}
	}
	
	public static float StrengthToVolumeFactor{
		get{
			return strengthToVolumeFactor;
		}
	}
	
	public static float SupposedPatientMaxVolume{
		get{
			return supposedPatientMaxVolume;
		}
	}
}

