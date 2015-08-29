using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour {

	public List<Sprite> images;

	public Image uiImage;

	public Image backStars;

	private bool isStarAnimationFinished=false;

	private Animator starsAnimator;

	// Use this for initialization
	void Start () {
		uiImage.enabled = false;
		backStars.enabled = false;
		starsAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateProgress(float percent){
		if (percent > 0.0f) {
			uiImage.enabled=true;
			int imgNumber = (int)(percent * images.Count);
			uiImage.sprite = images [imgNumber];
		}
	}

	public void LaunchStarAnimation(){
		backStars.enabled = true;
		starsAnimator.SetTrigger ("goalReached");
		isStarAnimationFinished = true;
	}

	public bool IsStarAnimationFinished{
		get{ return isStarAnimationFinished;}
	}
}
