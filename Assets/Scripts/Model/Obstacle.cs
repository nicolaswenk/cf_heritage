using System;
using UnityEngine;

/// <summary>Describe an obstacle and its apparitions properties.</summary>
public class Obstacle : MonoBehaviour
{
	/// <summary>The type of the obstacle</summary>
	public ObstacleType obstacleType;
	/// <summary>The minimum height (in the scene) at which this obstacle can appear.</summary>
	public float minHeight=-10;
	/// <summary>The maximum height (in the scene) at which this obstacle can appear.</summary>
	public float maxHeight=0;	
	/// <summary>The rarity of this obstacle (1 is the standard value).</summary>
	public float rarity=1;
	
	public bool CanBePlaced(float height, ObstacleType type){
		if (this.obstacleType == ObstacleType.BOTH || type == this.obstacleType) {
			return height <= maxHeight && height >= minHeight;
		} else {
			return false;
		}
	}

	/// <summary>Gets the rarity of this obstacle (1 is the standard value).</summary>
	public float Rarity{
		get{ return rarity;}
	}
}

/// <summary>
/// The type of the obstacle.
/// </summary>
public enum ObstacleType{
	UP,
	DOWN,
	BOTH
}

