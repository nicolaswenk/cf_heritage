using UnityEngine;
using System.Collections;

public class DisableAnimatorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisableAnimator(){
		GetComponent<Animator> ().SetBool ("destroying", true);
	}
}
