using UnityEngine;
using System.Collections;

public class web_results : MonoBehaviour {

	private string secretKey = "patator4"; // Edit this value and make sure it's the same as the one stored on the server
	
	public GUIText guiText;
	public static string user; 
	private string u1;
	private string u2;
	char[] arr = new char[] { '\t', ',', ' ' };
	private string score = "155";
	
	void Start()
	{
		StartCoroutine(GetScores());
	}

	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Alpha5)) {
			StartCoroutine(PostScores());
		}
	}
	
	// remember to use StartCoroutine when calling this function!
	IEnumerator PostScores()
	{
		// This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.
		string hash = Md5Sum(user + score + secretKey);
		
		string post_url = "http://www.fibrosekystique.net/sites/add.php?name=" + WWW.EscapeURL(user) + "&score=" + score + "&hash=" + hash;
		
		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done

		if (hs_post.error != null)
		{
			print("Error - " + hs_post.error);
		}
	}
	
	// Get the scores from the MySQL DB to display in a GUIText.
	// remember to use StartCoroutine when calling this function!
	IEnumerator GetScores()
	{
		WWW hs_get = new WWW("http://www.fibrosekystique.net/?q=fr/username");
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			print("Error - " + hs_get.error);
		}
		else
		{
			user = hs_get.text;
			user = user.Replace("[]", "");
			user = user.Replace("----------------------------------------", "");
			user = user.Trim();
		}
	}
	
	public  string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}

}