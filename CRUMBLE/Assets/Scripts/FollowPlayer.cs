using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform target;//set target from inspector instead of looking in Update
    [SerializeField]
    private float speed = 3.0f;
	[SerializeField]
	private float trackingDistance = 10.0f;

	private Animator enemyAnimator;

    void Start()
    {
		enemyAnimator = GetComponent<Animator> ();
    }

    void Update()
    {
      
		if (Mathf.Abs (transform.position.x - target.position.x) < trackingDistance) {
			enemyAnimator.SetBool ("isMoving", true);
			if (transform.position.x > target.position.x + 0.5f) {
				transform.localEulerAngles = new Vector3 (0, +180, 0);//correcting the original rotation
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);	            //transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
			} else if (transform.position.x < target.position.x - 0.5f) {
				transform.localEulerAngles = new Vector3 (0, 0, 0);//correcting the original rotation
				//transform.position += new Vector3(speed* Time.deltaTime, 0, 0);
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);	
			}
		} else {
			enemyAnimator.SetBool ("isMoving", false);
		}
    }

}