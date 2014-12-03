using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public int damage;
	public Vector2 trajectory;
	public float timeToLive;
	public bool shotByPlayer = false;

	public static GameManager gameManager;


	// Use this for initialization
	void Start () {
		if(gameManager == null)
		{
			gameManager = GameObject.FindGameObjectWithTag ("GameRoot").GetComponent<GameManager> ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameManager.currentState != GameManager.GameState.Playing)
		{
			return;
		}

		transform.localPosition += new Vector3 (trajectory.x, trajectory.y, 0);

		// Cleanup if missed
		timeToLive -= Time.deltaTime;

		if(timeToLive <= 0)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
