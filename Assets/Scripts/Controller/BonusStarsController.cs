using UnityEngine;
using System.Collections;

public class BonusStarsController : MonoBehaviour {

	public LevelController levelController;

	public BonusStarsViewer viewer;
	private float holdingBreathPercentage=0.0f;
	
	// Update is called once per frame
	void Update () {
		if(levelController.Exercice.State!=null){
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
}
