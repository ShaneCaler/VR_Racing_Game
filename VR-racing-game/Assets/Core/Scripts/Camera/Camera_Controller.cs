using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CamMode
{
	FirstPerson,
	ThirdPerson
}

public class Camera_Controller : MonoBehaviour
{
	public GameObject focus;
	public float distance = 5f;
	public float height = 2f;
	public float dampening = 1f;
	public float h2 = 0f;
	public float d2 = 0f;
	public float len = 0f;

	private int numOfCamModes = System.Enum.GetValues(typeof(CamMode)).Length;
	private int prevCamMode;
	private OvrAvatar avatar;

	[HideInInspector] public CamMode currCamMode;

	void Start()
	{
		avatar = GetComponentInParent<OvrAvatar>();
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.C)) // OVRInput.GetDown(OVRInput.RawButton.B)
		{
			prevCamMode = (prevCamMode + 1) % numOfCamModes;
			currCamMode = (CamMode)prevCamMode;
		}

		switch (currCamMode)
		{
			case CamMode.FirstPerson:
				Debug.Log("Cam mode is now: " + currCamMode.ToString());
				avatar.ShowFirstPerson = true;
				avatar.ShowThirdPerson = false;
				transform.position = focus.transform.position +
					 focus.transform.TransformDirection(new Vector3(len, h2, d2));
				transform.rotation = focus.transform.rotation;
				break;
			case CamMode.ThirdPerson:
				Debug.Log("Cam mode is now: " + currCamMode.ToString());
				avatar.ShowFirstPerson = false;
				avatar.ShowThirdPerson = true;
				transform.position = Vector3.Lerp(transform.position, focus.transform.position +
												focus.transform.TransformDirection(new Vector3(0f, height, -distance)),
												dampening * Time.deltaTime);
				//StartCoroutine(WaitToLookAt());
				break;
			default:
				Debug.Log("Cam mode is now: " + currCamMode.ToString());
				transform.position = Vector3.Lerp(transform.position, focus.transform.position +
												focus.transform.TransformDirection(new Vector3(0f, height, -distance)), 
												dampening * Time.deltaTime);
				//StartCoroutine(WaitToLookAt());
				break;
		}
    }

	IEnumerator WaitToLookAt()
	{
		yield return new WaitForSeconds(2f);
		transform.LookAt(focus.transform);
	}
}
