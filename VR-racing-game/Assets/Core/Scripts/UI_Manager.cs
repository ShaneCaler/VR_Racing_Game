using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	public Text speedText;

	public virtual void UpdateSpeedText(float speed)
	{
		//convert to mph
		float newSpeed = speed * 2.23694f;
		speedText.text = Mathf.Clamp(Mathf.Round(newSpeed), 0f, 1000f) + " MPH";
	}
}
