using UnityEngine;
using System.Collections;

public class PepInputChart : MonoBehaviour {

    public GameObject dot;
    public GameObject zone_ok;
    public TextMesh g_text_todo;
    public TextMesh g_text_success;
    public TextMesh g_text_h2o;
    public TextMesh g_text_feedback;
    public TextMesh g_text_arduino;
    public TextMesh g_text_value;
    private float[] breathing = new float[1600];
    private float clock;
    private float clockEnd;
    private float clockExhaleEnd;
    private int  i_time = 0;
    private int  i_breath_todo = 15;
    private bool b_breath_start = true;
    private bool b_breath_end = true;
    private float f_cm;

    private float pepValue;

    void Start()
    {
        clock = Time.fixedTime + 0.05f;													    // time between each dot of the scheme
    }

    void Update()
    {
        ///// loading translation file
        lng.Translate();

        ///// collecting data from PEP

        pepValue = PepInputController.pepValue;
        g_text_arduino.text = pepValue.ToString();
        pepValue = pepValue / 100;
        g_text_value.text = pepValue.ToString();

        ///// converting data in H20 cm

        f_cm = Mathf.Round(pepValue / 0.03922f);
        g_text_h2o.text = f_cm.ToString() + " cm H2O";

        if (pepValue < 0.3081)									// -0.38378 // < 7.8 cm H20
            g_text_feedback.text = " ";
        else if (pepValue < 0.3922)								// -0.21569 // 7.8 to 10 cm H20
			g_text_feedback.text = lng.t[11];
        else if (pepValue < 0.7843)  								//  0.56863 // 10 to 20 cm H20, optimal zone
			g_text_feedback.text = lng.t[13];
		else 														// > 20 cm H20, too hard
			g_text_feedback.text = lng.t[12];		


        ///// creating chart with pressure

        clockEnd = clock - Time.fixedTime;

        if (clockEnd < 0f)
        {
            if (i_time > 20)																// if not during the first second (launch)
            {
                breathing[i_time] = pepValue;										        // saving pressure in an array
                Instantiate(dot, new Vector3(-15.5f + (i_time * 0.01f), (pepValue * 3) - 4.6f, -1), Quaternion.Euler(0, 0, 0)); // draw scheme
                clock = Time.fixedTime + 0.05f;
            }
            i_time++;
        }

        if ((clockExhaleEnd < Time.fixedTime) && (i_time > 20)) 							// if exhalation done long enough and not during launch
        {
            g_text_success.text = ":-)";													// visual feedback

            if (b_breath_start == true)														// do this action once
            {
                i_breath_todo -= 1;
                b_breath_start = false;
            }
        }
        if (i_breath_todo > 1)																// visual feedback
            g_text_todo.text = i_breath_todo.ToString() + " expirations à faire";
        else
            g_text_todo.text = i_breath_todo.ToString() + " expiration à faire";

        ///// show breathing zone for required pressure and time + smiley if succeed

        if ((pepValue < 0.3081f) || (pepValue > 0.7843f))				                    // if pressure too low or too high, restart
        {
            g_text_success.text = " ";
            b_breath_start = true;
            b_breath_end = true;
            clockExhaleEnd = Time.fixedTime + 3f;											// exhalation duration
        }
        else
        {
            if ((b_breath_end == true) && (i_time > 20)) 									// visual feedback to show how long and how much to exhale 
            {
                Instantiate(zone_ok, new Vector3(-15.2f + (i_time * 0.01f), -2.8f, -1), Quaternion.Euler(-90, 0, 0));
                b_breath_end = false;
            }
        }
    }

}
