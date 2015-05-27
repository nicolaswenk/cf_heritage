using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the apparition and duration of the bonus phase and
/// ask to the <see cref="bonusViewer"/> to update after it.
/// </summary>
public class BonusPhaseController : MonoBehaviour {

	/// <summary>The view component which shows the bonuses progress.</summary>
	public BonusViewer bonusViewer;
	/// <summary>The time passed since the last event (bonus started or ended).</summary>
	private float time=0.0f;
	/// <summary>The time (in seconds) between each bonus phases.</summary>
	private float bonusApparitionPeriod=10.0f;//5.0f*60.0f;// 5 minutes
	/// <summary>The duration in seconds of a bonus phase.</summary>
	private float bonusDuration=10.0f;//5.0f*60.0f;// 5 minutes
	/// <summary>Tells if the bonus is actually active or not.</summary>
	private bool isBonusActive=false;
	/// <summary>The gameObject which contains all the stars
	/// (to access them and put them in "bonus" looking).</summary>
	public GameObject starsContainer;

	/// <summary>
	/// Check after the time if the bonus is reached or over and ask
	/// to the viewer to update themselves.
	/// </summary>
	void Update () {
		time += Time.deltaTime;
		if (!isBonusActive) {
			if (time > bonusApparitionPeriod) {
				time = 0.0f;
				isBonusActive = true;
				LaunchBonus();
			} else {
				float percent = time / bonusApparitionPeriod;
				bonusViewer.UpdateWaiting (percent);
			}
		}
		else{
			if(time > bonusDuration){
				time=0.0f;
				isBonusActive=false;
				FinishBonus();
			}else{
				float percent = time / bonusApparitionPeriod;
				bonusViewer.UpdateLasting (percent);
			}
		}
	}

	/// <summary>
	/// Put all stars in the bonus state (launch an animation).
	/// </summary>
	private void LaunchBonus(){
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("StarContainer")) {
			StarController star=gameObject.GetComponentInChildren<StarController>();
			star.animator.SetTrigger("bonusStart");
		}
	}
	
	
	/// <summary>
	/// Put all stars in the normal state (launch an animation).
	/// </summary>
	private void FinishBonus(){
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("StarContainer")) {
			StarController star=gameObject.GetComponentInChildren<StarController>();
			star.animator.SetTrigger("bonusEnd");
		}		
	}

	/// <summary>
	/// Returns the star multiplicator. (1 if the bonus is off and 3 otherwise).
	/// </summary>
	public int GetMultiplicator(){
		if(isBonusActive){
			return 3;
		}
		else{
			return 1;
		}
	}
	
	/// <summary>Tells if the bonus is actually active or not.</summary>
	public bool IsBonusActive{
		get{
			return isBonusActive;
		}
	}
}
