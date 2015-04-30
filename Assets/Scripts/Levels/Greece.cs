using FlapiUnity;
using UnityEngine;
using System.Collections;

public class Greece : MonoBehaviour
{
	private GameState state = GameState.LOGO;

	public Animator logoAnimator;
	public Animator playAnimator;
	public Animator introAnimator;
	public Animator playerAnimator;
	public Animator gameOverAnimator;

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
		ioController = new KeyboardIOController (new ParameterManager(2,1), exercice);
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

	public void GameOver ()
	{
		gameOverAnimator.Play ("GameOver");
	}

	public void PlayClicked ()
	{
		if (state == GameState.LOGO)
			ShowIntro ();
		else if (state == GameState.INTRO)
			StartGame ();
	}

	private void ShowIntro ()
	{
		state = GameState.INTRO;

		logoAnimator.SetBool ("visible", false);
		introAnimator.SetBool ("visible", true);
	}

	private void StartGame ()
	{

		playAnimator.SetBool ("visible", false);		
		introAnimator.SetBool ("visible", false);
		playerAnimator.SetTrigger ("entering");

		StartCoroutine (WaitForPlayer ());
	}

	private IEnumerator WaitForPlayer ()
	{
		Animator animator = (Animator)player.GetComponent ("Animator");
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
