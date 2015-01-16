using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Greece game;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		rigidbody2D.gravityScale = 1;
		game.GameOver ();
	}
}
