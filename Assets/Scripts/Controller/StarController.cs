using UnityEngine;
using System.Collections;

/// <summary>
/// The star controller. Manage a star collision and destroy.
/// </summary>
public class StarController : MonoBehaviour {

	/// <summary>The star animator.</summary>
	public Animator animator;
	/// <summary>The counter of instanciated stars.</summary>
	private static int starCounter=0;

	/// <summary>
	/// Initializes a new instance of the <see cref="StarController"/> class and increment <see cref="starCounter"/>.
	/// </summary>
	public StarController(){
		starCounter++;
	}

	/// <summary>
	/// Call <see cref="Collide"/>.
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	void OnTriggerEnter2D(Collider2D other){
        Debug.Log("HEY");
		Collide (other);
	}

	/// <summary>
	/// If the other is a player, the star we launch the collector anim et start the destroy coroutine.
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	public void Collide(Collider2D other){
        if (other.tag.Equals("Player")) {
			animator.SetTrigger("collected");
			GetComponent<CircleCollider2D>().enabled=false;
			StartCoroutine(KillAfterAnimation());
			Fabric.EventManager.Instance.PostEvent("StarCollected");
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
