using UnityEngine;
using System.Collections;

/// <summary>
/// This class shows the progress of the bonuses we can have by doing correct breathings.
/// It also instanciate the bonus stars when asked (by the BonusStarsController).
/// </summary>
public class BonusStarsViewer : MonoBehaviour {

	/// <summary>
	/// True is the bonus progress is full.
	/// </summary>
	private bool isFull=false;
	/// <summary>
	/// The model of a bonus star to instanciate when bonus reached.
	/// </summary>
	public GameObject bonusStarModel;

	/// <summary>
	/// Shows the progress of the holding breath bonus.
	/// </summary>
	/// <param name="percentage">Percentage reached of this bonus.</param>
	public void ShowHoldingBreathProgress(float percentage){
		if(!isFull){
			this.gameObject.SetActive (true);
			this.transform.localScale = new Vector3 (percentage, percentage, percentage);
			SpriteRenderer renderer=this.GetComponent<SpriteRenderer> ();
			renderer.color= new Color(renderer.color.r, renderer.color.g, renderer.color.b, percentage);

			if (percentage >= 1.0f) {
				isFull=true;
				Animator animator=this.GetComponent<Animator>();
				animator.SetTrigger("Full");
			}
		}
	}

	/// <summary>
	/// Releases a bonus star.
	/// Instantiate it at the correct location and set its reference on the player.
	/// </summary>
	/// <param name="player">The player (controller) reference to give to the new created bonusStar.</param>
	public void ReleaseStar(PlayerController player){
		GameObject gameObject=(GameObject)Instantiate (bonusStarModel, this.transform.position, Quaternion.identity);
		BonusStarController bonusStar=gameObject.GetComponent<BonusStarController> ();
		bonusStar.SetPlayer(player);
	}

	/// <summary>
	/// Hides the holding breath progress (if the bonus is failed or reached).
	/// </summary>
	public void HideHoldingBreathProgress(){
		isFull = false;
		Debug.Log ("hide");
		this.gameObject.SetActive (false);
	}
}
