using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script can be added as a level controller for a generated level.
/// </summary>
public class GeneratedLevelController : LevelController
{
	/// <summary>A prefab which will be used as model to instanciate new ground element</summary>
	public GameObject groundModel;
	/// <summary>A prefab which will be used as model to instanciate new stars</summary>
	public GameObject starModel;
	/// <summary>A list of prefab Obstacle models (with params correctly set).
	/// Will be used to generated the obstacles on the level.</summary>
	public List<Obstacle> listObstacleModels;
	/// <summary>The collider2D which will be placed at the end of the first exercice cycle.</summary>
	private EndCycleController endCycleController;

	/// <summary>
	/// The x offset for elements (star mainly, but also used to compute the one for the obstacles).
	/// Where (on x axis) the elements should begin.
	/// </summary>
	private float xElementsOffset=0.0f;
	/// <summary>
	/// The x offset for ground elements. Where (on x axis) the ground elements should begin.
	/// </summary>
	private float xGroundOffset=-10.0f;
	/// <summary>
	/// The ideal delta time beween each stars while inspiring.
	/// Will be multiply by a random value between 0.5 and 1.5 if <see cref="CreateBreathingStateStars"/>
	/// is called with randomDt as true; 
	/// </summary>
	private float idealDtStarInspiration=0.75f;
	/// <summary>
	/// The ideal delta time beween each stars while holding breath.
	/// Will be multiply by a random value between 0.5 and 1.5 if <see cref="CreateBreathingStateStars"/>
	/// is called with randomDt as true; 
	/// </summary>
	private float idealDtStarHoldingBreath=1.5f;
	/// <summary>
	/// The ideal delta time beween each stars while expiring.
	/// Will be multiply by a random value between 0.5 and 1.5 if <see cref="CreateBreathingStateStars"/>
	/// is called with randomDt as true; 
	/// </summary>
	private float idealDtStarExpiration=5.0f;
	/// <summary>
	/// The game world's length of a ground element.
	/// </summary>
	private const float GROUND_ELEMENT_LENGTH = 10.24f;

	/// <summary>
	/// Instantiate the exercice, find the <see cref="endCycleController"/>,
	/// create 2 exercice cycles (the ground elements, the stars and the obstacles),
	/// places the <see cref="endCycleController"/> at the end of the first one and
	/// launch the player entering animation.
	/// </summary>
	public override void BuildAndStart ()
	{
        PlayerProfileData profileDataObj = PlayerProfileData.profileData;
		endCycleController = GetComponentInChildren<EndCycleController> ();

        //inputController = new FlapiInputController (exercice, GetComponent<AudioSource>());
        inputController = new KeyboardInputController(10.0f);
        //inputController = new PepInputController();

        exercice = new DecreasingAutogenicDrainage(profileDataObj.nbBreathingsHigh, profileDataObj.nbBreathingsMedium, profileDataObj.nbBreathingsLow, profileDataObj.inspirationTime, profileDataObj.holdingBreathTime, profileDataObj.expirationMinTime, profileDataObj.volumeRange, inputController);
        ((OnlyExpirationInputController)inputController).SetExercice(exercice);


        CreateCycleComponents (BreathingState.HOLDING_BREATH);
		endCycleController.gameObject.transform.position = new Vector3 (xElementsOffset, 0.0f, 0.0f);
		CreateCycleComponents (BreathingState.INSPIRATION);
		
		StartCoroutine (WaitForGame ());
	}

	/// <summary>
	/// Creates all the components of a cycle (ground, stars and obstacles)
	/// </summary>
	/// <param name="startAt">Tells at which breathing state the cycle begin.</param>
	public void CreateCycleComponents(BreathingState startAt){
		float duration = exercice.Duration;
		float distance = duration * player.HorizontalSpeed;
		float xObstaclesOffset = xElementsOffset;

		CreateRandomStars (xElementsOffset, startAt, true);

		xElementsOffset += distance;
		float offset = 0.0f;
		switch (startAt) {
		case BreathingState.EXPIRATION:
			offset=(exercice.Breathings[0].InspirationTime + exercice.Breathings[0].HoldingBreathTime)*player.HorizontalSpeed;
			break;
		case BreathingState.HOLDING_BREATH:
			offset=(exercice.Breathings[0].InspirationTime)*player.HorizontalSpeed;		
			break;
		}
		xElementsOffset -= offset;
		xObstaclesOffset -= offset;
		
		CreateObstacles (xObstaclesOffset, 1.0f, 1.0f);
		
		xGroundOffset = CreateGround (distance, xGroundOffset);
	}

