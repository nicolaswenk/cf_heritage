using UnityEngine;
using System.Collections;

public class AdjustScript : MonoBehaviour {

    private PlayerProfileData profileDataObj = PlayerProfileData.profileData;

    void OnGUI ()
    {

        //3,3,3,1.5f,3.0f,10.0f,0.5f
        //nbBreathingsHigh, nbBreathingsMedium, nbBreathingsLow, inspirationTime, holdingBreathTime, expirationMinTime, volumeRange

        //nbBreathingsHigh
        if (GUI.Button(new Rect(100, 10, 20, 20), "-"))
        {
            profileDataObj.nbBreathingsHigh -= 1;
        }
        GUI.Label(new Rect(130, 10, 200, 30), "nbBreathingsHigh:" + profileDataObj.nbBreathingsHigh);

        if (GUI.Button(new Rect(300, 10, 20, 20), "+"))
        {
            profileDataObj.nbBreathingsHigh += 1;
        }

        //nbBreathingsMedium
        if (GUI.Button(new Rect(100, 30, 20, 20), "-"))
        {
            profileDataObj.nbBreathingsMedium -= 1;
        }
        GUI.Label(new Rect(130, 30, 300, 30), "nbBreathingsMedium:" + profileDataObj.nbBreathingsMedium);
        if (GUI.Button(new Rect(300, 30, 20, 20), "+"))
        {
            profileDataObj.nbBreathingsMedium += 1;
        }

        //nbBreathingsLow
        if (GUI.Button(new Rect(100, 50, 20, 20), "-"))
        {
            profileDataObj.nbBreathingsLow -= 1;
        }
        GUI.Label(new Rect(130, 50, 300, 30), "nbBreathingsLow:" + profileDataObj.nbBreathingsLow);
        if (GUI.Button(new Rect(300, 50, 20, 20), "+"))
        {
            profileDataObj.nbBreathingsLow += 1;
        }

        ////
        if (GUI.Button(new Rect(100, 70, 20, 20), "-"))
        {
            profileDataObj.inspirationTime -= .5f;
        }
        GUI.Label(new Rect(130, 70, 300, 30), "inspirationTime:" + profileDataObj.inspirationTime);
        if (GUI.Button(new Rect(300, 70, 20, 20), "+"))
        {
            profileDataObj.inspirationTime += .5f;
        }

        ////
        if (GUI.Button(new Rect(100, 90, 20, 20), "-"))
        {
            profileDataObj.holdingBreathTime -= .5f;
        }
        GUI.Label(new Rect(130, 90, 300, 30), "holdingBreathTime:" + profileDataObj.holdingBreathTime);
        if (GUI.Button(new Rect(300, 90, 20, 20), "+"))
        {
            profileDataObj.holdingBreathTime += .5f;
        }

        ////
        if (GUI.Button(new Rect(100, 110, 20, 20), "-"))
        {
            profileDataObj.expirationMinTime -= .5f;
        }
        GUI.Label(new Rect(130, 110, 300, 30), "expirationMinTime:" + profileDataObj.expirationMinTime);
        if (GUI.Button(new Rect(300, 110, 20, 20), "+"))
        {
            profileDataObj.expirationMinTime += .5f;
        }

        ////
        if (GUI.Button(new Rect(100, 130, 20, 20), "-"))
        {
            profileDataObj.volumeRange -= .1f;
        }
        GUI.Label(new Rect(130, 130, 300, 30), "volumeRange:" + profileDataObj.volumeRange);
        if (GUI.Button(new Rect(300, 130, 20, 20), "+"))
        {
            profileDataObj.volumeRange += .1f;
        }
        
        
        





    }
	

}
