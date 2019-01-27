using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject m_player;
	public static GameObject player;

	public GameObject m_gameOverObject;
	public static GameObject gameOverObject;

	public GameObject m_interactObject;
	public static GameObject interactObject;

	public static bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
		player = m_player;

		gameOverObject = m_gameOverObject;
		gameOverObject.SetActive (false);

		interactObject = m_interactObject;
		interactObject.SetActive (false);

		isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static void GameOver()
	{
		isGameOver = true;
        SceneManager.LoadScene("GameOver");
		//gameOverObject.SetActive (true);
	}

	public static void Reset()
	{
		gameOverObject.SetActive (false);
		interactObject.SetActive (false);
		isGameOver = false;

		EnemyLogic.needReset = true;
	}
}
