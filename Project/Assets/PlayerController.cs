using UnityEngine;
using System.Collections;

public class PlayerController : ShipController {

	public bool isAlive = true;
	private Vector3 originalPos;

	public enum PlayerMode {Flight, Ground};
	public PlayerMode currentMode = PlayerMode.Flight;

	public float streetlevel = -137;
	public float ceilingLevel = 161;

	public float takeOffTargetY = -50;
	public float takeOffSpeed = 50;
	public float jumpPower = 10;
	public float maxJumpStamina = 1;
	private float currentJumpStamina;
	private bool inTakeOff = false;

	// Use this for initialization
	protected override void Start ()
	{
		currentJumpStamina = maxJumpStamina;
		originalPos = transform.localPosition;
		base.Start ();
	}

	public void Reset()
	{
		transform.localRotation = new Quaternion (0, 0, 0, 0);
		transform.localPosition = originalPos;
		currentHealth = maxHealth;
		isAlive = true;
	}

	public float getStaminaPercent()
	{
		return currentJumpStamina / maxJumpStamina;
	}

	public void SwitchMode()
	{
		PlayerMode previousMode = currentMode;

		// Iterate through modes
		currentMode += 1;
		if( (int)currentMode >= (int)System.Enum.GetValues(typeof(PlayerMode)).Length)
		{
			currentMode = 0;
		}

		if(currentMode == previousMode) return;

		if (currentMode == PlayerMode.Flight)
		{
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			StartCoroutine(TakeFlight());
			sprite.spriteName = "ship";
			return;
		}

		if (currentMode == PlayerMode.Ground)
		{
			inTakeOff = false;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			sprite.spriteName = "Mario_Sprite";
			return;
		}
	}

	IEnumerator TakeFlight()
	{
		inTakeOff = true;
		yield return null;

		Debug.Log ("Start y = " + transform.localPosition.y);

		Vector3 newPos = transform.localPosition;		
		newPos.y = Mathf.Max(streetlevel, newPos.y);
		transform.localPosition = newPos;

		while(transform.localPosition.y < takeOffTargetY && inTakeOff)
		{
			newPos = transform.localPosition;
			newPos.y += takeOffSpeed * Time.deltaTime;

			transform.localPosition = newPos;
			yield return null;
		}
		inTakeOff = false;
	}

	// Update is called once per frame
	protected override void Update ()
	{
		if(gameManager.currentState != GameManager.GameState.Playing)
		{
			return;
		}
		else if(currentHealth <= 0)
		{
			if(isAlive)
			{
				isAlive = false;
				Debug.Log("Life lost!");
				gameManager.LoseLife();
			}
			return;
		}
		else
		{
			base.Update ();
		}

		if(currentJumpStamina < maxJumpStamina)
		{
			currentJumpStamina = Mathf.Min(currentJumpStamina + Time.deltaTime/10, maxJumpStamina);
		}


		//Check for start button being pressed
		if(Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Escape))
		{
			gameManager.PauseControl();
		}

		// Check for left shoulder (transform)
		if(Input.GetButtonDown("Left Shoulder") || Input.GetKeyDown(KeyCode.Q))
		{
			SwitchMode();
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

		if((Input.GetKey(KeyCode.Mouse0) || Input.GetAxis("Right Stick Horizontal") != 0) && timeSincelastShot >= shotCooldown)
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

		// Hard lock to the ground as a base minimum height
		if(transform.localPosition.y < streetlevel)
		{
			Vector3 newPos = transform.localPosition;
			newPos.y = streetlevel;
			transform.localPosition = newPos;
			rigidbody.velocity = Vector3.zero;
		}

		// Hard lock to the ceiling as a base max height
		if(transform.localPosition.y > ceilingLevel)
		{
			Vector3 newPos = transform.localPosition;
			newPos.y = ceilingLevel;
			transform.localPosition = newPos;
			rigidbody.velocity = Vector3.zero;
		}

		if((Input.GetButton("Cross") || Input.GetKey(KeyCode.Space)) && currentMode == PlayerMode.Ground  && transform.localPosition.y < ceilingLevel && currentJumpStamina > maxJumpStamina/20)
		{
			Debug.Log(rigidbody.velocity.ToString());
			currentJumpStamina -= Time.deltaTime;
			rigidbody.AddForce(Vector3.up * jumpPower);
		}

		if(inTakeOff || currentMode == PlayerMode.Ground) return; // Player has no direct control over vertical movement during takeoff or ground mode

		// Ship movement up
		if((Input.GetButton("Dup") || Input.GetAxis("Left Stick Vertical") < 0 || Input.GetKey(KeyCode.W)) && (transform.localPosition.y < ceilingLevel))
		{
			transform.localPosition += new Vector3(0, shipSpeed.y * Time.deltaTime, 0);
		}
		// ship movement down
		else if((Input.GetButton("Ddown") || Input.GetAxis("Left Stick Vertical") > 0 || Input.GetKey(KeyCode.S)) && (transform.localPosition.y > -120))
		{
			transform.localPosition += new Vector3(0, -shipSpeed.y * Time.deltaTime, 0);
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
