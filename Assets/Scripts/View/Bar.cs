using UnityEngine;
using System.Collections;

/// <summary>
/// This class represents a vertical loading bar. 
/// It is updated by increasing the y coord of a rectTransform <see cref="innerFilling"/> (which represent the filling).
/// For complex shaped bar, the rectTransform <see cref="innerFilling"/> can be in a UI Mask.
/// </summary>
public class Bar : MonoBehaviour {

	/// <summary>
	/// The RectTransform which represents the inner filling.
	/// </summary>
	public RectTransform innerFilling;
	/// <summary>
	/// The UI height which corresponds to the empty bar.
	/// </summary>
	public float min;	
	/// <summary>
	/// The UI height which corresponds to the filled bar.
	/// </summary>
	public float max;

	/// <summary>
	/// This method must be present (even empty) so the method
	/// "Update (float percent)" don't override the MonoBehaviour "Update" method.
	/// </summary>
	void Update(){
		//Nothing
	}

	/// <summary>
	/// Update the bar at the given percentage.
	/// </summary>
	/// <param name="percent">The percentage the bar must be fill.</param>
	public void Update (float percent) {
		Vector2 pos = innerFilling.anchoredPosition;
		pos.y=PercentToHeight(percent);
		innerFilling.anchoredPosition = pos;
	}

	/// <summary>
	/// Converts a percentage to the corresponding UI height.
	/// </summary>
	/// <returns>The UI height.</returns>
	/// <param name="percent">The percentage to convert.</param>
	private float PercentToHeight(float percent){
		float height = min;
		height += (max - min) * percent;
		return height;
	}
}
