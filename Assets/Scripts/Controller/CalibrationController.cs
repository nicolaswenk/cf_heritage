using UnityEngine;
using System.Collections;

//TODO: Doc and change inputController to be an inputCalibratorController which takes
// care of all calibration (not game) stuff so this class only focus on transition and refresh view.
public class CalibrationController : MonoBehaviour {

	public PlayerController playerController;
	public LevelController levelController;

	private float smoothTime=20.0f;
	private float yVelocity=0.0f;
	private float xVelocity=0.0f;

	private CalibrationState state=CalibrationState.WAITING;

	private InputController_I inputController;
	
	private float volume;
	private static float volumeMaxCalibrated;
	private float thresholdFactor=0.5f;
	private float maxVolume=Breathing.SupposedPatientMaxVolume;

	public Animator characterAnimator;

	public Bar calibrationBar;

	// Use this for initialization
	void Start () {		
		//inputController = new FlapiInputController (exercice, GetComponent<AudioSource>());
		inputController = new KeyboardSimpleInputController (10.0f);
	}
	
	// Update is called once per frame
	void Update () {

		inputController.Update ();

		DetectStateChanging ();

		UpdateState ();
	}
	
	private void DetectStateChanging(){
		
		switch (state) {
		case CalibrationState.WAITING:
			if (inputController.GetInputState()==BreathingState.EXPIRATION){
				state=CalibrationState.CHARGING;
				characterAnimator.SetBool("isExpiring", true);
				Update();
			}
			break;
		case CalibrationState.CHARGING:
			if(inputController.GetInputState()!=BreathingState.EXPIRATION){				
				characterAnimator.SetBool("isExpiring", false);
				if (volume>=thresholdFactor*maxVolume){					
					characterAnimator.SetBool("isThresholdReached", true);
					state=CalibrationState.TO_GAME_ANIMATION;
					volumeMaxCalibrated=volume;
                    inputController.SetCalibrationFactor(CalibrationController.StrengthCalibrationFactor);
					levelController.BuildAndStart();
					calibrationBar.gameObject.SetActive(false);
				}
				else{
					state=CalibrationState.FAIL_ANIMATION;
					characterAnimator.SetBool("isThresholdReached", false);
				}
				volume=0.0f;
			}
			break;
		case CalibrationState.TO_GAME_ANIMATION:
			if (characterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("Player_Swim")){
				levelController.GameState=GameState.GAME;
				levelController.VolumeMaxCalibrated=volumeMaxCalibrated;
				this.enabled=false;
			}
			break;
		case CalibrationState.FAIL_ANIMATION:			
			if(characterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("Player_Calibration_Idle")){
				state=CalibrationState.WAITING;				
				volume=0.0f;
				calibrationBar.Update(volume);
			}
			break;
		}
	}
	
	private void UpdateState(){
		switch (state) {
		case CalibrationState.WAITING:
			break;
		case CalibrationState.CHARGING:
			volume+=inputController.GetStrength()*Time.deltaTime*Breathing.StrengthToVolumeFactor;
			calibrationBar.Update(volume/maxVolume);
			break;
		}
	}

	public void Move(GameObject objectToMove, Vector3 destination, ref Vector3 velocity, float smoothTime){
		float vX = velocity.x;
		float vY = velocity.y;
		float vZ = velocity.z;
		float x = Mathf.SmoothDamp (objectToMove.transform.position.x, destination.x, ref vX, smoothTime * Time.deltaTime);
		float y = Mathf.SmoothDamp (objectToMove.transform.position.y, destination.y, ref vY, smoothTime * Time.deltaTime);
		float z = Mathf.SmoothDamp (objectToMove.transform.position.z, destination.z, ref vZ, smoothTime * Time.deltaTime);
		objectToMove.transform.position = new Vector3 (x, y, z);
		velocity = new Vector3 (vX, vY, vZ);
	}
	
	public static float VolumeMaxCalibrated{
		get{ return volumeMaxCalibrated;}
	}
	
	public static float StrengthCalibrationFactor{
		get{return Breathing.SupposedPatientMaxVolume/volumeMaxCalibrated;}
	}
}
