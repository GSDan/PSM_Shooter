using UnityEngine;
using System.Collections;

public class PlayerController : ShipController {

	public GameManager gameManager;

	private bool isAlive = true;
	private Vector3 originalPos;

	public void Reset()
	{
		transform.localRotation = new Quaternion (0, 0, 0, 0);
		transform.localPosition = originalPos;
		currentHealth = maxHealth;
		isAlive = true;
	}

	// Use this for initialization
	protected override void Start ()
	{
		originalPos = transform.localPosition;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		if(currentHealth <= 0)
		{
			if(isAlive)
			{
				gameManager.GameOver();
				isAlive = false;
			}
			return;
		}
		else
		{
			base.Update ();
		}

		// Ship movement up
		if((Input.GetButton("Dup") || Input.GetAxis("Left Stick Vertical") < 0 || Input.GetKey(KeyCode.W)) && (transform.localPosition.y < 161))
		{
			transform.localPosition += new Vector3(0, shipSpeed.y * Time.deltaTime, 0);
		}
		// ship movement down
		else if((Input.GetButton("Ddown") || Input.GetAxis("Left Stick Vertical") > 0 || Input.GetKey(KeyCode.S)) && (transform.localPosition.y > -120))
		{
			transform.localPosition += new Vector3(0, -shipSpeed.y * Time.deltaTime, 0);
		}
		
		// Movement right
		if((Input.GetButton("Dright") || Input.GetAxis("Left Stick Horizontal") > 0 || Input.GetKey(KeyCode.D)) && (transform.localPosition.x < -40))
		{
			transform.localPosition += new Vector3(shipSpeed.x * Time.deltaTime, 0, 0);
		}
		// movement left
		else if((Input.GetButton("Dleft") || Input.GetAxis("Left Stick Horizontal") < 0 || Input.GetKey(KeyCode.A)) && (transform.localPosition.x > -290))
		{
			transform.localPosition += new Vector3(-shipSpeed.x * Time.deltaTime, 0, 0);
		}

		if((Input.GetKey(KeyCode.Space) || Input.GetAxis("Right Stick Horizontal") != 0) && timeSincelastShot >= shotCooldown)
		{
			if(Input.GetAxis("Right Stick Horizontal") > 0)
			{
				shoot(new Vector2(shotVelocity, Input.GetAxis ("Right Stick Vertical") * -2));
			}
			else
			{
				shoot(new Vector2(-shotVelocity, Input.GetAxis ("Right Stick Vertical") * -2));
			}
		}
	}

	protected override Shot shoot(Vector2 direction)
	{
		Shot thisShot = base.shoot (direction);

		thisShot.shotByPlayer = true;
		return thisShot;
	}

	protected override void OnHit (Shot thisShot)
	{
		if(!thisShot.shotByPlayer)
		{
			currentHealth -= thisShot.damage;
			GameObject.Destroy(thisShot.gameObject);
		}
	}
}
