using System;
using UnityEngine;

/// <summary>
/// This script can be attached to any gameObject we want to delete when it leaves the screen
/// on the left.
/// </summary>
public class DetroyOnLeftScreenBorderController:MonoBehaviour
{
	/// <summary>
	/// Called at each frame. Checks if we leave the left border of the screen
	/// and if we did, destroy the gameObject this script is attached to.
	/// </summary>
	void Update(){
		//We use -0.5f instead of 0.0f to be sure that it leaves the screen
		if (Camera.main.WorldToViewportPoint (transform.position).x < -0.5f) {
			Destroy (gameObject);
		}
	}
}

