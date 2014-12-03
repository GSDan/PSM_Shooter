using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public UILabel scoreLabel;
	public EnemySpawner spawner;
	public SceneManager sceneManager;
	public SceneryManager scenery;
	public PlayerController player;

	public GameObject pauseMenu;

	int score = 0;

	public enum GameState{Playing, Paused, GameOver};

	public GameState currentState;

	void Start()
	{
		LevelData thisLevel = LevelLoader.loadLevel ("Levels/level001");
		Debug.Log ("Successfully loaded level: " + thisLevel.title);
		
		spawner.addEnemies (thisLevel.events);
		spawner.shouldSpawn = true;
		scenery.isMoving = true;
		currentState = GameState.Playing;
	}

	public void PauseControl()
	{
		if(currentState == GameState.Playing)
		{
			// Do pause
			currentState = GameState.Paused;
			spawner.shouldSpawn = false;
			scenery.isMoving = false;
			pauseMenu.SetActive(true);
		}
		else if(currentState == GameState.Paused)
		{
			// Do unpause
			currentState = GameState.Playing;
			spawner.shouldSpawn = true;
			scenery.isMoving = true;
			pauseMenu.SetActive(false);
		}
	}

	public void AddPoints(int points)
	{
		score += points;
		scoreLabel.text = score.ToString ();
	}

	public void GameOver()
	{
		currentState = GameState.GameOver;
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
		DestroyShots ();

		score = 0;
		scoreLabel.text = "0";

		spawner.Reset ();
		player.Reset ();

		// If the gameover screen is currently showing, hide it
		if(currentState == GameState.GameOver)
		{
			sceneManager.showGameplay ();
		}
		else if(currentState == GameState.Paused)
		{
			PauseControl();
		}

		this.Start ();
	}

	void DestroyShots()
	{
		Shot[] shots = GetComponentsInChildren<Shot> ();

		foreach(Shot s in shots)
		{
			GameObject.Destroy(s.gameObject);
		}
	}

}
