using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CrumbleSpin : MonoBehaviour {

	[SerializeField]
	private float spinSpeed;
	[SerializeField]
	private LayerMask groundItems;
	[SerializeField]
	new AudioSource soundClip;
	[SerializeField]
	private AudioClip splatSound;

	public bool gameOver;

	private Vector3 hit;
	private Vector3 Pos;
	private bool hitFloor;

	// Use this for initialization
	void Start () {
		soundClip = (AudioSource)gameObject.AddComponent<AudioSource> ();

		GetComponent<Rigidbody2D> ().velocity = new Vector2(Random.Range(-10.0f, 10.0f),20.0f);
		GetComponent<Rigidbody2D> ().angularVelocity = spinSpeed;
	}

	void FixedUpdate(){
		hitFloor = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<BoxCollider2D> (), groundItems);;
	}

	// Update is called once per frame
	void Update () {
		if (hitFloor == true) {
			GetComponent<Rigidbody2D> ().isKinematic = true;
			GetComponent<Transform> ().localEulerAngles = new Vector3(0.0f,0.0f,180.0f);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.layer == 8) {
			gameOver = true;
			soundClip.PlayOneShot (splatSound, 0.7f);
			hit = col.GetComponentInParent<Transform> ().position;
			Pos = GetComponent<Transform> ().position;
			GetComponent<Rigidbody2D> ().isKinematic = true;
			GetComponent<Transform> ().localEulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
			GetComponent<Transform> ().position = new Vector3 (0, hit.y, 0) + new Vector3 (Pos.x, 0.5f, 0.0f);
			SceneManager.LoadScene("Game Over");
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector3 (0.0f, GetComponent<Rigidbody2D> ().velocity.y, 0.0f);
		}
	}
}
