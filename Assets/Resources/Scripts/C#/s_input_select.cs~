using UnityEngine;
using System.Collections;

public class s_input_select : MonoBehaviour {

	void Start () {
		GetComponent<s_input_keyboard>().enabled = true;
		GetComponent<s_input_micro>().enabled = false;
		GetComponent<s_input_pep_onatha>().enabled = false;
		GetComponent<s_input_pep_redbox>().enabled = false;
		GetComponent<s_input_pep_redbox2>().enabled = false;
	}

	void Update () {
		if (Input.GetKey (KeyCode.Alpha7)) {
			s_input_time.arrow_up += 5;
			s_input_time.arrow_down += 7;
			s_input_time.arrow_left += 4;
			s_input_time.arrow_right += 8;
		}


		if (Input.GetKey (KeyCode.T))
		{
			GetComponent<s_input_keyboard>().enabled = true;
			GetComponent<s_input_micro>().enabled = false;
			GetComponent<s_input_pep_onatha>().enabled = false;
			GetComponent<s_input_pep_redbox>().enabled = false;
			GetComponent<s_input_pep_redbox2>().enabled = false;
		}
		if (Input.GetKey (KeyCode.U))
		{
			GetComponent<s_input_keyboard>().enabled = false;
			GetComponent<s_input_micro>().enabled = true;
			GetComponent<s_input_pep_onatha>().enabled = false;
			GetComponent<s_input_pep_redbox>().enabled = false;
			GetComponent<s_input_pep_redbox2>().enabled = false;
		}
		if (Input.GetKey (KeyCode.I))
		{
			GetComponent<s_input_keyboard>().enabled = false;
			GetComponent<s_input_micro>().enabled = false;
			GetComponent<s_input_pep_onatha>().enabled = true;
			GetComponent<s_input_pep_redbox>().enabled = false;
			GetComponent<s_input_pep_redbox2>().enabled = false;
		}
		if (Input.GetKey (KeyCode.O))
		{
			GetComponent<s_input_keyboard>().enabled = false;
			GetComponent<s_input_micro>().enabled = false;
			GetComponent<s_input_pep_onatha>().enabled = false;
			GetComponent<s_input_pep_redbox>().enabled = true;
			GetComponent<s_input_pep_redbox2>().enabled = false;
		}
		if (Input.GetKey (KeyCode.P))
		{
			GetComponent<s_input_keyboard>().enabled = false;
			GetComponent<s_input_micro>().enabled = false;
			GetComponent<s_input_pep_onatha>().enabled = false;
			GetComponent<s_input_pep_redbox>().enabled = false;
			GetComponent<s_input_pep_redbox2>().enabled = true;
		}
	}
}
