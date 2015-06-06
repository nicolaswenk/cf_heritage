﻿using UnityEngine;
using System.Collections;

//TODO: Doc and change inputController to be an inputCalibratorController which takes
// care of all calibration (not game) stuff so this class only focus on transition and refresh view.
public class CalibrationController : MonoBehaviour {

	public PlayerController playerController;
	public LevelController levelController;
	/*public Vector3 initPoint;
	public Vector3 destination;
	public Vector3 failPoint;
	public Vector3 playerVelocity;*/

	private float smoothTime=20.0f;
	private float yVelocity=0.0f;
	private float xVelocity=0.0f;

	private CalibrationState state=CalibrationState.WAITING;

	private InputController_I inputController;
	
	private float volume;
	private float volumeMaxCalibrated;
	private float thresholdFactor=0.5f;
	private float maxVolume=Breathing.SupposedPatientMaxVolume;

	public Animator characterAnimator;

	/*public Camera camera;
	public Vector3 cameraStart;
	public Vector3 cameraEnd;
	public Vector3 cameraVelocity;*/

	public Bar calibrationBar;

	// Use this for initialization
	void Start () {
		//playerController.transform.position = initPoint;
		
		//inputController = new FlapiInputController (exercice, GetComponent<AudioSource>());
		inputController = new KeyboardSimpleInputController (10.0f);
		playerController.enabled = false;
		//camera.transform.position = cameraStart;
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
					calibrationBar.gameObject.SetActive(false);
					volumeMaxCalibrated=volume;
				}
				else{
					state=CalibrationState.FAIL_ANIMATION;
					characterAnimator.SetBool("isThresholdReached", false);
				}
				volume=0.0f;
			}
			break;
		case CalibrationState.TO_GAME_ANIMATION:
			Debug.Log(characterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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
		case CalibrationState.TO_GAME_ANIMATION:
			//Move(playerController.gameObject, destination, ref playerVelocity, smoothTime);
			//Move(camera.gameObject, cameraEnd, ref cameraVelocity, smoothTime);
			break;
		case CalibrationState.FAIL_ANIMATION:
			//Move(playerController.gameObject, failPoint, ref playerVelocity, smoothTime);
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
}