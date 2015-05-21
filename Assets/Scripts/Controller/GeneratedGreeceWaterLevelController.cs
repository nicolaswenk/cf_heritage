using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratedGreeceWaterLevelController : LevelController
{
	public GameObject ground;
	public GameObject starModel;
	public List<Obstacle> listObstacleModels;
	
	// Use this for initialization
	void Start ()
	{		
		//TODO: Build the ParameterManager object by reading the properties instead of setting with those magic values
		exercice = new DrainageAutogene (3,3,3,1.5f,3.0f,10.0f,0.5f);

		//Flapi.Start (GetComponent<AudioSource>(), Flapi.GetMicrophone (0), 60);
		//ioController = new FlapiIOController (new ParameterManager(10,1), exercice, GetComponent<AudioSource>());
		ioController = new KeyboardInputController (exercice, 10.0f);

		CreateGround ();

		CreateRandomStars ();
		//CreatePerfectStars ();

		CreateObstacles (1.0f, 1.0f);
		
		StartCoroutine (WaitForPlayer ());
	}

	private void CreateGround(){
		Transform groundParent = GameObject.Find ("Ground").transform;
		//TODO: Make an infinite repetion (not in the start but in update).
		for (int i=0; i<20; i++) {
			GameObject groundObject=(GameObject)Instantiate (ground, new Vector3 (i*10.24f, player.minHeight, 0), Quaternion.identity);
			groundObject.name = "Ground_"+i;
			groundObject.transform.parent=groundParent;
		}
	}

	private void CreateRandomStars(){
		Transform stars = GameObject.Find ("Stars").transform;
		float dtInspirationIdeal = 1.0f;
		float dtHoldingBreathIdeal = 1.0f;
		float dtExpirationIdeal = 5.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		float timeRef=time;
		foreach(Breathing respiration in exercice.Breathings){
			float inspirationTime=(respiration.MaxVolume-respiration.StartVolume)/exercice.IdealInspirationSpeed;
			int starRespirationCounter=1;
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				float dtInspirationRandom=Random.Range(dtInspirationIdeal/2.0f, dtInspirationIdeal*2.0f);
				time+=dtInspirationRandom;
				while(time-timeRef<inspirationTime){
					Vector3 pos=ExerciceToPlayer(new Vector3(time, (respiration.MaxVolume-respiration.StartVolume)*(time-timeRef) + respiration.StartVolume,0));
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
				Vector3 pos=ExerciceToPlayer(new Vector3(time, respiration.MaxVolume,0));
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
			float expirationTime=-(respiration.EndVolume-respiration.MaxVolume)/exercice.IdealExpirationSpeed;
			while(time-timeRef<expirationTime){
				Vector3 pos=ExerciceToPlayer(new Vector3(time, respiration.MaxVolume+(time-timeRef)*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
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

	private void CreatePerfectStars(){
		Transform stars = GameObject.Find ("Stars").transform;
		float dtInspiration = 0.5f;
		float dtExpiration = 1.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Breathing respiration in exercice.Breathings){
			float xOffset=0.0f;
			float inspirationTime=(respiration.MaxVolume-respiration.StartVolume)/exercice.IdealInspirationSpeed;
			int starRespirationCounter=1;
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				for(float i=0.0f; i<inspirationTime; i+=dtInspiration){
					Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, (respiration.MaxVolume-respiration.StartVolume)*i + respiration.StartVolume,0));
					GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
					star.transform.parent=stars;
					star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
					xOffset+=dtInspiration;
					starRespirationCounter++;
				}
			}
			for(float i=0.0f;i<respiration.HoldingBreathTime;i+=1.0f){
				Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, respiration.MaxVolume,0));
				GameObject star=(GameObject)Instantiate(starModel, pos, Quaternion.identity);
				star.transform.parent=stars;
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				xOffset+=1.0f;
				starRespirationCounter++;
			}
			float expirationTime=-(respiration.EndVolume-respiration.MaxVolume)/exercice.IdealExpirationSpeed;
			for(float i=0.0f; i<expirationTime; i+=dtExpiration){
				Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, respiration.MaxVolume+i*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
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
	
	private void CreateObstacles(float probaObstacleDown, float probaObstacleTop){
		Transform obstacles = GameObject.Find ("Obstacles").transform;
		float randomDown=Random.Range (0, 100)/100.0f;
		float randomTop=Random.Range (0, 100)/100.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Breathing respiration in exercice.Breathings){
			float secureTime = 0.0f;
			if(randomDown<probaObstacleDown){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(respirationToWorldHeight(respiration.StartVolume), ObstacleType.DOWN);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(x, respiration.StartVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleDown_"+respirationIndex;
			}			
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				time+=respiration.InspirationTime;
			}
			time+=respiration.HoldingBreathTime;
			secureTime = 1.0f;
			if(randomTop<probaObstacleTop){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(respirationToWorldHeight(respiration.MaxVolume), ObstacleType.UP);
				Obstacle obstacle=(Obstacle)Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(x, respiration.MaxVolume,0)), Quaternion.identity);
				obstacle.transform.parent=obstacles;
				obstacle.name="ObstacleTop_"+respirationIndex;
			}
			time+=respiration.ExpirationTime;
			respirationIndex++;
		}
	}

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

	private float respirationToWorldHeight(float respirationValue){
		float worldHeight = respirationValue * (player.maxHeight - player.minHeight);
		worldHeight += player.minHeight;
		return worldHeight;
	}

	private Vector3 ExerciceToPlayer(Vector3 vector){
		vector.y = respirationToWorldHeight (vector.y);
		vector.x *= player.HorizontalSpeed;

		return vector;
	}
	
}
