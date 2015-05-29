using UnityEngine;
using System.Collections;

/// <summary>
/// The controller that manages the bonus stars progess and apparition.
/// </summary>
public class BonusStarsController : MonoBehaviour {

	/// <summary>
	/// A reference to the level controller to get the exercice.
	/// </summary>
	public LevelController levelController;
	/// <summary>
	/// A reference to the bonusStarsViewer which will show the progress
	/// and the bonus stars in the scene.
	/// </summary>
	public BonusStarsViewer viewer;
	/// <summary>
	/// The percentage reached of the holding breath volume. If the player
	/// expire before it reaches 1.0f (100%) no star will be created.
	/// </summary>
	private float holdingBreathPercentage=0.0f;

	/// <summary>
	/// Called once per frame.
	/// Updates the progress percentages, lauches the bonuses when needed and update the viewer.
	/// </summary>
	void Update () {
		switch (levelController.Exercice.State) {

		case BreathingState.HOLDING_BREATH:
			holdingBreathPercentage=levelController.Exercice.ActualBreathing.HoldingBreathPercentage;
			viewer.ShowHoldingBreathProgress(holdingBreathPercentage);
			break;

		case BreathingState.EXPIRATION:
			if(holdingBreathPercentage>=1.0f){
				viewer.ReleaseStar(levelController.player);
				holdingBreathPercentage=0.0f;
			}
			else {
				viewer.HideHoldingBreathProgress();
			}
			break;
		}
	}
}
