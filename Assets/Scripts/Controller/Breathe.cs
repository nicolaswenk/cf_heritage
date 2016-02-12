using UnityEngine;
using System.Collections;

public class Breathe : MonoBehaviour {

	LevelController levelController;
	Animator animator;


	// Use this for initialization
	void Start () {
		levelController = GameObject.FindObjectOfType<LevelController> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		InputController_I inputController = levelController.InputController;

		//check if we are in the game
		if (levelController.GameState == GameState.GAME) {

			//if we are in breathing state set the animation to breathe_in if not set to breathe_out
			if (inputController.GetInputState () == BreathingState.INSPIRATION) {
				animator.SetBool ("Breathe_in", true);
			} else {
				animator.SetBool ("Breathe_in", false);
			}
		}
	}
}
