using UnityEngine;
using System.Collections;

public class BonusViewer : MonoBehaviour {
	
	public Bar bonusWaitingBar;
	public Bar bonusLastingBar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateWaiting (float percent){
		bonusWaitingBar.Update (percent);
	}
	
	public void UpdateLasting (float percent){
		bonusLastingBar.Update (percent);		
	}
}
