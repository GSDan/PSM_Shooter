using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public UILabel scoreLabel;

	public UILabel finalScoreLabel;
	public UILabel highScoreLabel;

	public EnemySpawner spawner;
	public SceneManager sceneManager;
	public SceneryManager scenery;
	public PlayerController player;
	public GameObject pauseMenu;

	public enum GameState{Playing, Paused, GameOver};
	public GameState currentState;

	DataManager dataManager;

	public int lives = 3;
	int currentLevel = 0;
	int score = 0;

	void Start()
	{
		dataManager = GameObject.Find ("Persistent").GetComponent<DataManager>();

		dataManager.LoadLevelList ();

		LevelSetup ();
	}

	void LevelSetup()
	{
		LevelData thisLevel = LevelLoader.loadLevel (dataManager.levelLocs[currentLevel]);
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

	public void LoseLife()
	{
		lives--;

		if(lives < 0)
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		bool newHigh = dataManager.data.EnterScoreIfGreater (currentLevel, score);
		dataManager.Save ();

		if(newHigh)
		{
			highScoreLabel.text = "NEW HIGH SCORE!";
		}
		else
		{
			highScoreLabel.text = "HIGHSCORE: " + dataManager.data.levelScores[currentLevel].ToString();
		}

		finalScoreLabel.text = "SCORE: " + scoreLabel.text;

		currentState = GameState.GameOver;
		scenery.isMoving = false;
		spawner.shouldSpawn = false;
		sceneManager.showGameOver ();
	}

	public void LevelComplete()
	{
		spawner.Reset ();

		if(currentLevel < dataManager.levelLocs.Count - 1)
		{
			currentLevel++;
		}
		else
		{
			Debug.Log("run out of levels! Restarting from 0");
			currentLevel = 0;
		}

		LevelSetup();
	}

	public void Restart()
	{
		DestroyShots ();

		score = 0;
		currentLevel = 0;
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

		LevelSetup();
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
