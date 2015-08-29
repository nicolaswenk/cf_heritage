using FlapiUnity;
using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the Menu view.
/// </summary>
public class MenuController : MonoBehaviour
{
	/// <summary>The game state.</summary>
	private GameState state = GameState.LOGO;
	/// <summary>The logo animator.</summary>
	public Animator logoAnimator;
	/// <summary>The play animator.</summary>
	public Animator playAnimator;
	/// <summary>The intro animator.</summary>
	public Animator introAnimator;

	/// <summary>
	/// Called when the play button is cliqued. Show the correct view (intro, then the game).
	/// </summary>
	public void PlayClicked ()
	{
		if (state == GameState.LOGO)
			ShowIntro ();
		else if (state == GameState.INTRO)
			StartGame ();
	}

	/// <summary>
	/// Shows the intro text (doing all the animations).
	/// </summary>
	private void ShowIntro ()
	{
		state = GameState.INTRO;
		
		logoAnimator.SetBool ("visible", false);
		introAnimator.SetBool ("visible", true);
	}

	/// <summary>
	/// Load the game scene.
	/// </summary>
	private void StartGame ()
	{
		playAnimator.SetBool ("visible", false);		
		introAnimator.SetBool ("visible", false);

		Application.LoadLevel ("GreeceGenerated");
	}
	
}
