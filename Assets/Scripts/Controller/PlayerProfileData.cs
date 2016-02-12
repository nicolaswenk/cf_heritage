using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerProfileData : MonoBehaviour {

    public static PlayerProfileData profileData;

    //3,3,3,1.5f,3.0f,10.0f,0.5f

    public int nbBreathingsHigh=3;
    public int nbBreathingsMedium=3;
    public int nbBreathingsLow=3;
    public float inspirationTime=3.0f;
    public float holdingBreathTime=3.0f;
    public float expirationMinTime=10.0f;
    public float volumeRange=0.5f;

    // Use this for initialization
    void Awake ()
    {
	    if (profileData==null)
        {
            DontDestroyOnLoad(gameObject);
            profileData = this;
        }
        else if(profileData != this)
        {
            Destroy(gameObject);
        }
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath+"/playerInfo.dat", FileMode.Create);

        PlayerData data = new PlayerData();

        //passe datas to serialized object
        data.nbBreathingsHigh = nbBreathingsHigh;
        data.nbBreathingsMedium = nbBreathingsMedium;
        data.nbBreathingsLow = nbBreathingsLow;
        data.inspirationTime = inspirationTime;
        data.holdingBreathTime = holdingBreathTime;
        data.expirationMinTime = expirationMinTime;
        data.volumeRange = volumeRange;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //get data from serialized object
            nbBreathingsHigh = data.nbBreathingsHigh;
            nbBreathingsMedium = data.nbBreathingsMedium;
            nbBreathingsLow = data.nbBreathingsLow;
            inspirationTime = data.inspirationTime;
            holdingBreathTime = data.holdingBreathTime;
            expirationMinTime = data.expirationMinTime;
            volumeRange = data.volumeRange;
        }
    }
}

[Serializable]
class PlayerData
{
    public int nbBreathingsHigh;
    public int nbBreathingsMedium;
    public int nbBreathingsLow;
    public float inspirationTime;
    public float holdingBreathTime;
    public float expirationMinTime;
    public float volumeRange;
}
