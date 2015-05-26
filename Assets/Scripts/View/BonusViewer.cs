using UnityEngine;
using System.Collections;

/// <summary>
/// This class is call by the <see cref="BonusController"/> for showing any progress
/// of the bonus (like the progress to reach the bonus or when it will be over).
/// </summary>
public class BonusViewer : MonoBehaviour {

	/// <summary>
	/// The bar used to show the time left to the next bonus phase.
	/// </summary>
	public Bar bonusWaitingBar;
	/// <summary>
	/// The bar used to show the time left to the end of the current bonus phase.
	/// </summary>
	public Bar bonusLastingBar;

	/// <summary>
	/// Call this methods to update the <see cref="bonusWaitingBar"/>
	/// </summary>
	/// <param name="percent">The percentage of the <see cref="bonusWaitingBar"/>.</param>
	public void UpdateWaiting (float percent){
		bonusWaitingBar.Update (percent);
	}
	
	/// <summary>
	/// Call this methods to update the <see cref="bonusLastingBar"/>
	/// </summary>
	/// <param name="percent">The percentage of the <see cref="bonusLastingBar"/>.</param>	
	public void UpdateLasting (float percent){
		bonusLastingBar.Update (percent);		
	}
}
