using UnityEngine;
using System.Collections;

public class EnemyController : ShipController {

	public float timeToLive;
	public int pointsValue = 500;

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		timeToLive -= Time.deltaTime;

		if(timeToLive <= 0)
		{
			GameObject.Destroy(gameObject);
		}

		// You may fire when ready, Admiral
		if (timeSincelastShot >= shotCooldown) 
		{
			shoot(new Vector2(-shotVelocity, 0));
		}

		transform.localPosition += new Vector3 (shipSpeed.x * Time.deltaTime, shipSpeed.y * Time.deltaTime);
	}

	protected override void OnHit (Shot thisShot)
	{
		if(thisShot.shotByPlayer)
		{
			Damage(thisShot.damage);

			if(currentHealth <= 0)
			{
				GameObject.FindGameObjectWithTag("GameRoot").GetComponent<GameManager>().AddPoints(pointsValue);
			}

			GameObject.Destroy(thisShot.gameObject);
		}
	}


}
