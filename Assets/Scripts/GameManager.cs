using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public GameObject player;
	public GameObject interactObject;
	public bool isGameOver;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
		isGameOver = false;
    }

	public void GameOver()
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
