using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private float playerMoveSpeed;
	[SerializeField]
	private LayerMask groundItems;
	[SerializeField]
	private LayerMask wallItems;
	[SerializeField]
	private LayerMask ledgeItems;
	[SerializeField]
	private float jumpSpeed;
	[SerializeField]
	private float maxJumpLimiter;
	[SerializeField]
	private bool facingRight = false;
	[SerializeField]
	new AudioSource soundClip;
	[SerializeField]
	private AudioClip jumpSound;
	[SerializeField]
	private float fallLimit;


	public bool hasCrumble;
	public bool isStunned;
	public Vector2 flingVector;

	private bool isGrounded;
	private bool isTouchingEdge;
	private Vector2 playerVelocity;
	private float jumpHeight;
	private float initialJumpHeight;
	private Vector3 playerPos;

	private Animator playerAnimator;
	private float movementSpeed = 0;
	private bool isJumping = false;
	private bool canJump;

	private Vector3 currentScale;


	// Use this for initialization
	void Start () {
		soundClip = (AudioSource)gameObject.AddComponent<AudioSource> ();
		playerAnimator = GetComponent<Animator> ();
		currentScale = transform.localScale;
	}

	void FixedUpdate () {
		isGrounded = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<EdgeCollider2D> (), groundItems + ledgeItems);
		isTouchingEdge = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<BoxCollider2D> (), groundItems + wallItems);
		//isJumping = !isGrounded;
		playerAnimator.SetBool ("isJumping", !isGrounded);
		//Debug.Log ("player is grounded = " + isGrounded);
	}

	// Update is called once per frame
	void Update () {
		isPlayerDead ();
		hasCrumble = GetComponent<PlayerHit> ().hasCrumble;
		isStunned = GetComponent<PlayerHit> ().stunned;
		flingVector = GetComponent<PlayerHit> ().flingVector;
		if (hasCrumble == true) {
			playerAnimator.SetBool ("hasCrumble", true);
		}
			else
		{
			playerAnimator.SetBool ("hasCrumble", false);
		}
		if (isGrounded == true) {
			canJump = true;
		} else {
			canJump = false;
		}
		playerVelocity = GetComponent<Rigidbody2D> ().velocity;
		playerPos = GetComponent<Transform> ().position;
		if (Input.GetButtonUp ("Jump")) {
			isJumping = false;
			canJump = false;
			//jumpButtonDown = false;
		} 
		if (isStunned) {
			GetComponent<Rigidbody2D> ().velocity = flingVector;
		} else {
			if (Input.GetButton ("Jump")) {
				if (isJumping == false && canJump == true) {
					//Debug.Log ("1");
					initialJumpHeight = playerPos.y;
					playerJump (new Vector3 (0, jumpSpeed, 0));
					isJumping = true;
					soundClip.PlayOneShot (jumpSound, 0.7f);
				} else if (isJumping == true) {
					//Debug.Log ("2");
					jumpHeight = playerPos.y - initialJumpHeight;
					if (jumpHeight < maxJumpLimiter) {
						playerJump (new Vector2 (0, jumpSpeed));
					} else {
						isJumping = false;
					}
				}
			}
			if (Input.GetAxis ("Vertical") < 0 && GetComponent<Rigidbody2D> ().velocity.y > -7.0f) {
				GetComponent<Rigidbody2D> ().velocity = GetComponent<Rigidbody2D> ().velocity + new Vector2 (0.0f, -0.2f);
			}
			/*if (isGrounded) {
			isJumping = false;
			canJump = true;
		}*/
			if (Input.GetAxis ("Horizontal") != 0 && isTouchingEdge == false) {
				playerMove ();
			} else {
				playerAnimator.SetFloat ("Speed", 0);
			}
			if (Input.GetAxis ("Horizontal") > 0 && !facingRight) {
				flipHorizontal ();
			} else if (Input.GetAxis ("Horizontal") < 0 && facingRight) {
				flipHorizontal ();
			}
		}
	}

	private void playerJump(Vector2 vel){
		GetComponent<Rigidbody2D> ().velocity = GetComponent<Rigidbody2D> ().velocity + vel;
	}
	private void playerMove(){
		playerVelocity = GetComponent<Rigidbody2D> ().velocity;
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (playerMoveSpeed * Input.GetAxis("Horizontal"), playerVelocity.y);
		movementSpeed = Mathf.Abs (Input.GetAxis ("Horizontal"));
		playerAnimator.SetFloat ("Speed", movementSpeed);
	}
	private void flipHorizontal(){
		facingRight = !facingRight;
		currentScale.x *= -1;
		transform.localScale = currentScale;
	}

	private void isPlayerDead(){
		if (transform.position.y < fallLimit) {
			SceneManager.LoadScene("Game Over");
		}
	}
}