using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	private GameObject target;
	[SerializeField]
	private float smoothingPrecision;
	[SerializeField]
	private float smoothingMax;
	[SerializeField]
	private bool verticalSmoothing;
	[SerializeField]
	private bool horizontalSmoothing;
	[SerializeField]
	private Vector3 cameraOffset;

	private string smoothingString;
	private int smoothingDigitLength;
	private float multiplier;
	private Vector3 playerVel;
	private float smoothCountX;
	private float smoothCountY;

//private float angle;
//private float vX;
//private float vY;
//private float cameraSpeed = 2.5f;
//private float targetOffsetX;
//private float targetOffsetY;
	// Use this for initialization
	void Start () {
		GetComponent<Transform> ().position = cameraOffset;
		smoothCountX = 0.0f;;
		smoothCountY = 0.0f;
		smoothingString = smoothingPrecision.ToString ();
		smoothingDigitLength = smoothingString.Length - 2;
		multiplier = Mathf.Pow (10.0f, smoothingDigitLength);
	}

	void FixedUpdate () {

	}

	// Update is called once per frame
	void Update () {
		
		smoothCountX = Mathf.Round (smoothCountX * multiplier) / multiplier;
		smoothCountY = Mathf.Round (smoothCountY * multiplier) / multiplier;
		playerVel = new Vector3 (
			target.GetComponent<Rigidbody2D> ().velocity.x / 5,
			target.GetComponent<Rigidbody2D> ().velocity.y / 5,
			0
		);
		if (horizontalSmoothing == true) {
			//Debug.Log("horiz");
			if (playerVel.x >= 0.05f && smoothCountX < smoothingMax) {
				//Debug.Log("1");
				smoothCountX += smoothingPrecision;
			} else if (playerVel.x <= -0.05f && smoothCountX > -smoothingMax) {
				//Debug.Log("2");
				smoothCountX -= smoothingPrecision;
			} else if (smoothCountX < 0.0f && playerVel.x > -0.05f && playerVel.x < 0.05f) {
				//Debug.Log("3");
				smoothCountX += smoothingPrecision;
			} else if (smoothCountX > 0.0f && playerVel.x > -0.05f && playerVel.x < 0.05f) {
				//Debug.Log("4");
				smoothCountX -= smoothingPrecision;
			}
		}
		if (verticalSmoothing == true) {
			//Debug.Log("verti");
			if (playerVel.y >= 0.05f && smoothCountY < smoothingMax) {
				smoothCountY += smoothingPrecision;
			} else if (playerVel.y <= -0.05f && smoothCountY > -smoothingMax) {
				smoothCountY -= smoothingPrecision;
			} else if (smoothCountY < 0.0f && playerVel.y > -0.05f && playerVel.y < 0.05f) {
				smoothCountY += smoothingPrecision;
			} else if (smoothCountY > 0.0f && playerVel.y < -0.05f && playerVel.y < 0.05f) {
				smoothCountY -= smoothingPrecision;
			}
		}
		//Debug.Log ("smoothCountY = " + smoothCountY+" | VelY = "+playerVel.y);
		//Debug.Log ("smoothCountX = " + smoothCountX+" | VelX = "+playerVel.x);
		GetComponent<Transform> ().position = target.GetComponent<Transform> ().position + cameraOffset + new Vector3(smoothCountX, smoothCountY, 0)/*playerVel*/;
	}
}