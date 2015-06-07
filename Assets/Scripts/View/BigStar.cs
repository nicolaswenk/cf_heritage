using UnityEngine;
using System.Collections;

public class BigStar : MonoBehaviour {

	private bool isApparitionFinished=false;

	public Animator animator;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.GetCurrentAnimatorClipInfo (0) [0].clip.name.Equals ("BigStar_Created")) {
			isApparitionFinished = true;
		}	
	}

	public void Appears(){
		gameObject.SetActive(true);
	}

	public bool IsApparitionFinished{
		get{ return isApparitionFinished;}
	}
}
