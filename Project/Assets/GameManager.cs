using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public UILabel scoreLabel;
	public EnemySpawner spawner;
	public SceneManager sceneManager;
	public SceneryManager scenery;
	public PlayerController player;

	int score = 0;

	public void AddPoints(int points)
	{
		score += points;
		scoreLabel.text = score.ToString ();
	}

	public void GameOver()
	{
		scenery.isMoving = false;
		spawner.shouldSpawn = false;
		sceneManager.showGameOver ();
	}

	public void LevelComplete()
	{
		spawner.Reset ();
		this.Start (); // TODO load next level
	}

	public void Restart()
	{
		score = 0;
		scoreLabel.text = "0";

		spawner.Reset ();
		player.Reset ();

		sceneManager.showGameplay ();

		this.Start ();
	}

	void Start()
	{
		LevelData thisLevel = LevelLoader.loadLevel ("Levels/level001");
		Debug.Log ("Successfully loaded level: " + thisLevel.title);

		spawner.addEnemies (thisLevel.events);
		spawner.shouldSpawn = true;
		scenery.isMoving = true;
	}
}
