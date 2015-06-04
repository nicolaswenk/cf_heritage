//------------------------------------------------------------------------------
//
//	FlapiUnity::Flapi
//
//	Author : Cyriaque Skrapits <cyriaque.skrapits[at]he-arc.ch>
//	Version : 1.0
//
//	Port of the Flapi audio library for http://fibrosekystique.net/.
//	This library analyzes the audio input, find the ticks recorded with
//	a PEP/Flutter and gives their frequency.
//
//	TODO : 	Implement a high pass filter with rolloff (see Audacity's) in order
//			to avoid the high passes cascade in Flapi::Process().
//			(See FlapiFilters header.)
//
//	TODO :	Don't save everything in a buffer. Just work on the last seconds.
//			This would save some memory.
//
//	TODO :	Store ticks in a hashmap with the timestamp -> less memory usage.
//
//	TODO :	See if TICKS_PER_SEC_MIN/MAX must be specified by user. If so,
//			add a public variable for this situation.
//
//------------------------------------------------------------------------------
using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FlapiUnity
{
	[RequireComponent(typeof(AudioSource))]
	public class Flapi : MonoBehaviour
	{
		//
		// Private variables
		//

		private static int				DEFAULT_SAMPLE_RATE = 44100;
		private static float			THRESHOLD_MIN = 2.0f;			// Min ratio between noise and tick
		private static float			THRESHOLD_MAX = 10.0f;			// Max ratio between noise and tick
		private static readonly int		TICKS_PER_SEC_MIN = 6;			// Lowest ticks frequency acceptable
		private static readonly int		TICKS_PER_SEC_MAX = 26;			// Higher ticks frequency acceptable
		private static readonly float	TICK_LENGTH = 0.018f; 			// Assume a tick last 0.005 sec

		private static int				lastAnalyzedSample;
		private static bool				sourceIsFile;					// If the audio source is a file, filters at the beginning
		private static int				tickMinDelta;					// Samples in high tick frequency (sample in a TICKS_PER_SEC_MAX Hz tick)
		private static int				tickSamples;					// Number of samples in a tick

		private static float[]			_buffer;						// Recorded audio data (really necessary ?)
		private static float			_frequency;						// Ticks frequency
		private static int				_sampleRate;
		private static float			_threshold = THRESHOLD_MIN;		// Ratio signal/noise to use in order to detect ticks
		private static bool[]			_ticks;							// Ticks detected

		
		
		//
		// Public variables
		//

		public static readonly int		FREQUENCY_MAX = TICKS_PER_SEC_MAX;

		public static bool blowing { 
			get {
				return _frequency > (float)TICKS_PER_SEC_MIN;
			} 
		}

		public static float[] buffer {
			get {
				return _buffer;
			}
		}
		
		public static float frequency {
			get {
				return _frequency;
			}
		}

		public static float threshold {
			get {
				return _threshold;
			}
			set {
				_threshold = value < THRESHOLD_MIN ? THRESHOLD_MIN : value > THRESHOLD_MAX ? THRESHOLD_MAX : value;
			}
		}

		public static bool[] ticks {
			get {
				return _ticks;
			}
		}

		public struct FlapiMicrophone
		{
			public string	name;
			public int		minSamplingRate;
			public int		maxSamplingRate;
		}

		/// <summary>
		/// Analyze upcoming audio signal and find the ticks.
		/// 
		/// The algorithm is simple : we analyze the upcoming signal in many parts with a size of a half tick (~44 samples at 44100 Hz);
		/// We take each time two parts of samples and if the first part has less gain than the second's,
		/// we consider that there is a tick in the second.
		/// 
		/// When the tick has been found, we approximatively skip to a place where the next tick could be
		/// in order to optimise the research.
		/// Otherwise we continue from the second part.
		/// </summary>
		/// <param name="audio">Audio source (Unity).</param>
		public static void Analyze (AudioSource audio)
		{
			int currentSample = audio.timeSamples;
			int samples = currentSample - lastAnalyzedSample;

			// We must analyze a range which can contain at least one tick.
			if (samples < tickMinDelta || samples == 0)
				return;

			// Copy the audio data in our buffer.
			float[] data = new float[samples];
			audio.GetOutputData (data, 0);
			data.CopyTo (_buffer, lastAnalyzedSample);

			// Ignore filters processes if audio source is a file.
			// The file is already processed.
			if (!sourceIsFile)
				Process (data);

			float[] means = new float[2]; // Two parts of samples.

			int d = tickSamples / 2; // Half tick.
			int parts = samples / d; // Number of parts to process.

			// Process the parts.
			for (int i = 0; i < parts - 1;) {
				means [0] = FlapiUtils.AbsMean (data, i * d, (i + 1) * d - 1); // Mean gain of first part.
				means [1] = FlapiUtils.AbsMean (data, (i + 1) * d, (i + 2) * d - 1); // Mean gain of second part.

				// Second part has more gain than first's ?
				if (means [1] > _threshold * means [0]) {
					_ticks [lastAnalyzedSample + (i + 2) * d] = true; // Here's a tick !
					i += 2 * d; //(tickMinDelta / d - 1); // Process to the approximative next tick.
				} else {
					i++;
				}
			}

			_frequency = GetTicksFrequency (currentSample);
			lastAnalyzedSample = currentSample - (samples % tickMinDelta); // Next call skip after the last analyzed buffer.
		}

		/// <summary>
		/// Gets the audio frequency.
		/// </summary>
		/// <returns>The audio frequency.</returns>
		/// <param name="audio">Audio.</param>
		public static float GetAudioFrequency (AudioSource audio)
		{
			float fundamentalFrequency = 0.0f;
			float[] data = new float[1024];

			audio.GetSpectrumData (data, 0, FFTWindow.BlackmanHarris);

			float s = 0.0f;
			int i = 0;
			for (int j = 1; j < data.Length; j++) {
				if (s < data [j]) {
					s = data [j];
					i = j;
				}
			}

			fundamentalFrequency = i * _sampleRate / data.Length;

			return fundamentalFrequency;
		}

		public static FlapiMicrophone GetMicrophone (int index)
		{
			return GetMicrophones () [index];
		}
		
		public static FlapiMicrophone[] GetMicrophones ()
		{
			FlapiMicrophone[] microphones;
			string[] devices;
			int devicesAvailables;
			int deviceMinFrequency;
			int deviceMaxFrequency;
			
			devices = UnityEngine.Microphone.devices;
			devicesAvailables = devices.Length;
			
			microphones = new FlapiMicrophone[devicesAvailables];

			for (int i = 0; i < devicesAvailables; i++) {
				UnityEngine.Microphone.GetDeviceCaps (devices [i], out deviceMinFrequency, out deviceMaxFrequency);
				microphones [i].name = devices [i];
				microphones [i].minSamplingRate = deviceMinFrequency;
				microphones [i].maxSamplingRate = deviceMaxFrequency;
			}
			
			return microphones;
		}

		/// <summary>
		/// Catch every ticks in the past second in the buffer and return the ticks frequency.
		/// </summary>
		/// <returns>The ticks frequency.</returns>
		/// <param name="timeSamples">Current audio source's time sample.</param>
		private static float GetTicksFrequency (int timeSamples)
		{
			int last = 0;
			float f = 0.0f;
			int t = 0;

			int to = timeSamples;
			int from = to - _sampleRate;
			if (from < 0)
				return 0.0f;

			int delta = tickMinDelta - tickSamples;

			for (int i = from; i < to; i++) {
				if (_ticks [i]) {
					t++;
					f += _sampleRate / (float)(i - last);
					last = i;
					i += delta;
				}
			}

			return f / t;
		}

		/// <summary>
		/// Apply some filters to the audio signal.
		/// </summary>
		/// <param name="data">Signal.</param>
		private static void Process (float[] data)
		{
			FlapiFilters.RemoveDC (data);

			FlapiFilters.HighPassFilterAlpha (data, 2000f, 0.2f, _sampleRate);
			FlapiFilters.HighPassFilterAlpha (data, 5000f, 0.2f, _sampleRate);
			FlapiFilters.HighPassFilterAlpha (data, 10000f, 0.2f, _sampleRate);
		}

		/// <summary>
		/// Start the record.
		/// </summary>
		/// <param name="audio">Audio source (Unity).</param>
		/// <param name="microphone">Microphone.</param>
		/// <param name="seconds">Record length in seconds.</param>
		public static void Start (AudioSource audio, FlapiMicrophone microphone, int seconds)
		{
			sourceIsFile = false;
			_sampleRate = microphone.maxSamplingRate;
	
			// Check sample rate.
			// A zero value indicates that the microphone can support any frequency.
			if (_sampleRate == 0)
				_sampleRate = DEFAULT_SAMPLE_RATE;

			tickMinDelta = _sampleRate / TICKS_PER_SEC_MAX;
			tickSamples = (int)(TICK_LENGTH / 2 * _sampleRate);

			_ticks = new bool[seconds * _sampleRate];
			_buffer = new float[seconds * _sampleRate];

			//StartAudioFile (audio, "blowshort.wav");
			StartMicrophone (audio, microphone, seconds);
			Debug.Log (microphone.name);

			// Start the process, no playback.
			audio.Play ();
			audio.volume = 0.0f;
		}

		//---------------------------Debug use only--------------------------------
		private static void StartAudioFile (AudioSource audio, string assetFile)
		{
			sourceIsFile = true;

			WWW www = new WWW ("file://" + Application.dataPath + "/" + assetFile);
			audio.clip = www.audioClip;
			
			while (!audio.clip.isReadyToPlay) {
			}

			// Apply filters to the audio file.
			float[] input = new float[audio.clip.samples];
			audio.clip.GetData (input, 0);
			Process (input);
			audio.clip.SetData (input, 0);
		}
		//-------------------------------------------------------------------------

		private static void StartMicrophone (AudioSource audio, FlapiMicrophone microphone, int seconds)
		{
			sourceIsFile = false;

			RequireAuthorization ();

			string device = microphone.name;
			audio.clip = UnityEngine.Microphone.Start (device, false, seconds, _sampleRate);

			// Simple trick to ensure the microphone and the audio card are synchronized.
			// Avoid laggy records.
			while (!(UnityEngine.Microphone.GetPosition(device) > 0)) {
			}
			;
		}

		static IEnumerator RequireAuthorization ()
		{
			yield return Application.RequestUserAuthorization (UserAuthorization.Microphone);
			if (Application.HasUserAuthorization (UserAuthorization.Microphone)) {
			} else {
			}
		}
	}
}