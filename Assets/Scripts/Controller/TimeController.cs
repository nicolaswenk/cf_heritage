using UnityEngine;
using System.Diagnostics;
using System.Collections;

public class TimeController : MonoBehaviour {

	public LevelController levelController;

	private const long TIME_GOAL=60*1000;//1 minute

	private Stopwatch stopWatch=new Stopwatch();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (levelController.GameState == GameState.GAME) {
			if(!stopWatch.IsRunning){
				stopWatch.Start();
			}
		}
	}

	public float GetTimeNumberGoalReached(){
		return stopWatch.ElapsedMilliseconds/(float)TIME_GOAL;
	}
}
