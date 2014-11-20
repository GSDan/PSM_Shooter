using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public UILabel scoreLabel;
	public EnemySpawner spawner;

	int score = 0;

	public void AddPoints(int points)
	{
		score += points;
		scoreLabel.text = score.ToString ();
	}

	void Start()
	{
		LevelData thisLevel = LevelLoader.loadLevel ("Levels/level001");
		Debug.Log ("Successfully loaded level: " + thisLevel.title);

		spawner.addEnemies (thisLevel.events);
		spawner.shouldSpawn = true;
	}
}
