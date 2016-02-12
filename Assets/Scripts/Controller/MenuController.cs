using FlapiUnity;
using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the Menu view.
/// </summary>
public class MenuController : MonoBehaviour
{
	private PlayerProfileData profileDataObj = PlayerProfileData.profileData;
	/// <summary>The game state.</summary>
	private GameState state = GameState.LOGO;
	/// <summary>The logo animator.</summary>
	public Animator logoAnimator;
	/// <summary>The play animator.</summary>
	public Animator playAnimator;
	/// <summary>The intro animator.</summary>
	public Animator introAnimator;
    /// <summary>The setup button animator.</summary>
    public Animator setupAnimator;
    /// <summary>The setup button animator.</summary>
    public Animator backAnimator;

    /// <summary>
    /// Called when the play button is cliqued. Show the correct view (intro, then the game).
    /// </summary>
    /// 
    public void Start()
    {
        PlayerProfileData.profileData.Load();
    }

    public void PlayClicked ()
	{
		if (state == GameState.LOGO)
			ShowIntro ();
		else if (state == GameState.INTRO)
			StartGame ();
	}

    public void SetupClicked()
    {
        Application.LoadLevel("SetupMenu");
    }

    public void BackClicked()
    {
        PlayerProfileData.profileData.Save();
        Application.LoadLevel("Menu");
    }

	// Add or remove one value in the parameters
	//nb Breathing High
	public void nbBreathingsHighDecClicked()
	{
		profileDataObj.nbBreathingsHigh -= 1;
	}

	public void nbBreathingsHighIncClicked()
	{
		profileDataObj.nbBreathingsHigh += 1;
	}

	//nbBreathingsMedium
	public void nbBreathingsMediumDecClicked()
	{
		profileDataObj.nbBreathingsMedium -= 1;
	}

	public void nbBreathingsMediumIncClicked()
	{
		profileDataObj.nbBreathingsMedium += 1;
	}

	//nbBreathingsLow
	public void nbBreathingsLowDecClicked()
	{
		profileDataObj.nbBreathingsLow -= 1;
	}

	public void nbBreathingsLowIncClicked()
	{
		profileDataObj.nbBreathingsLow += 1;
	}

	public void inspirationTimeDecClicked()
	{
		profileDataObj.inspirationTime -= .5f;
	}

	public void inspirationTimeIncClicked()
	{
		profileDataObj.inspirationTime += .5f;
	}

	public void holdingBreathTimeDecClicked()
	{
		profileDataObj.holdingBreathTime -= .5f;
	}

	public void holdingBreathTimeTimeIncClicked()
	{
		profileDataObj.holdingBreathTime += .5f;
	}

	public void expirationMinTimeTimeDecClicked()
	{
		profileDataObj.expirationMinTime -= .5f;
	}

	public void expirationMinTimeTimeIncClicked()
	{
		profileDataObj.expirationMinTime += .5f;
	}
		

    /// <summary>
    /// Shows the intro text (doing all the animations).
    /// </summary>
    /// 

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
        setupAnimator.SetBool ("visible", false);

        Application.LoadLevel ("GreeceGenerated");
	}
	
}
