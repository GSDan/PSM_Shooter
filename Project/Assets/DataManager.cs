using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;

public class DataManager : MonoBehaviour {

	public static DataManager manager;
	public GameData data;
	public List<string> levelLocs;

	private bool attemptedLoad = false;
	private static string saveLoc;

	void Awake () {
		if(manager == null)
		{
			// make this the persistent data manager (works across scenes)
			DontDestroyOnLoad(gameObject);
			manager = this;
			saveLoc = Application.persistentDataPath + "/playerSave.dat";

			if(!attemptedLoad)
			{
				Load();
				attemptedLoad = true;
			}
		}
		else
		{
			// This object is redundant, delete
			GameObject.Destroy(gameObject);
		}
	}

	public void Save()
	{
		if(data == null)
		{
			data = new GameData();
		}

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (saveLoc, FileMode.OpenOrCreate);

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if(File.Exists(saveLoc))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (saveLoc, FileMode.Open);

			data = (GameData) bf.Deserialize(file);
		}
		else
		{
			Save ();
		}
	}

	// Clear any existing level locations and load them anew
	public void LoadLevelList()
	{
		levelLocs.Clear ();

		// load the json file and then parse with the JSON library
		TextAsset textData = Resources.Load ("Levels/_Data") as TextAsset;
		var json = JSONNode.Parse (textData.text);
		
		if (json == null)
		{
			Debug.LogError("Failed to load json at " + "Levels/_Data");
			return;
		}

		JSONArray levelList = json ["levels"].AsArray;

		for(int i = 0; i < levelList.Count; i++)
		{
			Debug.Log("Adding level " + levelList[i]);
			levelLocs.Add(levelList[i]);
		}
	}

}

[Serializable]
public class GameData
{
	public string playerName = "Player";
	public int currentLevel = 1;
	public Dictionary<int,int> levelScores;

	public GameData()
	{
		levelScores = new Dictionary<int, int> ();
	}

	// Enter a score if it is greater than existing or new. Return true if dictionary updated
	public bool EnterScoreIfGreater(int level, int score)
	{
		if(levelScores.ContainsKey(level))
		{
			int existing = levelScores[level];

			if(existing >= score)
			{
				return false;
			}
			else
			{
				levelScores[level] = score;
				return true;
			}
		}
		else
		{
			levelScores.Add(level, score);
			return true;
		}
	}
}