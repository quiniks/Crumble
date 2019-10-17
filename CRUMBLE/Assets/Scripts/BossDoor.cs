using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossDoor : MonoBehaviour {

	[SerializeField]
	private LayerMask player;

	private bool doorActivated;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		doorActivated = Physics2D.IsTouchingLayers (this.gameObject.GetComponent<BoxCollider2D> (), player);
		if (doorActivated == true) {
			SceneManager.LoadScene("Boss");
		}
	}
}
