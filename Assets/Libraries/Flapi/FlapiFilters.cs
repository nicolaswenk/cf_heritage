//------------------------------------------------------------------------------
//
//	FlapiUnity::FlapiFilters
//
//	Author : Cyriaque Skrapits <cyriaque.skrapits[at]he-arc.ch>
//	Version : 1.0
//
//	Provides some audio filters for the sound processing in Flapi.
//	Some of them were added in test phase and are not used anymore. However,
//	they were left in case of need.
//
//	TODO : 	Implement a high pass filter with rolloff (see Audacity's) in order
//			to avoid the high passes cascade in Flapi::Process().
//
//------------------------------------------------------------------------------

using FlapiUnity;
using System;
using UnityEngine;

namespace FlapiUnity
{
	public static class FlapiFilters
	{
		/// <summary>
		/// Amplify signal by a gain value.
		/// </summary>
		/// <param name="data">Signal.</param>
		/// <param name="gain">Coefficient of amplification.</param>
		public static void Amplify (float[] data, float gain)
		{
			for (int i = 0; i < data.Length; i++)
				data [i] *= gain;
		}

		/// <summary>
		/// Taken from : https://github.com/aboisvert/high-pass-filter
		/// Cut the signal at a certain level.
		/// Useful when noise value is known and has to be cut.
		/// </summary>
		/// <param name="data">Signal.</param>
		/// <param name="beta">Amplitude cutoff.</param>
		public static void Cutoff (float[] data, float beta)
		{
			float[] input = new float[data.Length];
			data.CopyTo (input, 0);
			
			for (int i = 1; i < data.Length; i++) {
				// Original line was :
				// data [i] = (input [i] < 0.0f && input [i] < -beta && input [i] < input [i - 1]) ? input [i] : 0.0f;
				data [i] = (input [i] < 0.0f && Math.Abs (input [i]) < beta && input [i] < input [i - 1]) ? input [i] : 0.0f;
			}
		}
	
		/// <summary>
		/// Simple low pass filter taken from Wikipedia.
		/// Cut the high frequencies from a specified value.
		/// </summary>
		/// <param name="data">Signal</param>
		/// <param name="cutoff">Cutoff frequency.</param>
		/// <param name="sampleRate">Sample rate.</param>
		public static void LowPassFilter (float[] data, float cutoff, int sampleRate)
		{
			float RC = 1.0f / (cutoff * 2 * (float)Math.PI);
			float dt = 1.0f / (float)sampleRate;
			float alpha = RC / (RC + dt);
			
			float[] input = new float[data.Length];
			data.CopyTo (input, 0);
			
			for (int i = 1; i < data.Length; i++) {
				data [i] = data [i - 1] + (alpha * (input [i] - data [i - 1]));
			}
		}

		/// <summary>
		/// High pass filter taken from Wikipedia.
		/// Cut the low frequencies from a specified value.
		/// </summary>
		/// <param name="data">Signal.</param>
		/// <param name="cutoff">Cutoff frequency.</param>
		/// <param name="sampleRate">Sample rate.</param>
		public static void HighPassFilter (float[] data, float cutoff, int sampleRate)
		{
			float RC = 1.0f / (cutoff * 2 * (float)Math.PI);
			float dt = 1.0f / (float)sampleRate;
			float alpha = RC / (RC + dt);
			
			float[] input = new float[data.Length];
			data.CopyTo (input, 0);
			
			for (int i = 1; i < data.Length; i++) {
				data [i] = alpha * (data [i - 1] + input [i] - input [i - 1]);
			}
		}

		/// <summary>
		/// Taken from : https://github.com/aboisvert/high-pass-filter
		/// Better high pass filter implementation.
		/// Cut the low frequencies from a specified value.
		/// Alpha means higher frequency cutoff (more noise).
		/// </summary>
		/// <param name="data">Signal.</param>
		/// <param name="cutoff">Cutoff frequency.</param>
		/// <param name="alpha">Alpha coefficient.</param>
		/// <param name="sampleRate">Sample rate.</param>
		public static void HighPassFilterAlpha (float[] data, float cutoff, float alpha, int sampleRate)
		{
			float[] input = new float[data.Length];
			data.CopyTo (input, 0);

			data [0] = 0.0f;
			for (int i = 1; i < data.Length; i++) {
				data [i] = alpha * (data [i - 1] + input [i] - input [i - 1]);
			}
		}
	
		/// <summary>
		/// Normalize the signal.
		/// That means the signal will be amplified until saturation.
		/// </summary>
		/// <param name="data">Signal.</param>
		public static void Normalize (float[] data)
		{
			// Find the higher value (positive/negative sides).
			float min = Mathf.Abs (Mathf.Min (data));
			float max = Mathf.Max (data);
			float biggest = max > min ? max : min;
			
			float scale = 1.0f / biggest; // Amplification coefficient.
			for (int i = 0; i < data.Length; i++)
				data [i] = scale * data [i];
		}

		/// <summary>
		/// Removes signal offset.
		/// </summary>
		/// <param name="data">Signal.</param>
		public static void RemoveDC (float[] data)
		{
			int i;
			int length = data.Length;
			double mean = 0.0;
			
			// Compute offset
			for (i = 0; i < length; i++)
				mean += data [i];
			mean /= length;
			
			// Remove offset
			for (i = 0; i < length; i++) {
				data [i] = data [i] - (float)mean;
				if (data [i] > 1.0f)
					data [i] = 1.0f;
				if (data [i] < -1.0f)
					data [i] = -1.0f;
			}
		}
		
		/// <summary>
		/// Smoothen the signal.
		/// Each value is smoothed by its N neighbors (apply a mean).
		/// 
		/// Warning : pre amplify the signal and post reduce it may
		/// 	help to work on it. Do it if the signal seems silent
		/// 	after processing.
		/// </summary>
		/// <param name="data">Signal.</param>
		/// <param name="iterations">Iterations (neighbors).</param>
		public static void Smoothen (float[] data, int iterations)
		{
			float[] copy = new float[data.Length];
			data.CopyTo (copy, 0);
			
			// Beginning
			for (int i = 0; i < iterations; i++)
				data [i] = FlapiUtils.Mean (copy, i, i + iterations);
			
			// Middle
			for (int i = iterations; i < data.Length - iterations; i++)
				data [i] = FlapiUtils.Mean (copy, i - iterations, i + iterations);
			
			// End
			for (int i = data.Length -iterations; i < data.Length; i++)
				data [i] = FlapiUtils.Mean (copy, i - iterations, i);
		}
	}
}