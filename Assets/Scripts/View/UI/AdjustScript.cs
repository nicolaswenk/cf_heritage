using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AdjustScript : MonoBehaviour {

    private PlayerProfileData profileDataObj = PlayerProfileData.profileData;
	public GameObject textHigh;
	public GameObject textMedium;
	public GameObject textLow;
	public GameObject textInspiration;
	public GameObject textHolding;
	public GameObject textExpiration;


    void OnGUI ()
    {
		var tComp = textHigh.GetComponent<Text>();
		tComp.text = "Nombre de respiration en haut volume :"+ profileDataObj.nbBreathingsHigh;

		tComp = textMedium.GetComponent<Text>();
		tComp.text = "Nombre de respiration en volume intermédiaire :"+ profileDataObj.nbBreathingsMedium;

		tComp = textLow.GetComponent<Text>();
		tComp.text = "Nombre de respiration en bas volume :"+ profileDataObj.nbBreathingsLow;

		tComp = textInspiration.GetComponent<Text>();
		tComp.text = "Temps d'inspiration :"+ profileDataObj.inspirationTime;

		tComp = textHolding.GetComponent<Text>();
		tComp.text = "Temps de pause inspiratoire :"+ profileDataObj.holdingBreathTime;

		tComp = textExpiration.GetComponent<Text>();
		tComp.text = "Temps d'expiration :"+ profileDataObj.expirationMinTime;

    }
	

}
