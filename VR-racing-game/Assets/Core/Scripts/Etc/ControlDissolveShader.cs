using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A temporary script used to understand how setting shader variables works 

public class ControlDissolveShader : MonoBehaviour
{
	public Material dissolveMaterial;
	public float stepSpeed;
	public float currStep = 0f;

	void Start()
	{
		dissolveMaterial.SetFloat("Vector1_5FD9AA31", 0);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(gameObject.ToString() + " collided with " + other.ToString() + " with tag of " + other.tag.ToString());
		if (other.CompareTag("Player"))
		{
			StartCoroutine(Dissolve());
		}
	}

	IEnumerator Dissolve()
	{
		for (currStep = 0f; currStep <= stepSpeed; currStep += Time.deltaTime * stepSpeed)
		{
			dissolveMaterial.SetFloat("Vector1_5FD9AA31", currStep);
			print("Ding");
			yield return new WaitForSeconds(.01f);
		}
	}

}
