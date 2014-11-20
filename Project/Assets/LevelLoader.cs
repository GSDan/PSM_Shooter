using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class LevelLoader : MonoBehaviour {

	public static LevelData loadLevel(string fileAddress)
	{
		// load the json file and then parse with the JSON library
		TextAsset textData = Resources.Load (fileAddress) as TextAsset;
		var json = JSONNode.Parse (textData.text);

		if (json == null)
		{
			Debug.LogError("Failed to load json at " + fileAddress);
			return null;
		}

		string levelTitle = json ["title"];
		int scrollSpeed = json ["scrollSpeed"].AsInt;

		LevelData thisLevel = new LevelData (levelTitle, scrollSpeed);

		JSONArray events = json ["events"].AsArray;

		for(int i = 0; i < events.Count; i++)
		{
			thisLevel.addEvent(
				events[i]["time"].AsFloat,
				events[i]["type"],
				events[i]["prefabName"],
				events[i]["yCoord"].AsFloat
			);
		}

		return thisLevel;
	}


}

// Class in which a level's events and details are stored 
public class LevelData 
{
	public string title;
	public int scrollSpeed;
	public List<LevelEvent> events;


	public LevelData(string title, int scrollSpeed)
	{
		this.title = title;
		this.scrollSpeed = scrollSpeed;
		events = new List<LevelEvent> ();
	}

	public void addEvent(float time, string type, string prefab, float yCoord)
	{
		events.Add (new LevelEvent (time, type, prefab, yCoord));
	}

	// Can potentially hold all sorts of different events - enemies, dialogue etc
	public class LevelEvent :IComparable <LevelEvent>
	{
		public float time;
		public string type;
		public string prefab;
		public float yCoord;

		public LevelEvent(float time, string type, string prefab, float yCoord)
		{
			this.time = time;
			this.type = type;
			this.prefab = prefab;
			this.yCoord = yCoord;
		}

		public int CompareTo(LevelEvent rhs)
		{
			//if(rhs == null) return 1;
			if(time == rhs.time) return 0;
			if(time > rhs.time) return -1;
			else return 1;
		}
	}

}