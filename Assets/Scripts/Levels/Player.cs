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

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Move(IOController_I ioController, DrainageAutogene exercice){	
	
		float movement = 0.0f;
		float horizontalMovement = 0.0f;

		if (ioController.IsMoving()) {
			horizontalMovement=speedHorizontal*Time.deltaTime;
		}

		float newY = (maxHeight - minHeight) * exercice.Volume + minHeight;
		transform.Translate (new Vector3 (horizontalMovement, newY-transform.position.y));
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		rigidbody2D.gravityScale = 1;
		game.GameOver ();
	}
}
