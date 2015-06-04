using System;
using System.Collections.Generic;

/// <summary>
/// This class implements a decreasing "Drainage autogène" which consists of 3 sequence of breathings
/// which are made at 3 different volumes. The "Decreasing" is because we start at the highest volume.
/// </summary>
public class DecreasingAutogenicDrainage:Exercice
{

	/// <summary>
	/// Initializes a new instance of the <see cref="DecreasingAutogenicDrainage"/> class.
	/// </summary>
	/// <param name="nbBreathingsHigh">Number of breathing done at high pulmonary volume.</param>
	/// <param name="nbBreathingsMedium">Number of breathing done at medium pulmonary volume.</param>
	/// <param name="nbBreathingsLow">Number of breathing done at low pulmonary volume.</param>
	/// <param name="inspirationTime">The time in seconds the patient should be inpiring in each breathing.</param>
	/// <param name="holdingBreathTime">The time in seconds when the player has to hold his breath (between inspiration and expiration).</param>
	/// <param name="expirationMinTime">The minimum number of seconds the patient should be expiring in each breathing.</param>
	/// <param name="volumeRange">
	/// The volume range in which the patient should do a breathing.
	/// The high breathings will start from full lungs and deacrease of this value.
	/// The medium breathings will start at 0.5 + (this value)/2 and deacrease of this value.
	/// The low breathings will start from this value and deacrease until empty lungs.
	/// </param>
	public DecreasingAutogenicDrainage (int nbBreathingsHigh, int nbBreathingsMedium, int nbBreathingsLow, float inspirationTime, float holdingBreathTime, float expirationMinTime, float volumeRange)
	{
		int nbBreahthings = nbBreathingsHigh + nbBreathingsMedium + nbBreathingsLow;

		//Transition from low to high
		breathings = new List<Breathing> (nbBreahthings);
		float volumeMax = 1.0f;
		float lastVolume = 0.0f;
		float transitionInspirationTime = (volumeMax - lastVolume) / (volumeRange / inspirationTime);
		float endVolume=1.0f-volumeRange;		
		breathings.Add(new Breathing(lastVolume, volumeMax, endVolume, transitionInspirationTime, holdingBreathTime, expirationMinTime, BreathingState.HOLDING_BREATH));
		lastVolume = endVolume;
		//High volume
		for (int i=1; i<nbBreathingsHigh-1; i++) {
			breathings.Add(new Breathing(lastVolume, volumeMax, endVolume, inspirationTime, holdingBreathTime, expirationMinTime));
			lastVolume=endVolume;
		}
		//Transition from high to medium volume		
		endVolume = 0.5f - volumeRange / 2.0f;
		float transitionExpirationMinTime = (volumeMax - endVolume) / (volumeRange / expirationMinTime);
		breathings.Add (new Breathing (lastVolume, volumeMax, endVolume, inspirationTime, holdingBreathTime, transitionExpirationMinTime));
		lastVolume = endVolume;
		//Medium volume
		volumeMax = 0.5f + volumeRange / 2.0f;
		for (int i=0; i<nbBreathingsMedium-1; i++) {
			breathings.Add(new Breathing(lastVolume, volumeMax, endVolume, inspirationTime, holdingBreathTime, expirationMinTime));
			lastVolume=endVolume;
		}
		//Transition from medium to low volume
		endVolume = 0.0f;
		transitionExpirationMinTime = (volumeMax - endVolume) / (volumeRange / expirationMinTime);
		breathings.Add (new Breathing (lastVolume, volumeMax, endVolume, inspirationTime, holdingBreathTime, transitionExpirationMinTime));
		lastVolume = endVolume;
		//Low volume
		for (int i=0; i<nbBreathingsLow; i++) {
			breathings.Add(new Breathing(lastVolume, 0.5f, endVolume, inspirationTime, holdingBreathTime, expirationMinTime));
			lastVolume=endVolume;
		}

		indexActualBreathing = 0;
	}
}


