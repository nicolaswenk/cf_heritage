using FlapiUnity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratedLevel : Level
{
	public GameObject ground;
	public GameObject starModel;
	public List<Obstacle> listObstacleModels;
	
	// Use this for initialization
	void Start ()
	{		
		//TODO: Build the ParameterManager object by reading the properties instead of setting with those magic values
		exercice = new DrainageAutogene ();

		//ioController = new FlapiIOController (audio);
		ioController = new KeyboardIOController (new ParameterManager(10,1), exercice);

		CreateGround ();

		CreateRandomStars ();
		//CreatePerfectStars ();

		CreateObstacles (1.0f, 1.0f);
		
		StartCoroutine (WaitForPlayer ());
	}

	private void CreateGround(){
		//TODO: Make an infinite repetion (not in the start but in update).
		Debug.Log (player.minHeight);
		for (int i=0; i<20; i++) {
			Instantiate (ground, new Vector3 (i*10.24f, player.minHeight, 0), Quaternion.identity).name = "Ground_"+i;
		}
	}

	private void CreateRandomStars(){
		float dtInspirationIdeal = 1.0f;
		float dtHoldingBreathIdeal = 1.0f;
		float dtExpirationIdeal = 5.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		float timeRef=time;
		foreach(Respiration respiration in exercice.Respirations){
			float inspirationTime=(respiration.MaxVolume-respiration.StartVolume)/exercice.InspirationSpeed;
			int starRespirationCounter=1;
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				float dtInspirationRandom=Random.Range(dtInspirationIdeal/2.0f, dtInspirationIdeal*2.0f);
				time+=dtInspirationRandom;
				while(time-timeRef<inspirationTime){
					Vector3 pos=ExerciceToPlayer(new Vector3(time, (respiration.MaxVolume-respiration.StartVolume)*(time-timeRef) + respiration.StartVolume,0));
					Object star=Instantiate(starModel, pos, Quaternion.identity);
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
				Object star=Instantiate(starModel, pos, Quaternion.identity);
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				dtHoldingBreathRandom=Random.Range(dtHoldingBreathIdeal/2.0f, dtHoldingBreathIdeal*2.0f);
				time+=dtHoldingBreathRandom;
				starRespirationCounter++;
			}
			timeRef=timeRef+respiration.HoldingBreathTime;
			float dtExpirationRandom=Random.Range(dtExpirationIdeal/2.0f, dtExpirationIdeal*2.0f);
			time+=dtExpirationRandom;
			float expirationTime=-(respiration.EndVolume-respiration.MaxVolume)/exercice.ExpirationSpeed;
			while(time-timeRef<expirationTime){
				Vector3 pos=ExerciceToPlayer(new Vector3(time, respiration.MaxVolume+(time-timeRef)*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
				Object star=Instantiate(starModel, pos, Quaternion.identity);
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
		float dtInspiration = 0.5f;
		float dtExpiration = 1.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Respiration respiration in exercice.Respirations){
			float xOffset=0.0f;
			float inspirationTime=(respiration.MaxVolume-respiration.StartVolume)/exercice.InspirationSpeed;
			int starRespirationCounter=1;
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				for(float i=0.0f; i<inspirationTime; i+=dtInspiration){
					Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, (respiration.MaxVolume-respiration.StartVolume)*i + respiration.StartVolume,0));
					Object star=Instantiate(starModel, pos, Quaternion.identity);
					star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
					xOffset+=dtInspiration;
					starRespirationCounter++;
				}
			}
			for(float i=0.0f;i<respiration.HoldingBreathTime;i+=1.0f){
				Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, respiration.MaxVolume,0));
				Object star=Instantiate(starModel, pos, Quaternion.identity);
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				xOffset+=1.0f;
				starRespirationCounter++;
			}
			float expirationTime=-(respiration.EndVolume-respiration.MaxVolume)/exercice.ExpirationSpeed;
			for(float i=0.0f; i<expirationTime; i+=dtExpiration){
				Vector3 pos=ExerciceToPlayer(new Vector3(time+xOffset, respiration.MaxVolume+i*(respiration.EndVolume-respiration.MaxVolume)/respiration.ExpirationTime,0));
				Object star=Instantiate(starModel, pos, Quaternion.identity);
				star.name="Star_"+respirationIndex+"_"+starRespirationCounter;
				xOffset+=dtExpiration;
				starRespirationCounter++;
			}
			time+=xOffset;
			respirationIndex++;
		}
	}
	
	private void CreateObstacles(float probaObstacleDown, float probaObstacleTop){
		float randomDown=Random.Range (0, 100)/100.0f;
		float randomTop=Random.Range (0, 100)/100.0f;
		int respirationIndex = 0;
		float time = 0.0f;
		foreach(Respiration respiration in exercice.Respirations){
			float secureTime = 0.0f;
			if(randomDown<probaObstacleDown){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(respiration.StartVolume, ObstacleType.DOWN);
				Object obstacle=Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(x, respiration.StartVolume,0)), Quaternion.identity);
				obstacle.name="ObstacleDown_"+respirationIndex;
			}			
			if(respirationIndex>0 || exercice.State==InputState.INSPIRATION){
				time+=respiration.InspirationTime;
			}
			time+=respiration.HoldingBreathTime;
			secureTime = 1.0f;
			if(randomTop<probaObstacleTop){
				float x=(time+secureTime)*player.HorizontalSpeed;
				Obstacle obstacleModel=RandomObstacle(respiration.MaxVolume, ObstacleType.UP);
				Object obstacle=Instantiate(obstacleModel, ExerciceToPlayer(new Vector3(x, respiration.MaxVolume,0)), Quaternion.identity);
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

	private Vector3 ExerciceToPlayer(Vector3 vector){
		vector.y *= player.maxHeight - player.minHeight;
		vector.y += player.minHeight;
		vector.x *= player.HorizontalSpeed;

		return vector;
	}
	
}
