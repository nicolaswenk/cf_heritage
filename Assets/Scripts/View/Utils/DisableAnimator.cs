using UnityEngine;
using System.Collections;

/// <summary>
/// This class is use to disable the attached star animation.
/// In fact, it don't disable the animator but makes impossible
/// all other transitions by setting a bool parameter.
/// </summary>
public class DisableAnimator : MonoBehaviour {

	/// <summary>
	/// This methods is called by a animation event and tells to the animator
	/// that the object is destroying (no other animation can start).
	/// </summary>
	public void Destroying(){
		this.GetComponent<Animator> ().SetBool ("destroying", true);
	}
}
