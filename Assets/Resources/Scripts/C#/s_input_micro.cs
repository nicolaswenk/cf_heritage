using FlapiUnity;
using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
public class s_input_micro : MonoBehaviour
{
	private AudioSource source;
	private float frequency;

	public float f_loudness = 0;


	void Start ()
	{
		// Hum... Is this the right method ?
		AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource> ();
		source = sources [0];

		// Start Flapi on microphone 1 for 2 minutes.
		Flapi.FlapiMicrophone microphone = Flapi.GetMicrophone (1);
		Flapi.Start (source, microphone, 120);
	}

	void Update ()
	{
		Flapi.Analyze (source);
		frequency = Flapi.frequency / 26.0f;

		if (float.IsNaN (frequency))
			frequency = 0.0f;

		if (frequency > 1.0f)
			frequency = 1.0f;

		s_input.f_pressure = frequency;
	}
}