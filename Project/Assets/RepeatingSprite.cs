using UnityEngine;
using System.Collections;

public class RepeatingSprite : MonoBehaviour {

	public Transform partnerPosition;
	public UISprite thisSprite;

	private Vector3 originalPosition;

	void Start()
	{
		originalPosition = transform.localPosition;
	}

	public void ResetPosition()
	{
		transform.localPosition = originalPosition;
	}

	// Transport this background sprite to tile after the one following it once it is off camera
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Respawn")
		{
			transform.localPosition = partnerPosition.localPosition + new Vector3 (thisSprite.width - 1, 0, 0);
		}
	}
}
