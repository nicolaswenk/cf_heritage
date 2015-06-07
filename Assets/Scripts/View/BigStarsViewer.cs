using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigStarsViewer : MonoBehaviour {

	private bool isFinished=false;

	private BigStar[] listBigStars;

	// Use this for initialization
	void Start () {
		listBigStars=gameObject.GetComponentsInChildren<BigStar> (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator ShowBigStars(int starNumber){
		for (int i =0; i < starNumber; i++) {
			listBigStars[i].Appears();
			while(!listBigStars[i].IsApparitionFinished){
				yield return null;
			}
		}
		isFinished=true;
	}

	public bool IsFinished{
		get{return isFinished;}
	}
}
