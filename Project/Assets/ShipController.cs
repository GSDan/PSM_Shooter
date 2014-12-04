using UnityEngine;
using System.Collections;

public abstract class ShipController : MonoBehaviour {

	// Generic ship class inherited by player and enemies alike

	public Vector2 shipSpeed;

	public float maxHealth;
	protected float currentHealth;

	public float shotCooldown;
	protected float timeSincelastShot = 0;

	public float shotVelocity = 5;

	public GameObject shotPrefab;

	public static GameManager gameManager;

	protected UISprite sprite;

	public float getHealthPercent()
	{
		return currentHealth / maxHealth;
	}

	protected virtual void Start () 
	{
		currentHealth = maxHealth;

		sprite = gameObject.GetComponent<UISprite> ();

		if(gameManager == null)
		{
			gameManager = GameObject.FindGameObjectWithTag ("GameRoot").GetComponent<GameManager> ();
		}
	}

	protected virtual Shot shoot()
	{
		GameObject thisShot = (GameObject) Instantiate(shotPrefab, transform.position, transform.rotation);
		thisShot.transform.parent = gameManager.transform;
		thisShot.transform.localScale = Vector3.one;
		timeSincelastShot = 0;
		return thisShot.GetComponent<Shot> ();
	}

	protected virtual Shot shoot(Vector2 direction)
	{
		Shot thisShot = shoot();
		thisShot.trajectory = direction;
		return thisShot;
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		if(gameManager.currentState != GameManager.GameState.Playing)
		{
			return;
		}

		if(currentHealth <= 0)
		{
			GameObject.Destroy(gameObject);
		}
		else
		{
			// Ship firing
			timeSincelastShot += Time.deltaTime;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Shot")
		{
			Shot thisShot = other.gameObject.GetComponent<Shot>();
			OnHit(thisShot);
		}
	}

	protected abstract void OnHit (Shot thisShot);

	public virtual void Damage(int amount)
	{
		currentHealth = Mathf.Max (0, currentHealth - amount);
	}


	public virtual void Heal(int amount)
	{
		currentHealth = Mathf.Min (maxHealth, currentHealth + amount);
		Debug.Log ("healed to " + currentHealth);
	}

}
