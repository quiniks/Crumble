using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss : MonoBehaviour {

	[SerializeField]
	public GameObject leftPulleyObject;
	[SerializeField]
	public GameObject rightPulleyObject;
	[SerializeField]
	public GameObject player;
	[SerializeField]
	public float speed;
	[SerializeField]
	public LayerMask wallItems;
	[SerializeField]
	public LayerMask groundItems;
	[SerializeField]
	public LayerMask spikeItems;
	[SerializeField]
	public Sprite crate1;
	[SerializeField]
	public Sprite crate2;
	[SerializeField]
	public Sprite crate3;
	[SerializeField]
	new AudioSource soundClip;
	[SerializeField]
	public AudioClip smashSound;
	[SerializeField]
	public float timerStartTime;

	public Vector3 targetOffset;

	public int stage;
	public bool leftOrRight;

	public Transform leftBox;
	public Transform rightBox;
	public Vector3 leftBoxPos;
	public Vector3 rightBoxPos;

	public Vector3 leftPulleyVel;
	public Vector3 rightPulleyVel;
	public float leftBoxAngle;
	public float rightBoxAngle;
	public float elapsed;
	public bool isDoingActionLeft;
	public bool isDoingActionRight;
	public Vector3 difLeft;
	public Vector3 difRight;
	public float timerLeft;
	public float timerRight;
	public bool isTouchingEdgeLeft; 
	public bool isTouchingEdgeRight;
	public bool isTouchingSpikeLeft; 
	public bool isTouchingSpikeRight; 
	public int livesLeft;
	public int livesRight;
	public bool leftSpikeDebounce;
	public bool rightSpikeDebounce;
	public bool leftSmashDebounce;
	public bool rightSmashDebounce;
	public float meanLives;
	public bool leftActive;
	public bool rightActive;
	public bool attack;

	// Use this for initialization
	void Start () {		
		leftActive = true;
		rightActive = true;
		soundClip = (AudioSource)gameObject.AddComponent<AudioSource> ();
		stage = 0;
		leftOrRight = true;
		livesLeft = 3;
		livesRight = 3;
		leftSpikeDebounce = false;
		rightSpikeDebounce = false;
		leftSmashDebounce = false;
		rightSmashDebounce = false;
		timerLeft = timerStartTime;
		timerRight = timerStartTime;
		isDoingActionLeft = false;
		isDoingActionRight = false;
		leftBox = leftPulleyObject.transform.FindChild("crate_0");
		rightBox = rightPulleyObject.transform.FindChild("crate_0");
	}

	void FixedUpdate () {
		isTouchingEdgeLeft = leftBox.GetComponent<BoxCollider2D> ().IsTouchingLayers (groundItems + wallItems);
		isTouchingEdgeRight = rightBox.GetComponent<BoxCollider2D> ().IsTouchingLayers (groundItems + wallItems);
		isTouchingSpikeLeft = leftBox.GetComponent<BoxCollider2D> ().IsTouchingLayers (spikeItems);
		isTouchingSpikeRight = rightBox.GetComponent<BoxCollider2D> ().IsTouchingLayers (spikeItems);
	}

	// Update is called once per frame
	void Update () {
		if (livesLeft <= 0 && livesRight <= 0) {
			SceneManager.LoadScene("WIN");
		}
		attack = player.GetComponent<PlayerHit> ().hasCrumble;
		meanLives = (livesLeft + livesRight) / 2;
		hitEdge ();
		leftPulleyVel = leftPulleyObject.GetComponent<Rigidbody2D> ().velocity;
		rightPulleyVel = rightPulleyObject.GetComponent<Rigidbody2D> ().velocity;
		leftBoxPos = leftBox.transform.position;
		rightBoxPos = rightBox.transform.position;
		elapsed = Time.deltaTime;
		difLeft = new Vector2 (player.transform.position.x + targetOffset.x - leftBoxPos.x, player.transform.position.y + targetOffset.y - leftBoxPos.y);
		difRight = new Vector2 (player.transform.position.x + targetOffset.x - rightBoxPos.x, player.transform.position.y + targetOffset.y - rightBoxPos.y);
		attackTimer ();
		AttackLogic (elapsed);
		if (livesLeft == 0) {
			leftActive = false;
		}
		if (livesRight == 0) {
			rightActive = false;
		}
		Debug.Log("L lives: " + livesLeft + ", R lives: " + livesRight + ", M lives: " + meanLives + "offset: " + targetOffset);
	}

	void AttackLogic(float elapsed) {
		if (attack) {
			if (meanLives > 2) {
				if (stage == 0) {
					targetOffset = new Vector2 (0.0f, 5.0f);
					if (isDoingActionLeft == false) {
						if (AttackPatternFollowLeft () == false) {
							isDoingActionLeft = true;
							targetOffset = new Vector2 (0.0f, 0.0f);
							difLeft = new Vector2 (player.transform.position.x + targetOffset.x - leftBoxPos.x, player.transform.position.y + targetOffset.y - leftBoxPos.y);
							AttackPatternSwingLeft ();
							stage = 1;
						}
					}
				} else if (stage == 1) {
					targetOffset = new Vector2 (0.0f, 5.0f);
					if (isDoingActionRight == false) {
						if (AttackPatternFollowRight () == false) {
							isDoingActionRight = true;
							targetOffset = new Vector2 (0.0f, 0.0f);
							difRight = new Vector2 (player.transform.position.x + targetOffset.x - rightBoxPos.x, player.transform.position.y + targetOffset.y - rightBoxPos.y);
							AttackPatternSwingRight ();
							stage = 0;
						}
					}
				}
			} else if (meanLives > 1 && meanLives <= 2) {
				if (stage == 0) {
					targetOffset = new Vector2 (5.0f, 0.0f);
					if (isDoingActionLeft == false) {
						if (AttackPatternFollowLeft () == false) {
							isDoingActionLeft = true;
							targetOffset = new Vector2 (0.0f, 0.0f);
							difLeft = new Vector2 (player.transform.position.x + targetOffset.x - leftBoxPos.x, player.transform.position.y + targetOffset.y - leftBoxPos.y);
							AttackPatternSwingLeft ();
							stage = 1;
						}
					}
				} else if (stage == 1) {
					targetOffset = new Vector2 (-5.0f, 0.0f);
					if (isDoingActionRight == false) {
						if (AttackPatternFollowRight () == false) {
							isDoingActionRight = true;
							targetOffset = new Vector2 (0.0f, 0.0f);
							difRight = new Vector2 (player.transform.position.x + targetOffset.x - rightBoxPos.x, player.transform.position.y + targetOffset.y - rightBoxPos.y);
							AttackPatternSwingRight ();
							stage = 0;
						}
					}
				}
			} else if (meanLives == 0 && meanLives <= 1) {
				if (stage == 0) {
					if (isDoingActionLeft == false) {
						isDoingActionLeft = true;
						targetOffset = new Vector2 (0.0f, 0.0f);
						difLeft = new Vector2 (player.transform.position.x + targetOffset.x - leftBoxPos.x, player.transform.position.y + targetOffset.y - leftBoxPos.y);
						AttackPatternSwingLeft ();
						stage = 1;
					}
				} else if (stage == 1) {
					if (isDoingActionRight == false) {
						isDoingActionRight = true;
						targetOffset = new Vector2 (0.0f, 0.0f);
						difRight = new Vector2 (player.transform.position.x + targetOffset.x - rightBoxPos.x, player.transform.position.y + targetOffset.y - rightBoxPos.y);
						AttackPatternSwingRight ();
						stage = 0;
					}
				}
			}
		} else {
			targetOffset = new Vector2 (0.0f, 5.0f);
			AttackPatternFollowLeft ();
			AttackPatternFollowRight ();
		}
	}

	bool AttackPatternFollowLeft(){
		if ((Mathf.Abs (difLeft.y) > 1.0f || Mathf.Abs (difLeft.x) > 1.0f) && leftActive == true) {
			//leftBoxAngle = Mathf.Atan2 (targetOffset.y - leftBoxPos.y, targetOffset.x - leftBoxPos.x);
			//leftPulleyObject.GetComponent<Transform> ().position += new Vector3 (Mathf.Cos (leftBoxAngle) * speedX * , Mathf.Sin (leftBoxAngle) * speedY * , 0.0f);
			leftPulleyObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(difLeft.x * speed, difLeft.y * speed);
			return true;
		} else {
			return false;
		}
	}

	bool AttackPatternFollowRight(){
		if ((Mathf.Abs (difRight.y) > 1.0f || Mathf.Abs (difRight.x) > 1.0f) && rightActive == true) {
			//rightBoxAngle = Mathf.Atan2 (targetOffset.y - rightBoxPos.y, targetOffset.x - rightBoxPos.x);
			//rightPulleyObject.GetComponent<Transform> ().position += new Vector3 (Mathf.Cos (rightBoxAngle) * speedX * , Mathf.Sin (rightBoxAngle) * speedY * , 0.0f);
			rightPulleyObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(difRight.x * speed, difRight.y * speed);
			return true;
		} else {
			return false;
		}
	}

	void AttackPatternSwingLeft(){
		if (leftActive) {
			leftPulleyObject.GetComponent<Rigidbody2D> ().velocity = difLeft;
		}
	}
	void AttackPatternSwingRight(){
		if (rightActive) {
			rightPulleyObject.GetComponent<Rigidbody2D> ().velocity = difRight;
		}
	}

	void attackTimer()
	{
		if (isDoingActionLeft) {
			timerLeft -= elapsed;
		} else {
			timerLeft = timerStartTime;
		}
		if (isDoingActionRight) {
			timerRight -= elapsed;
		} else {
			timerRight = timerStartTime;
		}
		if (timerLeft < 0 && isDoingActionLeft) {
			isDoingActionLeft = false;
			timerLeft = timerStartTime;
		}
		if (timerRight < 0 && isDoingActionRight) {
			isDoingActionRight = false;
			timerRight = timerStartTime;
		}
	}

	void hitEdge(){
		if (isTouchingSpikeLeft && leftSpikeDebounce == false) {
			leftSpikeDebounce = true;
			livesLeft--;
		}
		if (isTouchingSpikeRight && rightSpikeDebounce == false) {
			rightSpikeDebounce = true;
			livesRight--;
		}
		if (!isTouchingSpikeLeft) {
			leftSpikeDebounce = false;
		}
		if (!isTouchingSpikeRight) {
			rightSpikeDebounce = false;
		}
		if (isTouchingEdgeLeft && leftSmashDebounce == false) {
			leftSmashDebounce = true;
			soundClip.PlayOneShot (smashSound, 0.7f);
			leftPulleyObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			isDoingActionLeft = false;
		}
		if (isTouchingEdgeRight && rightSmashDebounce == false) {
			rightSmashDebounce = true;
			soundClip.PlayOneShot (smashSound, 0.7f);
			rightPulleyObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			isDoingActionRight = false;
		}
		if (!isTouchingEdgeLeft) {
			leftSmashDebounce = false;
		}
		if (!isTouchingEdgeRight) {
			rightSmashDebounce = false;
		}
		if (livesLeft == 2) {
			leftBox.GetComponent<SpriteRenderer> ().sprite = crate1;
		} else if (livesLeft == 1) {
			leftBox.GetComponent<SpriteRenderer> ().sprite = crate2;
		} else if (livesLeft == 0) {
			leftBox.GetComponent<SpriteRenderer> ().sprite = crate3;
		}

		if (livesRight == 2) {
			rightBox.GetComponent<SpriteRenderer> ().sprite = crate1;
		} else if (livesRight == 1) {
			rightBox.GetComponent<SpriteRenderer> ().sprite = crate2;
		} else if (livesRight == 0) {
			rightBox.GetComponent<SpriteRenderer> ().sprite = crate3;
		}
	}
}