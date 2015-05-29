using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// All level controller should inherit this class to manage the game.
/// A hand made level could directly use this script.
/// </summary>
public class LevelController : MonoBehaviour
{
	/// <summary>The actual game's state.</summary>
	protected GameState state = GameState.INTRO;
	/// <summary>The player game object.</summary>
	public PlayerController player;
	/// <summary>The input controller</summary>
	protected InputController_I inputController;
	/// <summary>The exercice the patient should follow (in loop) and after which the level should be.</summary>
	protected DecreasingDrainageAutogene exercice;

	/// <summary>
	/// Create the exercice and the input controller.
	/// Launches the player enter animation.
	/// </summary>
	void Start ()
	{		
		exercice = new DecreasingDrainageAutogene (3,3,3,1.5f,3.0f,10.0f,0.5f);

		//ioController = new FlapiIOController (audio);
		inputController = new KeyboardInputController (exercice,3.0f);
		
		StartCoroutine (WaitForPlayer ());
	}
	
	/// <summary>
	/// Called once per frame. Check the player progress and move it (after the input controller).
	/// Can warn of a transition (not implemented yet).
	/// </summary>
	void Update ()
	{
		if (state == GameState.GAME) {
			inputController.Update();
			exercice.CheckProgress(inputController);
			player.Move(inputController, exercice);
			Transition transition=exercice.GetTransition();
			if(transition!=Transition.NONE){
				Debug.Log(transition);
			}
		}
	}
	
	/// <summary>
	/// Convert a breathing's volume (from 0 to 1) into the world's Y axis.
	/// </summary>
	/// <returns>The corresponding world's Y value.</returns>
	/// <param name="respirationValue">The breathing's volume value (from 0 to 1).</param>
	protected float breathingVolumeToWorldHeight(float respirationValue){
		float worldHeight = respirationValue * (player.maxHeight - player.minHeight);
		worldHeight += player.minHeight;
		return worldHeight;
	}
	
	/// <summary>
	/// Convert an exercice "position" vector into a position in the game space.
	/// </summary>
	/// <returns>The corresponding position in the game space.</returns>
	/// <param name="vector">The exercice "position".</param>
	protected Vector3 ExerciceToPlayer(Vector3 vector){
		vector.y = breathingVolumeToWorldHeight (vector.y);
		vector.x *= player.HorizontalSpeed;
		
		return vector;
	}

	/// <summary>
	/// Launch the player animation and "yield" when it's over.
	/// Can be launch as a coroutine.
	/// </summary>
	protected IEnumerator WaitForPlayer ()
	{
		Animator animator = (Animator)GetComponent ("Animator");
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Playing")) {
			yield return null;
		}
		animator.enabled = false;
		
		state = GameState.GAME;
		
		/*Flapi.threshold = 5.0f;
		Flapi.Start (audio, Flapi.GetMicrophone (0), 60);
		Debug.Log (Flapi.GetMicrophone (0).name);*/
	}
	
	/// <summary>Gets the actual game's state.</summary>
	public GameState GameState{
		get{ return state;}
	}
	
	/// <summary>Gets the exercice the patient should follow (in loop) and after which the level should be.</summary>
	public Exercice Exercice{
		get { return exercice;}
	}
	
}