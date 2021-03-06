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
        SceneManager.LoadScene("Scenes/GameOver");
		//gameOverObject.SetActive (true);
	}

    public void GameWin()
    {
        isGameOver = true;
        SceneManager.LoadScene("Scenes/GameWin");
    }

	public static void Reset()
	{
		//gameOverObject.SetActive (false);
		//interactObject.SetActive (false);
		//isGameOver = false;

		//EnemyLogic.needReset = true;
	}
}
