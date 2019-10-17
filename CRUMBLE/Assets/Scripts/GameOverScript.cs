using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	public Button retryButton;
	public Button exitButton;

	// Use this for initialization
	void Start()
	{
		retryButton = retryButton.GetComponent<Button>();
		exitButton = exitButton.GetComponent<Button>();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void RetryLevel()
	{
		SceneManager.LoadScene("Main");
	}
}