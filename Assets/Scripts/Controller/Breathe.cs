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
	
		if (levelController.InputController.GetInputState() == BreathingState.INSPIRATION) {
			animator.SetBool ("Breathe_in", true);
		}
		else {
			animator.SetBool ("Breathe_in", false);
		}
	}
}
