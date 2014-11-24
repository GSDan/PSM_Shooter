using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public Transform gameplayRoot;
	public Vector3 gameplayHidden;
	private Vector3 gameplayShown;

	public Transform metaRoot;
	public Vector3 metaShown;
	private Vector3 metaHidden;

	public Transform sky;
	public Vector3 skyMeta;
	private Vector3 skyGameplay;

	public float speed;
	public float animCutOff = 2;

	public bool isReady = true;

	IEnumerator moveTo(Vector3 gameplayFinal, Vector3 metaFinal, Vector3 skyFinal)
	{
		isReady = false;

		float currentTime = 0;

		while(currentTime < animCutOff)
		{
			gameplayRoot.localPosition = Vector3.Lerp(gameplayRoot.localPosition, gameplayFinal, speed * Time.deltaTime);
			metaRoot.localPosition = Vector3.Lerp(metaRoot.localPosition, metaFinal, speed * Time.deltaTime);
			sky.localPosition = Vector3.Lerp(sky.localPosition, skyFinal, speed * Time.deltaTime);

			currentTime += Time.deltaTime;

			yield return null;
		}

		gameplayRoot.localPosition = gameplayFinal;
		metaRoot.localPosition = metaFinal;
		sky.localPosition = skyFinal;

		isReady = true;
		yield return null;
	}

	void Start()
	{
		gameplayShown = gameplayRoot.localPosition;
		metaHidden = metaRoot.localPosition;
		skyGameplay = sky.localPosition;
	}

	public void showGameOver()
	{
		gameplayRoot.localPosition = gameplayShown;
		metaRoot.localPosition = metaHidden;
		sky.localPosition = skyGameplay;

		StartCoroutine (moveTo(gameplayHidden, metaShown, skyMeta));
	}

	public void showGameplay()
	{
		if(!isReady) return;

		gameplayRoot.localPosition = gameplayHidden;
		metaRoot.localPosition = metaShown;
		sky.localPosition = skyMeta;
		
		StartCoroutine (moveTo(gameplayShown, metaHidden, skyGameplay));
		gameplayRoot.gameObject.SetActive(true);
	}
}
