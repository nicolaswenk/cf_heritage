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
	/// </summary>
	float GetStrength();

	/// <summary>
	/// Gets the InputState (expiration, inspiration, holding breath, ...).
	/// </summary>
	InputState GetInputState();

	/// <summary>
	/// Determines whether the patient is holding the moving input or not.
	/// </summary>
	bool IsMoving();
}

