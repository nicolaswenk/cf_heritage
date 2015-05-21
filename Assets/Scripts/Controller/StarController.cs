using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {

	public Animator animator;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag.Equals("Player")) {
			animator.SetTrigger("collected");
			GetComponent<CircleCollider2D>().enabled=false;
			StartCoroutine(KillAfterAnimation());
		}
	}

	private IEnumerator KillAfterAnimation() {
		yield return new WaitForSeconds (1.0f);
		Destroy (gameObject.transform.parent.gameObject);
	}
}
