using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Win : MonoBehaviour
{
	public Button exitButton;

	// Use this for initialization
	void Start()
	{
		exitButton = exitButton.GetComponent<Button>();
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}