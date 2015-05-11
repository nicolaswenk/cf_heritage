using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public Animator animator;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag.Equals("Player")) {
			animator.SetTrigger("collected");
			GetComponent<CircleCollider2D>().enabled=false;
		}
	}
}
