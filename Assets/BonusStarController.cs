using UnityEngine;
using System.Collections;

public class BonusStarController : MonoBehaviour {

	private Vector3 initPos;
	private float timeSpend;
	private float speed=5.0f;
	private float timeNeededAway;
	
	private bool isGoalSet=false;
	private bool isGoalReached=false;

	public StarController starController;

	private Vector3 maxDistance=new Vector3(2.0f, 0.0f, 0.0f);

	private PlayerController playerController;

	private bool destroying=false;

	private BonusPhaseController bonusPhaseController;

	private bool wasGoingAway=true;

	void Start(){
		GameObject gameObject=GameObject.Find ("BonusPhaseController");
		bonusPhaseController=gameObject.GetComponent<BonusPhaseController> ();
		if (bonusPhaseController.IsBonusActive) {
			starController.animator.SetTrigger("bonusStart");
		}
		starController.GetComponent<Collider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGoalSet && !isGoalReached) {
			Vector3 playerPos=playerController.transform.position;
			timeSpend+=Time.deltaTime;

			//Star goes away
			if(timeSpend<timeNeededAway){
				float x=(playerPos+maxDistance*timeSpend/timeNeededAway).x;
				float y=playerPos.y;
				this.transform.position=new Vector3(x,y,0.0f);
			}
			//Star goes closer
			else if(!destroying){
				if(wasGoingAway){
					starController.GetComponent<Collider2D> ().enabled = true;
				}
				wasGoingAway=false;
				//this.transform.position=playerPos+maxDistance-maxDistance*(timeSpend-timeNeededAway)/timeNeededClose;
				this.transform.position=new Vector3(this.transform.position.x, playerPos.y, 0.0f);
			}
		}
	}
	
	
	/// <summary>
	/// If the other is a player, the star we launch the collector anim et start the destroy coroutine.
	/// </summary>
	/// <param name="other">The other collider 2D.</param>
	void OnTriggerEnter2D(Collider2D other){
		if (timeSpend > timeNeededAway && !destroying) {
			destroying=true;
			starController.Collide (other);
		}
	}

	public void SetGoal(PlayerController playerController){
		this.initPos = this.transform.position;
		this.timeSpend=0.0f;
		this.playerController = playerController;
		
		this.timeNeededAway = maxDistance.magnitude / speed;
		isGoalSet = true;
	}
}
