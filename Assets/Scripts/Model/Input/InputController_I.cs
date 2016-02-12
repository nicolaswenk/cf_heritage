using System;

/// <summary>
/// Implements this interface to manage a new input device (or a simulator like a keyboard for testing purposes).
/// </summary>
public interface InputController_I
{
	/// <summary>
	/// Read the input and update the values to return.
	/// </summary>
	void Update();

	/// <summary>
	/// Gets the strength of expiration or inspiration (depends on the InputSate).
	/// 0.0f, it means that the patient is not blowing or inspiring.
	/// 1.0f, is that the patient is blowing at 105ml/sec so empty its lungs in 20 seconds.
	/// </summary>
	float GetStrength();

	/// <summary>
	/// Gets the BreathingState (expiration, inspiration, holding breath, ...).
	/// </summary>
	BreathingState GetInputState();

    /// <summary>
    /// SupposedPatientMaxVolume / volumeMaxReached
    /// 1.0f is default value
    /// </summary>
    /// <param name="calibrationFactor"></param>
    void SetCalibrationFactor(float calibrationFactor);

    /// <summary>
    /// SupposedPatientMaxVolume / volumeMaxReached
    /// 1.0f is default value
    /// </summary>
    float GetCalibrationFactor();

    /// <summary>
    /// Determines whether the patient is holding the moving input or not.
    /// </summary>
    bool IsMoving();
}

