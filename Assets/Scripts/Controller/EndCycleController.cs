using UnityEngine;
using System.Collections;

/// <summary>
/// This script must be placed on a collider 2D. When the player enter in it,
/// it means that the exercice cycle is over. Therefore, we instantiate a new one
/// and move the collider at the end of the next cycles. (There is always 2 cycle
/// in the scene so we don't see the other appears)
/// </summary>
public class EndCycleController : MonoBehaviour {

	/// <summary>
	/// The level controller (of a generated level).
	/// </summary>
	public GeneratedLevelController generatedLevelController;

	/// <summary>
	/// Check if the player reach it. If it does,it means that the exercice cycle is over.
	/// Therefore, we instantiate a new one and move the collider at the end of the next cycle.
	/// (There is always 2 cycles in the scene so we don't see the other appears)
	/// </summary>
	/// <param name="other">The other collider which touch this gameObject.
	/// Does something only if it has the "player" tag.</param>
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			float x=generatedLevelController.XElementsOffset;
			generatedLevelController.CreateCycleComponents(BreathingState.INSPIRATION);
			this.transform.position=new Vector3(x, 0.0f, 0.0f);
		}
	}
}
