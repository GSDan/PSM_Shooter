using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public int damage;
	public Vector2 trajectory;
	public float timeToLive;
	public bool shotByPlayer = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localPosition += new Vector3 (trajectory.x, trajectory.y, 0);

		// Cleanup if missed
		timeToLive -= Time.deltaTime;

		if(timeToLive <= 0)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
