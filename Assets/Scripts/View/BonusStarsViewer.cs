using UnityEngine;
using System.Collections;

public class BonusStarsViewer : MonoBehaviour {

	private bool isFull=false;

	public GameObject bonusStarModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowHoldingBreathProgress(float percentage){
		if(!isFull){
			this.gameObject.SetActive (true);
			this.transform.localScale = new Vector3 (percentage, percentage, percentage);
			SpriteRenderer renderer=this.GetComponent<SpriteRenderer> ();
			renderer.color= new Color(renderer.color.r, renderer.color.g, renderer.color.b, percentage);

			if (percentage >= 1.0f) {
				isFull=true;
				Animator animator=this.GetComponent<Animator>();
				animator.SetTrigger("Full");
			}
		}
	}

	public void ReleaseStar(PlayerController player){
		GameObject gameObject=(GameObject)Instantiate (bonusStarModel, this.transform.position, Quaternion.identity);
		BonusStarController bonusStar=gameObject.GetComponent<BonusStarController> ();
		bonusStar.SetGoal(player);
	}

	public void HideHoldingBreathProgress(){
		isFull = false;
		Debug.Log ("hide");
		this.gameObject.SetActive (false);
	}
}
