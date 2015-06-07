using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndCanvasController : MonoBehaviour {
	
	public RectTransform starBar;
	public RectTransform endCanvas;

	public PlanetsViewer planetsViewer;
	private EndCanvasState state;

	public LevelController levelController;

	public Button quitButton;

	public BigStarsViewer bigStarsViewer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case EndCanvasState.PLANETS_SHOWING:
			if (planetsViewer.IsFinished){
				state=EndCanvasState.STARS_SHOWING;
				StartCoroutine (bigStarsViewer.ShowBigStars (8));
			}
			break;
		case EndCanvasState.STARS_SHOWING:
			if(bigStarsViewer.IsFinished){
				state=EndCanvasState.WAITING_QUIT;
				quitButton.interactable=true;
			}
			break;
		}
	}

	
	public void QuitClicked (){
		quitButton.interactable = false;
		levelController.GameState = GameState.END_SCREEN;
		starBar.gameObject.SetActive (false);
		endCanvas.gameObject.SetActive (true);

		state = EndCanvasState.PLANETS_SHOWING;
		StartCoroutine (planetsViewer.ShowPlanets (1.5f));
	}
}
