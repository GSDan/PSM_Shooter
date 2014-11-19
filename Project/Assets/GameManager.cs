using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public UILabel scoreLabel;

	int score = 0;

	public void AddPoints(int points)
	{
		score += points;
		scoreLabel.text = score.ToString ();
	}
}
