using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TowerDefense.UI.HUD;
using UnityEngine.UI;

public class ScoreList : MonoBehaviour
{

	public GameObject scoreTemplatePrefab;

	public Transform container;
   

	public void SetFinalTimerScore()
	{
		DateTime currentTime = DateTime.Now;
		TimeSpan timeSpan = currentTime - GameUI.instance.startWaveTime;

		BestScores.Instance.SetBestScore(Convert.ToSingle(timeSpan.TotalSeconds));
		foreach (Transform t in container)
			Destroy(t.gameObject);

		foreach(var s in BestScores.Instance.scores)
		{
			GameObject score = Instantiate(scoreTemplatePrefab, container);
			score.GetComponentInChildren<Text>().text = (int)s.timePlayed + " seconds";
		}

	}
}

[Serializable]
public class TimeScore
{
   public float timePlayed = 0;

	public TimeScore(float time)
	{
		timePlayed = time;
	}

	public TimeScore()
	{

	}
}

[Serializable]
public class BestScores
{
	
	[SerializeField]
    public List<TimeScore> scores = new List<TimeScore>();



	static BestScores instance;
	public static BestScores Instance
	{
		get
		{
			if(instance==null)
			{
				LoadScoreData();
			}
			return instance;
		}
	}

	public void SetBestScore(float time)
	{
		GetBestScore();
		scores.Add(new TimeScore(time));
		scores.Sort((x, y) => x.timePlayed.CompareTo(y.timePlayed));
		if (scores.Count > 5)
			scores.RemoveAt(scores.Count - 1);
		SaveScore();

	}

	public List<TimeScore> GetBestScore()
	{
		if(scores==null ||scores.Count==0)
		{
			scores = new List<TimeScore>();
		}
		scores.Sort((x, y) => x.timePlayed.CompareTo(y.timePlayed));
		return scores;
	}
	/// <summary>
	/// Loads the score data of the user.
	/// </summary>
	/// <returns>Whether the profile could be loaded.</returns>
	private static  void LoadScoreData()
	{
		if (File.Exists(Application.persistentDataPath + PathOnPlatform("/scores.txt")))
		{
			instance = XMLSerializer.Load<BestScores>(Application.persistentDataPath + PathOnPlatform("/scores.txt"));
		}
		else
			instance = new BestScores();


	}

	/// <summary>
	/// Save the profile.
	/// </summary>
	/// 

	public void SaveScore()
	{
		//Debug.Log ("Profile written");
		XMLSerializer.Save<BestScores>(Application.persistentDataPath + PathOnPlatform("/scores.txt"), instance);
		Debug.Log("saved");

	}

	private static string PathOnPlatform(string path)
	{

		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			path = path.Replace("/", @"\");
		else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			path = path.Replace(@"\", "/");
		return path;
	}

}

