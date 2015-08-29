//------------------------------------------------------------------------------
//
//	FlapiUnity::FlapiUtils
//
//	Author : Cyriaque Skrapits <cyriaque.skrapits[at]he-arc.ch>
//	Version : 1.0
//
//	This class provides useful functions for the FlapiUnity library.
// 
//------------------------------------------------------------------------------

using System;

namespace FlapiUnity
{
	public class FlapiUtils
	{
		/// <summary>
		/// Compute the mean in a specified range from an array.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="from">Range beginning</param>
		/// <param name="to">Range ending</param>
		public static float Mean (float[] data, int from, int to)
		{
			float mean = 0.0f;
			for (int i = from; i <= to; i++)
				mean += data [i];
				
			return mean / (float)(to - from);
		}

		/// <summary>
		/// Compute the absolute mean in a specified range from an array.
		/// </summary>
		/// <returns>The mean.</returns>
		/// <param name="data">Data.</param>
		/// <param name="from">Range beginning</param>
		/// <param name="to">Range ending</param>
		public static float AbsMean (float[] data, int from, int to)
		{
			float mean = 0.0f;
			for (int i = from; i <= to; i++)
				mean += Math.Abs (data [i]);
				
			return mean / (float)(to - from);
		}
	}
}

