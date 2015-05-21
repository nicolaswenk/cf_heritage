using System;
using System.Collections;

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

	/// <summary>
	/// Initializes a new instance of the <see cref="Breathing"/> class. A breathing is composed of three state. The first is "inpiration", the second in "holding breath" and the last one is "expiration".
	/// </summary>
	/// <param name="startVolume">The volume at which we start the breathing (before inspiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: before inspiration; is irrelevant).</param>
	/// <param name="maxVolume">The volume at which we end the inspiration and start the holding breath. As all volume, 0 is for empty (which in this case: after inspiration; is irrelevant) lungs and 1 for full ones.</param>
	/// <param name="endVolume">The volume at which we end the breathing (the end of the expiration). As all volume, 0 is for empty lungs and 1 for full ones (which in this case: after expiration; is irrelevant).</param>
	/// <param name="holdingBreathTime">The time (is seconds) the patient should hold his breath after an inspiration.</param>
	/// <param name="inspirationTime">The time (is seconds) the patient should inspiring (from <see cref="startVolume"/> to <see cref="maxVolume"/>).</param>
	/// <param name="expirationTime">The time (is seconds) the patient should expiring (from <see cref="maxVolume"/> to <see cref="endVolume"/>).</param>
	public Breathing (float startVolume, float maxVolume, float endVolume, float inspirationTime, float holdingBreathTime, float expirationTime){
		this.startVolume = startVolume;
		this.maxVolume = maxVolume;
		this.endVolume = endVolume;
		this.inspirationTime = inspirationTime;
		this.holdingBreathTime = holdingBreathTime;
		this.expirationTime=expirationTime;
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
}

