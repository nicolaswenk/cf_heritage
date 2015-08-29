using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsViewer : MonoBehaviour {
	
	public List<Planet> planets;
	private bool isFinished=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator ShowPlanets(float planetNumber){		
		foreach (Planet planet in planets) {
			float percentStop;

			if(planetNumber>=1){
				percentStop=1.0f;
				planetNumber--;
			}
			else{
				percentStop=planetNumber;
				planetNumber=0.0f;
			}

			if(percentStop>0.0f){
				for (float percent=0.0f; percent<percentStop; percent+=0.01f) {
					planet.UpdateProgress(percent);
					
					yield return new WaitForSeconds(0.01f);
				}
			}

			if (percentStop == 1.0f) {
				planet.LaunchStarAnimation();
				while(!planet.IsStarAnimationFinished){
					yield return null;
				}
			}
		}
		isFinished = true;
	}

	public bool IsFinished{
		get{return isFinished;}
	}
}
