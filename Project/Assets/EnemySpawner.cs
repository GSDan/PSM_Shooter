using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
		
	public bool shouldSpawn = false;
	float currentTime = 0;

	Stack<LevelData.LevelEvent> enemyEvents;

	// Grab all loaded enemy events and add them to the list, 
	// where they're sorted by the time at which they happen in the level
	public void addEnemies(List<LevelData.LevelEvent> allEvents)
	{
		enemyEvents = new Stack<LevelData.LevelEvent> ();
		List<LevelData.LevelEvent> sortedEvents = new List<LevelData.LevelEvent>();

		for(int i = 0; i < allEvents.Count; i++)
		{
			if(allEvents[i].type == "enemy")
			{
				sortedEvents.Add(allEvents[i]);
			}
		}

		sortedEvents.Sort ();

		// Use a stack so they can be easily traversed and removed
		enemyEvents = new Stack<LevelData.LevelEvent> (sortedEvents);
	}

	// Update is called once per frame
	void Update ()
	{
		if(!shouldSpawn) return;

		currentTime += Time.deltaTime;

		// Loop through enemy events until we find one which shouldn't happen yet
		while(enemyEvents.Count > 0)
		{
			LevelData.LevelEvent enemyEvent = enemyEvents.Peek();

			if(enemyEvent.time > currentTime)
			{
				break;
			}
			else
			{

				GameObject prefab = (GameObject) Resources.Load("Prefabs/" + enemyEvent.prefab);
				GameObject enemy = (GameObject) Instantiate(prefab, transform.position, transform.rotation);
				enemy.transform.parent = transform;
				enemy.transform.localPosition += new Vector3(0, enemyEvent.yCoord, 0);
				enemy.transform.localScale = Vector3.one;

				enemyEvents.Pop();
			}
		}
	}
	
}
