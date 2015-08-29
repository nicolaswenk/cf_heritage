using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage a bonus star which appears (thanks to a correct breathing).
/// This stars goes away from the player (on x) in a first time and it's speed is decreasing.
/// Then it comes closer to the player (on x and y) with a increasing speed (until the star
/// is catched by the player).
/// </summary>
public class BonusStarController : MonoBehaviour {

	/// <summary>
	/// The initial speed of the star.
	/// It will decrease of <see cref="decelaration"/> each seconds.
	/// When speed is positive, the star goes away of the player (only x coordinate changes).
	/// When speed is negative, the star come closer to the player (x and y coorinates chage).
	/// </summary>
	private float speed=0.5f;
	/// <summary>
	/// The deceleration of the speed. (speed decrease of this value in one second).
	/// </summary>
	private float deceleration=1.0f;
	/// <summary>
	/// A reference tho the starController of the star object conained in this bonus star gameobject.
	/// </summary>
	public StarController starController;
	/// <summary>
	/// A reference to the playerController to know its position.
	/// </summary>
	private PlayerController playerController;
	/// <summary>
	/// A reference to the bonusPhaseController to know if our star should be the one used in the bonus phase
	/// or the usual ones.
	/// </summary>
	private BigStarController bonusPhaseController;
	/// <summary>
	/// Is true when the 'x' of this object is lower than the player one.
	/// Used to stop the star movement.
	/// </summary>
	private bool destroying=false;
	/// <summary>
	/// Is true while the star goes away from the player (speed > 0).
	/// </summary>
	private bool wasGoingAway=true;

	/// <summary>
	/// Retrieve some controller. Disable the star collider (will be enable later when <see cref="wasGoingAway"/> is false.
	/// Enable the bonus phase looking of our star if we are in bonus phase.
	/// </summary>
	void Start(){
		starController.GetComponent<Collider2D> ().enabled = false;
	}

	/// <summary>
	/// Called once per frame. Move the star and changes its speed.
	/// </summary>
	void Update () {
		if(!destroying){
			speed -= deceleration * Time.deltaTime;
			if (speed < 0) {
				if(wasGoingAway){
					starController.GetComponent<Collider2D> ().enabled = true;
				}
				wasGoingAway=false;
				if (speed < -0.5f) {
					speed=-0.5f;
				}
				
				transform.position += (transform.position-playerController.transform.position).normalized * speed;
				if(transform.position.x<playerController.transform.position.x){
					destroying=true;
					transform.position=playerController.transform.position;
				}
			}
			else{
				transform.position += new Vector3 (speed, 0.0f, 0.0f);
			}
		}
	}

	/// <summary>
	/// Sets the player. The star will then move after the player location.
	/// </summary>
	/// <param name="playerController">The player controller reference.</param>
	public void SetPlayer(PlayerController playerController){
		this.playerController = playerController;
	}
}
