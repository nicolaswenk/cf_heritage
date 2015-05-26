using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	
	public GameObject player;
	public LevelController lvlManager;
	
	public bool followOnX;
	public bool followOnY;

	private Vector3 diff;
	private Vector3 dimensions;

	// Use this for initialization
	void Start () {
		dimensions = new Vector3 (followOnX?1:0, followOnY?1:0, 1);
	}
	
	// Update is called once per frame
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
