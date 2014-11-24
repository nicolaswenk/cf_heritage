using UnityEngine;
using System.Collections;

public class s_pep_smooth : MonoBehaviour {
	
	public Transform e_breath;
	public float pressure;

	void Start () {
	}
	
	void Update () {

		float breath_y = e_breath.position.y;

		breath_y = Mathf.Clamp(breath_y, -2f, 3f);

		e_breath.position = new Vector2(e_breath.position.x, breath_y);



		transform.position = Vector2.Lerp(transform.position, e_breath.position, 0.3f * Time.deltaTime); 

		//	new Vector2(transform.position.x, Mathf.Lerp(e_breath.position.y, e_breath.position.y, 0.01f * Time.deltaTime));

		if (pressure < 0.3f)
			renderer.material.color = Color.red;
		if ((pressure > 0.3f) && (pressure < 1.0f)) {
			renderer.material.color = Color.green;

		}
		if (pressure > 2.0f)
			renderer.material.color = Color.blue;
		
		// end_level = clock - (int)Time.fixedTime;
	}
}
