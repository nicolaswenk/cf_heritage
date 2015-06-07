using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the apparition and duration of the bonus phase and
/// ask to the <see cref="bonusViewer"/> to update after it.
/// </summary>
public class BigStarController : MonoBehaviour {

	/// <summary>The view component which shows the bonuses progress.</summary>
	public BigStarViewer bigStarViewer;
	
	private float bigStarProgress=0.0f;
	
	private const int NUMBER_TO_UNLOCK=5;

	private int starCounter=0;

	private int lastBigStarReached=0;

	public void CollectAStar(){
		starCounter++;
		float percentage = (starCounter - lastBigStarReached) / ((float)NUMBER_TO_UNLOCK);
		bigStarViewer.UpdateFilling (percentage);

		if (percentage>=1.0f) {
			bigStarViewer.ShowBigStar();
			lastBigStarReached=starCounter;
		}
	}	
	
	public void LooseAStar(){
		starCounter--;
		if (starCounter < lastBigStarReached) {
			starCounter=lastBigStarReached;
		}
		
		float percentage = (starCounter - lastBigStarReached) / ((float)NUMBER_TO_UNLOCK);
		bigStarViewer.UpdateFilling (percentage);
	}
}
