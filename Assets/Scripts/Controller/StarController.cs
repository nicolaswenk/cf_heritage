using UnityEngine;
using System.Collections;

/// <summary>
/// The star controller. Manage a star collision and destroy.
/// </summary>
public class StarController : MonoBehaviour {

	/// <summary>The star animator.</summary>
	public Animator animator;
	private static int starCounter=0;

	public StarController(){
		starCounter++;
	}

	/// <summary>
	/// If the other is a player, the star we launch the collector anim et start the destroy coroutine.
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	void OnTriggerEnter2D(Collider2D other){
		Collide (other);
	}

	public void Collide(Collider2D other){
		if (other.tag.Equals("Player")) {
			animator.SetTrigger("collected");
			GetComponent<CircleCollider2D>().enabled=false;
			StartCoroutine(KillAfterAnimation());
		}
	}

	/// <summary>
	/// Wait 1 seconds, then "yield" and destroy this gameObject.
	/// Can be call as a coroutine.
	/// </summary>
	private IEnumerator KillAfterAnimation() {
		yield return new WaitForSeconds (1.0f);
		Destroy (gameObject.transform.parent.gameObject);
	}

	/// <summary>
	/// Return a name for a new star. The name is "Star X" where 'X' is a counter value (<see cref="starCounter"/>).
	/// </summary>
	public static string GetNewName(){
		return "Star " + starCounter;
	}
}
