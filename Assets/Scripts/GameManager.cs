using UnityEngine;
using UnityEngine.SceneManagement;

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
		EnemyLogic.needReset = true;
    }

	public void GameOver()
	{
		isGameOver = true;
        SceneManager.LoadScene("GameOver");
	}
}
