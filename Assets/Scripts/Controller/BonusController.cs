using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour {

	public BonusViewer bonusViewer;

	private float time=0.0f;
	private float bonusApparitionPeriod=10.0f;//5.0f*60.0f;// 5 minutes
	private float bonusDuration=10.0f;//5.0f*60.0f;// 5 minutes

	private bool isBonusActive=false;

	public GameObject starsContainer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (!isBonusActive) {
			if (time > bonusApparitionPeriod) {
				time = 0.0f;
				isBonusActive = true;
				LaunchBonus();
			} else {
				float percent = time / bonusApparitionPeriod;
				bonusViewer.UpdateWaiting (percent);
			}
		}
		else{
			if(time > bonusDuration){
				time=0.0f;
				isBonusActive=false;
				FinishBonus();
			}else{
				float percent = time / bonusApparitionPeriod;
				bonusViewer.UpdateLasting (percent);
			}
		}
	}
	
	private void LaunchBonus(){
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("StarContainer")) {
			StarController star=gameObject.GetComponentInChildren<StarController>();
			star.animator.SetTrigger("bonusStart");
		}
	}
	
	private void FinishBonus(){
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("StarContainer")) {
			StarController star=gameObject.GetComponentInChildren<StarController>();
			star.animator.SetTrigger("bonusEnd");
		}
		
	}

	public int GetMultiplicator(){
		if(isBonusActive){
			return 3;
		}
		else{
			return 1;
		}
	}
}
