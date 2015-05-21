using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {

	public RectTransform innerFilling;
	public float min;
	public float max;

	// Use this for initialization
	void Start () {
	
	}

	void Update(){
	}

	public void Update (float percent) {
		Vector2 pos = innerFilling.anchoredPosition;
		pos.y=PercentToHeight(percent);
		innerFilling.anchoredPosition = pos;
	}

	private float PercentToHeight(float percent){
		float height = min;
		height += (max - min) * percent;
		return height;
	}
}
