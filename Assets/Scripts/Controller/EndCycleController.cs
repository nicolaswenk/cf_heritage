using UnityEngine;
using System.Collections;

public class EndCycleController : MonoBehaviour {

	public GeneratedGreeceWaterLevelController generatedLevelController;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			generatedLevelController.CreateCycleComponents(InputState.INSPIRATION);
			this.transform.position=new Vector3(generatedLevelController.XElementsOffset, 0.0f, 0.0f);
		}
	}
}