	/// <summary>
	/// Creates the ground elements for an exercice's cycle
	/// </summary>
	/// <returns>The next x position to place the next ground elements (for the next cycle).</returns>
	/// <param name="distance">The length the cycle is.</param>
	/// <param name="xGroundOffset">The x position at which the first ground element of this cycle should start.</param>
	private float CreateGround(float distance, float xGroundOffset){
		Transform groundParent = GameObject.Find ("Ground").transform;
		float initXOffset = xGroundOffset;
		while (xGroundOffset-initXOffset < distance) {
			xGroundOffset+=GROUND_ELEMENT_LENGTH;
			GameObject groundObject=(GameObject)Instantiate (groundModel, new Vector3 (xGroundOffset, player.minHeight, 0), Quaternion.identity);
			groundObject.name = "Ground_"+((int)(xGroundOffset/GROUND_ELEMENT_LENGTH));
			groundObject.transform.parent=groundParent;
		}
		return xGroundOffset;
	}
	/// <summary>
	/// Creates stars along the ideal path. Those stars are spaced following their ideal delta time 
	/// (which will vary if <see cref="isDtRandom"/> is true).
	/// </summary>
	/// <param name="xOffset">The x position at which the first star of this cycle could start.</param>
	/// <param name="startAt">Tells at which breathing state the cycle begin.</param>
	/// <param name="isDtRandom">If true, the ideal dt will vary with a random factor between 0.5 and 1.5.</param>
	private void CreateRandomStars(float xOffset, BreathingState startAt, bool isDtRandom){
		Transform stars = GameObject.Find ("Stars").transform;
		int breathingIndex = 0;
		float time = 0.0f;
		foreach(Breathing breathing in exercice.Breathings){
			float inspirationTime=breathing.InspirationTime;
			if(breathingIndex>0 || startAt==BreathingState.INSPIRATION){
				CreateBreathingStateStars(xOffset+time*player.HorizontalSpeed, breathing, BreathingState.INSPIRATION, stars, isDtRandom);
				time=time+inspirationTime;
			}
			
			if(breathingIndex>0 || startAt==BreathingState.INSPIRATION || startAt==BreathingState.HOLDING_BREATH){
				CreateBreathingStateStars(xOffset+time*player.HorizontalSpeed, breathing, BreathingState.HOLDING_BREATH, stars, isDtRandom);
				time+=breathing.HoldingBreathTime;
			}

			CreateBreathingStateStars(xOffset+time*player.HorizontalSpeed, breathing, BreathingState.EXPIRATION, stars, isDtRandom);
			time+=breathing.ExpirationTime;

			breathingIndex++;
		}
	}

	/// <summary>
	/// Creates the breathing state stars.
	/// </summary>
	/// <param name="xOffset">The x value where the breathing state should begin in the scene.</param>
	/// <param name="breathing">The breathing which the stars will follow its <see cref="state"/> .</param>
	/// <param name="state">The state of the breathing to follow.</param>
	/// <param name="starsContainer">The transform object in the scene which should contain all the stars..</param>
	/// <param name="randomDt">If set to <c>true</c> the ideal delta time will be multiplicated by a random value between
	/// 0.5 and 1.5.</param>
	private void CreateBreathingStateStars(float xOffset, Breathing breathing, BreathingState state, Transform starsContainer, bool randomDt){
		float stateTime = 0.0f;
		float idealDt = 0.0f;
		float minFactor = 0.5f;
		float maxFactor = 1.5f;
		switch (state) {
		case BreathingState.INSPIRATION:
			stateTime=breathing.InspirationTime;
			idealDt = idealDtStarInspiration;
			break;
		case BreathingState.HOLDING_BREATH:
			stateTime=breathing.HoldingBreathTime;
			idealDt = idealDtStarHoldingBreath;
			break;
		case BreathingState.EXPIRATION:
			stateTime=breathing.ExpirationTime;
			idealDt = idealDtStarExpiration;
			break;
		}
		
		float dt = idealDt;
		if (randomDt) {
			dt*=Random.Range(minFactor, maxFactor);
		}

		float time=dt;
		while(time<stateTime){
			float x=xOffset+time;
			float y = 0.0f;
			switch (state) {
			case BreathingState.EXPIRATION:
				y=breathing.MaxVolume+(time)*(breathing.EndVolume-breathing.MaxVolume)/stateTime;
				break;
			case BreathingState.INSPIRATION:
				y=(breathing.MaxVolume-breathing.StartVolume)*(time)/stateTime + breathing.StartVolume;
				break;
			case BreathingState.HOLDING_BREATH:
				y=breathing.MaxVolume;
				break;
			}
			
			GameObject star=(GameObject)Instantiate(starModel, ExerciceToPlayer(new Vector3(x,y,0.0f)), Quaternion.identity);
			star.transform.parent=starsContainer;
			star.name=StarController.GetNewName();
            star.GetComponent<Animator>().Play("Star_moving", 0, Random.Range(0.0f, 1.0f));

			dt=idealDt*Random.Range(minFactor, maxFactor);
			time+=dt;
		}
	}

