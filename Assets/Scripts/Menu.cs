using FlapiUnity;
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	private GameState state = GameState.LOGO;
	
	public Animator logoAnimator;
	public Animator playAnimator;
	public Animator introAnimator;

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
		Debug.Log ("intro");
	}
	
	private void StartGame ()
	{
		playAnimator.SetBool ("visible", false);		
		introAnimator.SetBool ("visible", false);

		Application.LoadLevel ("Greece");
	}
	
}
