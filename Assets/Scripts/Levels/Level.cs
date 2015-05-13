using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
	protected GameState state = GameState.INTRO;
	
	//public Animator playerAnimator;
	
	public Player player;
	
	protected float acceleration = 0.0f;
	
	protected IOController_I ioController;
	
	protected DrainageAutogene exercice;
	
	// Use this for initialization
	void Start ()
	{		
		//TODO: Build the ParameterManager object by reading the properties instead of setting with those magic values
		exercice = new DrainageAutogene ();

		//ioController = new FlapiIOController (audio);
		ioController = new KeyboardIOController (new ParameterManager(10,1), exercice);
		
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
			string warning=exercice.GetWarning();
			if(warning!=null){
				Debug.Log(warning);
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
