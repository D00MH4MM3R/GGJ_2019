using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsLogic : MonoBehaviour
{
	public Transform m_connectingStairCase;

	public bool m_localPlayerJustEnteredStairs;
	public static bool m_globalPlayerJustEnteredStairs;

    // Start is called before the first frame update
    void Start()
    {
		m_globalPlayerJustEnteredStairs = false;
    }

    // Update is called once per frame
    void Update()
    {
		if(m_localPlayerJustEnteredStairs && Input.GetKeyDown("w"))
		{
			m_localPlayerJustEnteredStairs = false;
			GameManager.player.transform.position = m_connectingStairCase.position;
		}
        
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			if (!m_globalPlayerJustEnteredStairs) {
				GameManager.interactObject.SetActive (true);
				//GameManager.player.transform.position = m_connectingStairCase.position;
				m_localPlayerJustEnteredStairs = true;
				m_globalPlayerJustEnteredStairs = true;
			} 
			else {
				m_globalPlayerJustEnteredStairs = false;
			}
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			GameManager.interactObject.SetActive (false);
			m_localPlayerJustEnteredStairs = false;
		}
	}
}
