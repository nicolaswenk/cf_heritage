using FlapiUnity;
using UnityEngine;
using System.Collections;

public class Greece : MonoBehaviour
{
	private readonly static int STATE_LOGO = 0;
	private readonly static int STATE_INTRO = 1;
	private readonly static int STATE_GAME = 2;

	private int state = STATE_LOGO;

	public Animator logoAnimator;
	public Animator playAnimator;
	public Animator introAnimator;
	public Animator playerAnimator;
	public Animator gameOverAnimator;

	public GameObject player;

	float acceleration = 0.0f;
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (state == STATE_GAME) {
			Flapi.Analyze (audio);

			//Debug.Log (Flapi.blowing + " - " + Flapi.frequency);

			if (player.transform.position.y > -5)
				player.transform.Translate (new Vector3 (0, -0.02f));

			if (Flapi.blowing && acceleration < 1.0f)
				acceleration += 0.02f;
			if (!Flapi.blowing && acceleration > 0.0f)
				acceleration -= 0.02f;


			float move = 0.04f * (acceleration);
			if (player.transform.position.y < 5 && move > 0.0f)
				player.transform.Translate (new Vector3 (0, move));
		}
	}

	public void GameOver ()
	{
		gameOverAnimator.Play ("GameOver");
	}

	public void PlayClicked ()
	{
		if (state == STATE_LOGO)
			ShowIntro ();
		else if (state == STATE_INTRO)
			StartGame ();
	}

	private void ShowIntro ()
	{
		state = STATE_INTRO;

		logoAnimator.SetBool ("visible", false);
		introAnimator.SetBool ("visible", true);
	}

	private void StartGame ()
	{
		state = STATE_GAME;

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

		Flapi.threshold = 5.0f;
		Flapi.Start (audio, Flapi.GetMicrophone (0), 60);
		Debug.Log (Flapi.GetMicrophone (0).name);
	}
	
}
