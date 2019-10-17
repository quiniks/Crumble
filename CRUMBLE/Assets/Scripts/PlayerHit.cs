using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {

	[SerializeField]
	private LayerMask enemies;
	[SerializeField]
	private LayerMask crumble;
	[SerializeField]
	private LayerMask boss;
	[SerializeField]
	private float bouncePower;
	[SerializeField]
	private GameObject crumbleClone;
	[SerializeField]
	private float inviTime;
	[SerializeField]
	private float flashSpeed;
	[SerializeField]
	private float stunTime;

	private bool isHit;
	private bool getCrumble;
	private bool hitDebounce;
	private float timeCount;
	private bool flashing;
	private float canFlash;
	public bool hasCrumble;
	public bool stunned;
	public Vector2 flingVector;


	// Use this for initialization
	void Start () {
		canFlash = 0.0f;
		flashing = false;
		isHit = false;
		hasCrumble = true;
		stunned = false;
	}

	void FixedUpdate ()	{
		isHit = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<BoxCollider2D> (), enemies + boss);
		getCrumble = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<BoxCollider2D> (), crumble);
	}

	// Update is called once per frame
	void Update () {
		if (isHit == true && hitDebounce == false) {
			stunned = true;
			if (hasCrumble == true) {
				throwCrumble ();
				hasCrumble = false;
			}
			Debug.Log ("hit");
			hitDebounce = true;
			//GetComponent<Rigidbody2D> ().velocity = new Vector2(GetComponent<Rigidbody2D> ().velocity.normalized.x * -1 * bouncePower, bouncePower);
		}
		if (getCrumble == true) {
			if (GameObject.Find("crumble(Clone)").GetComponent<CrumbleSpin>().gameOver == false) {
				Debug.Log ("has crumble");
				Destroy (GameObject.Find ("crumble(Clone)"));
				hasCrumble = true;
			}
		}
		if (hitDebounce == true) {
			if (canFlash > 0.5f) {
				GetComponent<SpriteRenderer> ().color = new Color(1.0f,1.0f,1.0f,0.5f);
			} else {
				GetComponent<SpriteRenderer> ().color = Color.white;
			}
			timeCount = timeCount + Time.deltaTime;
			canFlash = (timeCount*flashSpeed)  % 2.0f;
		}
		if (timeCount > inviTime) {
			GetComponent<SpriteRenderer> ().color = Color.white;
			hitDebounce = false;
			timeCount = 0.0f;
		}
		if (timeCount > stunTime) {
			stunned = false;
		}
	}

	void throwCrumble()
	{
		Instantiate (crumbleClone, new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + 2), Quaternion.identity);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.layer == 12 /*&& col.gameObject.layer == 9*/) {
			flingVector = new Vector2 (col.GetComponentInParent<Rigidbody2D> ().velocity.x, col.GetComponentInParent<Rigidbody2D> ().velocity.y);
		} else if (col.gameObject.layer == 9) {
			flingVector = new Vector2 (Random.Range(-2,2), 2);
		}
	}
}