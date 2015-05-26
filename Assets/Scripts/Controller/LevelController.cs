using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
	protected GameState state = GameState.INTRO;
	
	//public Animator playerAnimator;
	
	public PlayerController player;
	
	protected float acceleration = 0.0f;
	
	protected InputController_I ioController;
	
	protected DecreasingDrainageAutogene exercice;
	
	// Use this for initialization
	void Start ()
	{		
		//TODO: Build the ParameterManager object by reading the properties instead of setting with those magic values
		exercice = new DecreasingDrainageAutogene (3,3,3,1.5f,3.0f,10.0f,0.5f);

		//ioController = new FlapiIOController (audio);
		ioController = new KeyboardInputController (exercice,3.0f);
		
		StartCoroutine (WaitForPlayer ());
	}
	
	private Vector3 exerciceToPlayer(Vector3 vector){
		vector.y *= player.maxHeight - player.minHeight;
		vector.y += player.minHeight;
		vector.x *= player.HorizontalSpeed;
		
		return vector;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (state == GameState.GAME) {
			ioController.Update();
			exercice.CheckProgress(ioController);
			player.Move(ioController, exercice);
			Transition transition=exercice.GetTransition();
			if(transition!=Transition.NONE){
				Debug.Log(transition);
			}
		}
	}
	
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
	
	public GameState GameState{
		get{ return state;}
	}
	
}
