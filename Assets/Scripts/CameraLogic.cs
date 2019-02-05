using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
	public Transform m_player;

	private Vector3 m_currentPos;

    // Start is called before the first frame update
    void Start()
    {
		m_currentPos.x = m_player.position.x;
		m_currentPos.y = m_player.position.y;
		m_currentPos.z = transform.position.z;

		transform.position = m_currentPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void LateUpdate()
	{
		m_currentPos.x = m_player.position.x;
		m_currentPos.y = m_player.position.y;

		transform.position = m_currentPos;
	}
}
