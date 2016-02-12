using UnityEngine;
using System.Collections;

/// <summary>
/// The player controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
	/// <summary>A reference to the game controller.</summary>
	public LevelController game;
	/// <summary>The minimum height in the game the player can reach (when his lungs are empty).</summary>
	public float minHeight;
	/// <summary>The maximum height in the game the player can reach (when his lungs are full).</summary>
	public float maxHeight;
	/// <summary>Horizontal speed of the player (world unit/second)</summary>
	private float speedHorizontal=1.0f;
	/// <summary>A gameObject which will inflate as the player lungs. (using transform.scale from 0.5 to 1.5)</summary>
	public GameObject inflatablePart=null;
	/// <summary>The bubbles particles system.</summary>
	public ParticleSystem bubbles;
	/// <summary>The swimming animator</summary>
	public Animator SwimAnimator;
	/// <summary>The star counter (to increment when a star is catched and decreased when an obstacle touched).</summary>
	public BigStarController bigStarController;
	/// <summary>The bonus controller (to know how many star we collect).</summary>
	public BigStarController bonusController;
	/// <summary>The volume updated at each <see cref="Move"/> call after the one the exercice computes.</summary>
	private float volume=1.0f;
	/// <summary>The player's breathing stat at the last update.</summary>
	private BreathingState lastState;
	/// <summary>The breathing switch audio component. To change the volume after the breathing strength.</summary>
	public Fabric.SwitchComponent breathingSwitch;

	/// <summary>
	/// Called once per frame. Apply the scaling to the <see cref="inflatablePart"/> and <see cref=""/> 
	/// </summary>
	void Update ()
    {
        if (inflatablePart != null) 
		{
			Transform[] childrens=inflatablePart.GetComponentsInChildren<Transform>();

			inflatablePart.transform.localScale = new Vector3 (1, volume + 0.5f, 1);
			for(int i=0; i<childrens.Length; i++){
				if(childrens[i]!=inflatablePart.transform && childrens[i].gameObject.name!="Legs"){
					childrens[i].localScale=new Vector3(1, 1/(volume+0.5f), 1);
				}
			}
		}
	}

	/// <summary>
	/// Move the player after the inputController state and volume (read from the exercice).
	/// </summary>
	/// <param name="inputController">The input controller.</param>
	/// <param name="exercice">Used to get the actual pulmonary volume.</param>
	public void Move(InputController_I inputController, DecreasingAutogenicDrainage exercice){

        volume = exercice.Volume;
	
		float horizontalMovement = 0.0f;

		if (inputController.IsMoving ()) {
			horizontalMovement = speedHorizontal * Time.deltaTime;
			SwimAnimator.speed = 1.0f;
		} else {
			SwimAnimator.speed = 0.0f;
		}

		BreathingState state = inputController.GetInputState ();
        //em.type = ParticleSystemEmissionType.Time;
        if (state != lastState)
        {
            switch (state)
            {
                case BreathingState.EXPIRATION:
                    Fabric.EventManager.Instance.PostEvent("BreathingSwitch", Fabric.EventAction.SetSwitch, "Expiring");
                    Fabric.EventManager.Instance.PostEvent("BreathingSwitch", Fabric.EventAction.PlaySound);
                    break;
                case BreathingState.INSPIRATION:
                    bubbles.emissionRate = 0.0f;
                    Fabric.EventManager.Instance.PostEvent("BreathingSwitch", Fabric.EventAction.SetSwitch, "Inspiring");
                    break;
                default:
                    bubbles.emissionRate = 0.0f;
                    Fabric.EventManager.Instance.PostEvent("BreathingSwitch", Fabric.EventAction.PauseSound);
                    break;
            }
        }

        if (state == BreathingState.EXPIRATION)
        {
            bubbles.emissionRate = 10.0f * inputController.GetStrength();
        }

		//TODO Make this value more precise and clean. What a strength of 1.0f mean ? What's the max strength ?
		//Actually, the keyboardInputController return 1.0f for a normal strength and 3.0f for a big one.
		breathingSwitch.SetVolume (inputController.GetStrength()/3.0f);

		float newY = (maxHeight - minHeight) * exercice.Volume + minHeight;
		transform.Translate (new Vector3 (horizontalMovement, newY-transform.position.y));
		lastState=inputController.GetInputState();
	}

	/// <summary>
	/// Call the <see cref="Collide"/> method
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	void OnTriggerEnter2D(Collider2D other){
		Collide (other);
	}

	/// <summary>
	/// Implement the collision business.
	/// If the other is an obstacle we loose a star.
	/// If the other is a start we collect the star (the number depends on <see cref="bonusController"/>).
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	public void Collide(Collider2D other){
		switch (other.tag) {
		case "Obstacle":
			bigStarController.LooseAStar();
			GetComponent<Animator>().SetTrigger("collision");
			other.enabled=false;
			Fabric.EventManager.Instance.PostEvent("PlayerHurt");
			break;
		case "Star":
			bigStarController.CollectAStar();
			break;
		}
	}
	
	/// <summary>Gets the horizontal speed of the player (world unit/second)</summary>
	public float HorizontalSpeed{
		get{ 
			return speedHorizontal;
		}
	}
}
