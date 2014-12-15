/*
 * TextureScroller2D
 * 
 * Author : Cyriaque Skrapits
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Utility class allowing to scroll a texture in a specified offset and speed.
/// </summary>
public class TextureScroller2D : MonoBehaviour
{
	public float scrollSpeedX;
	public float scrollSpeedY;
	public float tileSizeX;
	public float tileSizeY;

	private Vector3 startPosition;

	void Start ()
	{
		startPosition = transform.position;
	}
	
	void Update ()
	{
		float newPositionX = Mathf.Repeat (Time.time * scrollSpeedX, tileSizeX);
		float newPositionY = Mathf.Repeat (Time.time * scrollSpeedY, tileSizeY);
		transform.position = startPosition + Vector3.left * newPositionX + Vector3.down * newPositionY;
	}
}