using UnityEngine;
using System.Collections;

/// <summary>
/// This class is call by the <see cref="BonusController"/> for showing any progress
/// of the bonus (like the progress to reach the bonus or when it will be over).
/// </summary>
public class BigStarViewer : MonoBehaviour {

	/// <summary>
	/// The bar used to show the time left to the next bonus phase.
	/// </summary>
	public Bar bigStarFillingBar;

	public GameObject PopUp;

	/// <summary>
	/// Call this methods to update the <see cref="bonusWaitingBar"/>
	/// </summary>
	/// <param name="percent">The percentage of the <see cref="bonusWaitingBar"/>.</param>
	public void UpdateFilling (float percent){
		bigStarFillingBar.Update (percent);
	}

	public void ShowBigStar (){
		bigStarFillingBar.Update (0.0f);
		PopUp.SetActive (true);
		PopUp.GetComponent<Animator> ().SetTrigger ("restart");
	}
}
