using UnityEngine;
using System.Collections;

public class SceneryManager : MonoBehaviour {

	public RepeatingSprite farBackground;
	public RepeatingSprite farBackground2;
	public float farSpeed;

	public RepeatingSprite closeBackground;
	public RepeatingSprite closeBackground2;
	public float closeSpeed;

	public RepeatingSprite playerLayer;
	public RepeatingSprite playerLayer2;
	public float playerSpeed;

	public bool isMoving = true;

	public void resetScenery()
	{
		if(farBackground != null) farBackground.ResetPosition ();
		if(farBackground2 != null) farBackground2.ResetPosition ();
		if(closeBackground != null) closeBackground.ResetPosition ();
		if(closeBackground2 != null) closeBackground2.ResetPosition ();
		if(playerLayer != null) playerLayer.ResetPosition ();
		if(playerLayer2 != null) playerLayer2.ResetPosition ();
	}

	public void updateSpeeds(float farSpeed, float closeSpeed, float playerSpeed )
	{
		this.farSpeed = farSpeed;
		this.closeSpeed = closeSpeed;
		this.playerSpeed = playerSpeed;
	}

	// Update is called once per frame
	void Update ()
	{
		if(!isMoving) return;

		if(farBackground != null)	farBackground.transform.localPosition += new Vector3 (farSpeed * Time.deltaTime, 0, 0);
		if(farBackground2 != null)	farBackground2.transform.localPosition += new Vector3 (farSpeed * Time.deltaTime, 0, 0);

		if(closeBackground != null) closeBackground.transform.localPosition += new Vector3 (closeSpeed * Time.deltaTime, 0, 0);
		if(closeBackground2 != null)closeBackground2.transform.localPosition += new Vector3 (closeSpeed * Time.deltaTime, 0, 0);

		if(playerLayer != null)		playerLayer.transform.localPosition += new Vector3 (playerSpeed * Time.deltaTime, 0, 0);
		if(playerLayer2 != null)	playerLayer2.transform.localPosition += new Vector3 (playerSpeed * Time.deltaTime, 0, 0);
	}
}
