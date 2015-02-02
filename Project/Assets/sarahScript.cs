using UnityEngine;
using System.Collections;

public class sarahScript : MonoBehaviour {

	int health;
	bool isAlive;

	// Use this for initialization
	void Start () {
		health = 100;
		isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {

		health = health - 1;

		if (health == 0) {
			killCharacter();	
		}
	
	
	}

	void killCharacter()
	{

	}
}
