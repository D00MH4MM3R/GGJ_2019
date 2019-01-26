using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsLogic : MonoBehaviour
{
	public Transform m_connectingStairCase;

	public static bool m_globalPlayerJustEnteredStairs;

    // Start is called before the first frame update
    void Start()
    {
		m_globalPlayerJustEnteredStairs = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			if (!m_globalPlayerJustEnteredStairs) {
				m_globalPlayerJustEnteredStairs = true;
				GameManager.player.transform.position = m_connectingStairCase.position;
			} 
			else {
				m_globalPlayerJustEnteredStairs = false;
			}
		}
	}
}
