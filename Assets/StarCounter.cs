using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarCounter : MonoBehaviour {

	private int starCollected=0;
	public Text label;
	
	public void Collect(int numberCollected){
		starCollected += numberCollected;
		UpdateLabel ();
	}

	public void Loose(int numberLost){
		starCollected -= numberLost;
		if (starCollected < 0) {
			starCollected = 0;
		}
		UpdateLabel ();
	}

	private void UpdateLabel (){
		label.text = starCollected.ToString();
	}
}
