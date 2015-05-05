using FlapiUnity;
using UnityEngine;
using System.Collections;

public class Greece : MonoBehaviour
{
	private GameState state = GameState.INTRO;

	//public Animator playerAnimator;

	public Player player;

	float acceleration = 0.0f;

	IOController_I ioController;

	private DrainageAutogene exercice;

	// Use this for initialization
	void Start ()
	{
		//ioController = new FlapiIOController (audio);
	 
		//TODO: Build the ParameterManager object by reading the properties instead of setting with those magic values
		exercice = new DrainageAutogene ();
		ioController = new KeyboardIOController (new ParameterManager(10,1), exercice);

		//playerAnimator.SetTrigger ("entering");

		StartCoroutine (WaitForPlayer ());
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

	private IEnumerator WaitForPlayer ()
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
