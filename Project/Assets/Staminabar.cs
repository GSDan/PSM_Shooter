using UnityEngine;
using System.Collections;

public class Staminabar : MonoBehaviour {

	public PlayerController player;
	public UISlider slider;
	
	// Update is called once per frame
	void Update ()
	{
		slider.value = player.getStaminaPercent ();
	}
}
