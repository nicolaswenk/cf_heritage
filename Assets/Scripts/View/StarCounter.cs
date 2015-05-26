using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This class represents the UI component which shows the player's
/// actual number of stars.
/// </summary>
public class StarCounter : MonoBehaviour {

	/// <summary>
	/// The amount of star the player has collected.
	/// </summary>
	private int starCollected=0;
	/// <summary>
	/// The textual UI component which shows the amount of star collected.
	/// </summary>
	public Text label;

	/// <summary>
	/// Called when a star is collected. Increase <see cref="starCollected"/>  of the given number
	/// and update the label.
	/// </summary>
	/// <param name="numberCollected">Number of star collected (usually 1, but can be more if we're in bonus phase).</param>
	public void Collect(int numberCollected){
		starCollected += numberCollected;
		UpdateLabel ();
	}

	/// <summary>
	/// Called when the player hits an obstacle. Decrease <see cref="starCollected"/>  of the given number
	/// and update the label.
	/// <see cref="starCollected"/> can't be negative (stay at 0 if we keep loosing stars).
	/// </summary>
	/// <param name="numberLost">Number of stars lost (actually, always 1).</param>
	public void Loose(int numberLost){
		starCollected -= numberLost;
		if (starCollected < 0) {
			starCollected = 0;
		}
		UpdateLabel ();
	}

	/// <summary>
	/// Updates the label to show the value of <see cref="starCollected"/>.
	/// </summary>
	private void UpdateLabel (){
		label.text = starCollected.ToString();
	}
}
