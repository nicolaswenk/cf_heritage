using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Greece game;
	public float minHeight;
	public float maxHeight;
	
	private float speedHorizontal=1.0f;
	private float speedExpiration=-2.0f;
	private float speedInspiration=4.0f;

	public GameObject inflatablePart=null;

	public ParticleSystem bubbles;

	public Animator SwimAnimation;

	private float volume=1.0f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inflatablePart != null) 
		{
			inflatablePart.transform.localScale = new Vector3 (1, volume + 0.5f, 1);	
		}
	}

	public void Move(IOController_I ioController, DrainageAutogene exercice){	

		volume = exercice.Volume;
	
		float movement = 0.0f;
		float horizontalMovement = 0.0f;

		if (ioController.IsMoving ()) {
			horizontalMovement = speedHorizontal * Time.deltaTime;
			SwimAnimation.speed = 1.0f;
		} else {
			SwimAnimation.speed = 0.0f;
		}

		switch (ioController.GetInputState()) {
		case InputState.EXPIRATION:
		case InputState.STRONG_EXPIRATION:
			bubbles.emissionRate=10.0f*ioController.GetStrength();
			break;
		default:
			bubbles.emissionRate=0.0f;
			break;
		}

		float newY = (maxHeight - minHeight) * exercice.Volume + minHeight;
		transform.Translate (new Vector3 (horizontalMovement, newY-transform.position.y));
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		GetComponent<Rigidbody2D>().gravityScale = 1;
		game.GameOver ();
	}
}
