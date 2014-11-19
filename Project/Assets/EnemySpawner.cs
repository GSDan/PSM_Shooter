using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
		
	public float spawnEvery = 5;
	float currentTime = 0;

	public GameObject enemyPrefab;

	// Update is called once per frame
	void Update ()
	{
		currentTime += Time.deltaTime;

		if(currentTime >= spawnEvery)
		{
			GameObject enemy = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
			enemy.transform.parent = transform;
			enemy.transform.localScale = Vector3.one;

			currentTime = 0;
		}
	}
}
