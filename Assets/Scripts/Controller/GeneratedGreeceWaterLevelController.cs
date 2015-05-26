using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the level controller of the generated level with the greece-underwater theme.
/// </summary>
public class GeneratedGreeceWaterLevelController : LevelController
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
	private float xGroundOffset=0.0f;

	/// <summary>
	/// Instantiate the exercice, find the <see cref="endCycleController"/>,
	/// create 2 exercice cycles (the ground elements, the stars and the obstacles),
	/// places the <see cref="endCycleController"/> at the end of the first one and
	/// launch the player entering animation.
	/// </summary>
	void Start ()
	{		
		endCycleController = GetComponentInChildren<EndCycleController> ();

		exercice = new DecreasingDrainageAutogene (3,3,3,1.5f,3.0f,10.0f,0.5f);

		//Flapi.Start (GetComponent<AudioSource>(), Flapi.GetMicrophone (0), 60);
		//ioController = new FlapiIOController (new ParameterManager(10,1), exercice, GetComponent<AudioSource>());
		inputController = new KeyboardInputController (exercice, 10.0f);
		
		CreateCycleComponents (InputState.HOLDING_BREATH);
		endCycleController.gameObject.transform.position = new Vector3 (xElementsOffset, 0.0f, 0.0f);
		CreateCycleComponents (InputState.INSPIRATION);
		
		StartCoroutine (WaitForPlayer ());
	}

	/// <summary>
	/// Creates all the components of a cycle (ground, stars and obstacles)
	/// </summary>
	/// <param name="startAt">Tells at which input state the cycle begin.</param>
	public void CreateCycleComponents(InputState startAt){
		float duration = exercice.Duration;
		float distance = duration * player.HorizontalSpeed;
		float xObstaclesOffset = xElementsOffset;
		
		//CreatePerfectStars (xElementsOffset, startAt);
		CreateRandomStars (xElementsOffset, startAt);

		xElementsOffset += distance;
		float offset = 0.0f;
		switch (startAt) {
		case InputState.EXPIRATION:
			offset=(exercice.Breathings[0].InspirationTime + exercice.Breathings[0].HoldingBreathTime)*player.HorizontalSpeed;
			break;
		case InputState.HOLDING_BREATH:
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
		float length = 10.24f;
		Transform groundParent = GameObject.Find ("Ground").transform;
		float initXOffset = xGroundOffset;
		while (xGroundOffset-initXOffset < distance) {
			xGroundOffset+=length;
			GameObject groundObject=(GameObject)Instantiate (groundModel, new Vector3 (xGroundOffset, player.minHeight, 0), Quaternion.identity);
			groundObject.name = "Ground_"+((int)(xGroundOffset/length));
			groundObject.transform.parent=groundParent;
		}
		return xGroundOffset;
	}

	/// <summary>
	/// Creates stars along the ideal path. Those stars are created randomly along this path.
	/// </summary>
	/// <param name="xOffset">The x position at which the first star of this cycle could start.</param>
	/// <param name="startAt">Tells at which input state the cycle begin.</param>
	private void CreateRandomStars(float xOffset, InputState startAt){//TODO Make it support the expiration input state
		Transform stars = GameObject.Find ("Stars").transform;
		float dtInspirationIdeal = 1.0f;
		float dtHoldingBreathIdeal = 1.0f;
		float dtExpirationIdeal = 5.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		float timeRef=time;
		foreach(Breathing respiration in exercice.Breathings){
			float inspirationTime=respiration.InspirationTime;
			int starRespirationCounter=1;
			if(!(respirationIndex<=0 && startAt!=InputState.INSPIRATION)){
				float dtInspirationRandom=Random.Range(dtInspirationIdeal/2.0f, dtInspirationIdeal*2.0f);
				time+=dtInspirationRandom;
				while(time-timeRef<inspirationTime){
					float y=(respiration.MaxVolume-respiration.StartVolume)*(time-timeRef)/inspirationTime + respiration.StartVolume;
					Vector3 pos=ExerciceToPlayer(new Vector3(xOffset+time, y,0));
					GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
					star.transform.parent=stars;
					star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
					dtInspirationRandom=Random.Range(dtInspirationIdeal/2.0f, dtInspirationIdeal*2.0f);
					time+=dtInspirationRandom;
					starRespirationCounter++;
				}
				timeRef=timeRef+inspirationTime;
			}
			float dtHoldingBreathRandom=Random.Range(dtHoldingBreathIdeal/2.0f, dtHoldingBreathIdeal*2.0f);
			time+=dtHoldingBreathRandom;
			while(time-timeRef<respiration.HoldingBreathTime){
				Vector3 pos=ExerciceToPlayer(new Vector3(xOffset+time, respiration.MaxVolume,0));
				GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
				star.transform.parent=stars;
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				dtHoldingBreathRandom=Random.Range(dtHoldingBreathIdeal/2.0f, dtHoldingBreathIdeal*2.0f);
				time+=dtHoldingBreathRandom;
				starRespirationCounter++;
			}
			timeRef=timeRef+respiration.HoldingBreathTime;
			float dtExpirationRandom=Random.Range(dtExpirationIdeal/2.0f, dtExpirationIdeal*2.0f);
			time+=dtExpirationRandom;
			float expirationTime=respiration.ExpirationTime;
			while(time-timeRef<expirationTime){
				Vector3 pos=ExerciceToPlayer(new Vector3(xOffset+time, respiration.MaxVolume+(time-timeRef)*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
				GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
				star.transform.parent=stars;
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				dtExpirationRandom=Random.Range(dtExpirationIdeal/2.0f, dtExpirationIdeal*2.0f);
				time+=dtExpirationRandom;
				starRespirationCounter++;
			}
			timeRef=timeRef+expirationTime;
			respirationIndex++;
		}
	}

	/// <summary>
	/// Creates regular stars along the ideal path.
	/// </summary>
	/// <param name="xOffset">The x position at which the first star of this cycle should start.</param>
	/// <param name="startAt">Tells at which input state the cycle begin.</param>
	private void CreatePerfectStars(float xElementOffset, InputState startAt){
		Transform stars = GameObject.Find ("Stars").transform;
		float dtInspiration = 0.25f;
		float dtExpiration = 0.5f;
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Breathing respiration in exercice.Breathings){
			float xOffset=0.0f;
			float inspirationTime=respiration.InspirationTime;
			int starRespirationCounter=1;
			if(!(respirationIndex<=0 && startAt!=InputState.INSPIRATION)){
				for(float i=0.0f; i<inspirationTime; i+=dtInspiration){
					Vector3 pos=ExerciceToPlayer(new Vector3(xElementOffset+time+xOffset, (respiration.MaxVolume-respiration.StartVolume)*i/inspirationTime + respiration.StartVolume,0));
					GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
					star.transform.parent=stars;
					star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
					xOffset+=dtInspiration;
					starRespirationCounter++;
				}
			}
			for(float i=0.0f;i<respiration.HoldingBreathTime;i+=1.0f){
				Vector3 pos=ExerciceToPlayer(new Vector3(xElementOffset+time+xOffset, respiration.MaxVolume,0));
				GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
				star.transform.parent=stars;
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				xOffset+=1.0f;
				starRespirationCounter++;
			}
			float expirationTime=respiration.ExpirationTime;
			for(float i=0.0f; i<expirationTime; i+=dtExpiration){
				Vector3 pos=ExerciceToPlayer(new Vector3(xElementOffset+time+xOffset, respiration.MaxVolume+i*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
				GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
				star.transform.parent=stars;
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				xOffset+=dtExpiration;
				starRespirationCounter++;
			}
			time+=xOffset;
			respirationIndex++;
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
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Breathing respiration in exercice.Breathings){
			float secureTime = 0.0f;
			if(randomDown<probaObstacleDown){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(breathingVolumeToWorldHeight(respiration.StartVolume), ObstacleType.DOWN);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(xOffset+x, respiration.StartVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleDown_"+respirationIndex;
			}		
			time+=respiration.InspirationTime + respiration.HoldingBreathTime;
			secureTime = 1.0f;
			if(randomTop<probaObstacleTop){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(breathingVolumeToWorldHeight(respiration.MaxVolume), ObstacleType.UP);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(xOffset+x, respiration.MaxVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleTop_"+respirationIndex;
			}
			time+=respiration.ExpirationTime;
			respirationIndex++;
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