	/// <summary>
	/// Creates the obstacles to invite the player to respect the perfect cycle.
	/// </summary>
	/// <param name="xOffset">The x position at which the cycle begings.</param>
	/// <param name="probaObstacleDown">Probability to create an obstacle down in a breathing (to invite the player to inhale).</param>
	/// <param name="probaObstacleTop">Probability to create an obstacle up in a breathing (to invite the player to exhale).</param>
	private void CreateObstacles(float xOffset, float probaObstacleDown, float probaObstacleTop){
		Transform obstacles = GameObject.Find ("Obstacles").transform;
		float randomDown=Random.Range (0, 100)/100.0f;
		float randomTop=Random.Range (0, 100)/100.0f;
		int breathingIndex = 0;
		float time = 0.0f;
		foreach(Breathing breathing in exercice.Breathings){
			float secureTime = 0.0f;
			if(randomDown<probaObstacleDown){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(breathingVolumeToWorldHeight(breathing.StartVolume), ObstacleType.DOWN);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(xOffset+x, breathing.StartVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleDown_"+breathingIndex;
			}		
			time+=breathing.InspirationTime + breathing.HoldingBreathTime;
			secureTime = 1.0f;
			if(randomTop<probaObstacleTop){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(breathingVolumeToWorldHeight(breathing.MaxVolume), ObstacleType.UP);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(xOffset+x, breathing.MaxVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleTop_"+breathingIndex;
			}
			time+=breathing.ExpirationTime;
			breathingIndex++;
		}
	}

	/// <summary>
	/// Chosse a random (respecting rarity) obstacle from <see cref="listObstacleModels"/> 
	/// of type <see cref="type"/> which can be placed at the given <see cref="height"/>
	/// to use as model to instantiate a new one.
	/// </summary>
	/// <returns>The chosen obstacle model.</returns>
	/// <param name="height">The height at which we want an obstacles (not all can be at all height).</param>
	/// <param name="type">The type of the obstacle we want (top, bottom or both).</param>
	private Obstacle RandomObstacle(float height, ObstacleType type){
		List<Obstacle> listPossibleObstacles=new List<Obstacle>();
		float sumProba = 0.0f;
		foreach (Obstacle obstacle in listObstacleModels) {
			if(obstacle.CanBePlaced(height, type)){
				listPossibleObstacles.Add(obstacle);
				sumProba+=1/obstacle.Rarity;
			}
		}
		float random = Random.Range (0.0f, sumProba);
		foreach(Obstacle obstacle in listPossibleObstacles){
			float obstacleProbability=1/obstacle.Rarity;
			if(random<=obstacleProbability){
				return obstacle;
			}
			else{
				random-=obstacleProbability;
			}
		}
		return null;
	}

	public void StartPlaying(){
		GameObject.Find ("BonusStarsController").SetActive (true);
	}

	
	/// <summary>
	/// Gets the x offset for elements (star mainly, but also used to compute the one for the obstacles).
	/// Where (on x axis) the elements should begin.
	/// </summary>
	public float XElementsOffset{
		get{ 
			return xElementsOffset;
		}
	}
	
}
