using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathController : MonoBehaviour {

    private BezierSpline spline;
    private LevelController levelController;
    private float controlPointDistance = 1.0f;
    private PlayerController playerController;
    private BreathingState lastState=BreathingState.INSPIRATION;
    private float lastStartX = 0.0f;

    // Use this for initialization
    void Start () {
        spline =GetComponentInChildren<BezierSpline>();
        Debug.Log(spline);
        levelController = GameObject.FindObjectOfType<LevelController>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (levelController.GameState == GameState.GAME)
        {

            if (levelController.InputController.GetInputState() == BreathingState.INSPIRATION && lastState != BreathingState.INSPIRATION)
            {
                lastStartX = playerController.gameObject.transform.position.x;
            }

            //List<Breathing> listBreathings = levelController.Exercice.Breathings;
            List<Breathing> listBreathings = levelController.Exercice.GetActualBreathings(-1, 3);
            List<Vector3> listKeyPoints = ExtractWorldPoints(listBreathings, lastStartX, 1);
            spline.Reset();
            listKeyPoints = AddControlPoints(listKeyPoints);
            Vector3[] keyPoints = listKeyPoints.ToArray();
            for (int i=0; i<keyPoints.Length-3; i+=3)
            {
                if (i > 0)
                {
                    spline.AddCurve();
                }
                spline.SetControlPoint(i, keyPoints[i]);
                spline.SetControlPoint(i+1, keyPoints[i+1]);
                if (i > 0)
                {
                    spline.SetControlPoint(i - 1, keyPoints[i - 1]);
                }
            }
            spline.SetControlPoint(keyPoints.Length - 1, keyPoints[keyPoints.Length - 1]);
            spline.SetControlPoint(keyPoints.Length - 2, keyPoints[keyPoints.Length - 2]);

            lastState = levelController.InputController.GetInputState();
        }

    }

    public List<Vector3> AddControlPoints(List<Vector3> listKeyPoints)
    {
        List<Vector3> listControlPoints = new List<Vector3>((listKeyPoints.Count - 1) * 3);
        Vector3[] controlPoints = listKeyPoints.ToArray();

        for(int i=0; i<controlPoints.Length-1; i++)
        {
            listControlPoints.Add(controlPoints[i]);
            listControlPoints.Add(controlPoints[i]+Vector3.right*controlPointDistance);
            listControlPoints.Add(controlPoints[i+1]- Vector3.right * controlPointDistance);
        }
        listControlPoints.Add(controlPoints[controlPoints.Length - 1]);

        return listControlPoints;
    }

    public List<Vector3> ExtractWorldPoints(List<Breathing> listBreathings, float offsetX, int alignOffsetIndex)
    {
        List<Vector3> listPoints = new List<Vector3>(listBreathings.Count*3);

        int index = 0;
        float actualOffset = 0.0f;
        while (index<alignOffsetIndex)
        {
            actualOffset -= listBreathings[index].HoldingBreathTime;
            actualOffset -= listBreathings[index].ExpirationTime;
            actualOffset -= listBreathings[index].InspirationTime;
            index++;
        }
        actualOffset += offsetX;

        foreach (Breathing breathing in listBreathings)
        {
            //Start Inspiration
            float y = breathing.EndVolume;
            listPoints.Add(levelController.ExerciceToPlayer(new Vector3(actualOffset, y, 0.0f)));
            actualOffset += breathing.InspirationTime;

            //Start Holding breath
            y = breathing.MaxVolume;
            listPoints.Add(levelController.ExerciceToPlayer(new Vector3(actualOffset, y, 0.0f)));
            actualOffset += breathing.HoldingBreathTime;

            //Start Expiration
            y = breathing.MaxVolume;
            listPoints.Add(levelController.ExerciceToPlayer(new Vector3(actualOffset, y, 0.0f)));
            actualOffset += breathing.ExpirationTime;
        }

        return listPoints;
    }
}
