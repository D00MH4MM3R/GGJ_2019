using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
	public Transform m_player;
    public float m_size = 16;
    public float m_xOffset = 0.0f;
    public float m_yOffset = 0.0f;

    private Camera cam;
    private Vector3 m_constructedPos;
    private Vector3 m_currentPos;


    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();

		m_currentPos.x = m_player.position.x;
		m_currentPos.y = m_player.position.y;
		m_currentPos.z = transform.position.z;

        m_constructedPos = new Vector3(m_currentPos.x + m_xOffset, m_currentPos.y + m_yOffset, m_currentPos.z);

        transform.position = m_constructedPos;
        cam.orthographicSize = m_size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void LateUpdate()
	{
		m_currentPos.x = m_player.position.x;
		m_currentPos.y = m_player.position.y;

        m_constructedPos = new Vector3(m_currentPos.x + m_xOffset, m_currentPos.y + m_yOffset, m_currentPos.z);
        transform.position = m_constructedPos;
    }
}
