using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;

/// <summary>
/// This class simulated a Flutter (for testing purposes). If we press the space key, it's like we are blowing as awaited and
/// if we press Ctrl+space it's like we are blowing strongly. 
/// </summary>
public class PepInputController2
{
	/*SerialPort stream = new SerialPort("\\\\.\\COM25", 9600);			// open serial Mac = /dev/tty.usbmodem621, PC = COM3 or COM4 ; baud = 9600

	private float pepValue = 0f;
	private bool  pepBlowing = true;
	/// <summary>
	/// The expiration speed of a strong expiration.
	/// </summary>
	private float strongBreathValue;


	private float f_pressure_inter;
	private int x = 0;
	
	void Start () {
		stream.Open();
		stream.ReadTimeout = 1;													// delay if no data received
		InvokeRepeating("streamIn", 0.01f, 0.01f);								// frees memory by reading serial port each 0.5 sec.
	}

	void Update () {
	}
	
	void streamIn () {
		if (stream.IsOpen)
		{
			try
			{
				string value = stream.ReadLine();								// reads serial port
				pepValue = float.Parse(value);
				x++;
			}
			catch(System.Exception)												// exit the reading if no value to avoid infinite run
			{	
			}
		}		
	}


	/// <summary>
	/// Initializes a new instance of the <see cref="KeyboardIOController"/> class.
	/// </summary>
	/// <param name="exercice">Exercice.</param>
	/// <param name="strongBreathValue">The expiration speed of a strong breath.</param>
	public PepInputController2 (DecreasingAutogenicDrainage exercice, float strongBreathValue):base(exercice)
	{
		this.strongBreathValue = strongBreathValue;
	}

	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	public override void Update(){
		this.pepValue = Input.GetAxisRaw ("Breath");

		if (this.pepValue < 0.1f)
			pepBlowing = true;
	}

	/// <summary>
	/// Returns whether the patient is blowing or not (if the space bar is down or not).
	/// </summary>
	/// <returns><c>true</c>, if the patient is blowing, <c>false</c> otherwise.</returns>
	protected override bool isBlowing(){
		return this.pepBlowing;
	}

	/// <summary>
	/// Gets the strength of expiration (1.0f if the space key is down, <see cref="strongBreathValue"/> if Ctrl+space is down and 0.0f otherwise).
	/// 0.0f, it means that the patient is not blowing.
	/// </summary>
	public override float GetExpirationStrength(){
		return this.pepValue;
	}

	/// <summary>
	/// Determines whether the patient is holding the moving input or not (if the right arrow of the keyboard is down or not).
	/// </summary>
	public override bool IsMoving(){
		return Input.GetKey (KeyCode.RightArrow);
	}*/
}

