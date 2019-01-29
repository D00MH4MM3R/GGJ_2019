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
        Reset();
    }

	public void GameOver()
	{
		isGameOver = true;
        SceneManager.LoadScene("GameOver");
	}

	public void Reset()
    {
        isGameOver = false;
        EnemyLogic.needReset = true;

        InteractionText.SetText("");
	}
}
