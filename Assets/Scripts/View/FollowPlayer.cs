using UnityEngine;
using System.Collections;

/// <summary>
/// Put this script on a GameObject to make it following the player.
/// This class allows you to choose a specific axis to follow the player on.
/// </summary>
public class FollowPlayer : MonoBehaviour {

	/// <summary>The player (or an other gameObject if you want) to follow.</summary>
	public GameObject player;
	/// <summary>
	/// The lvl manager (to know the game state, because it only follows
	/// when the state is "GAME").
	/// </summary>
	public LevelController lvlManager;
	/// <summary>If true, will follow on X axis (don't exclude to follow on X also).</summary>
	public bool followOnX;
	/// <summary>If true, will follow on Y axis (don't exclude to follow on X also).</summary>
	public bool followOnY;
	/// <summary>The initial difference between this gameObject and the player.</summary>
	private Vector3 diff;
	/// <summary>Uses <see cref="followOnX"/> and <see cref="followOnY"/> to build this "boolean" vector
	/// of axis multiplicator.</summary>
	private Vector3 dimensions;
	
	/// <summary>Build <see cref="dimentsions"/> after <see cref="followOnX"/> and <see cref="followOnY"/>.</summary>
	void Start () {
		dimensions = new Vector3 (followOnX?1:0, followOnY?1:0, 1);
	}

	/// <summary>
	/// Move this game object to <see cref="player"/> with a shift of <see cref="diff"/> on the
	/// different axis (<see cref="dimensions"/>).
	/// </summary>
	void Update () {
		if (lvlManager.GameState == GameState.GAME) {
			transform.position = new Vector3(player.transform.position.x*dimensions.x,
			                                 player.transform.position.y*dimensions.y,
			                                 player.transform.position.z*dimensions.z)+diff;
		} else {
			diff=transform.position-
				new Vector3(player.transform.position.x*dimensions.x,
				            player.transform.position.y*dimensions.y,
				            player.transform.position.z*dimensions.z);
		}
	}
}
