using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
	public Transform m_startPoint;
	public Transform m_endPoint;
	public float m_patrolSpeed = 0.01f;
	public float m_chaseSpeed = 1.0f;
	public float m_rayCastDistance = 4;

	public static bool needReset;

	private Transform m_currentStartPoint;
	private Transform m_currentEndPoint;

	private float m_startTime;
	private float m_pathDistance;

	private bool m_GoingTowardsEndPoint;

	private SpriteRenderer m_spriteR;

	private Vector3 m_currentFowardVec;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = m_startPoint.position;
		m_pathDistance = Vector2.Distance (m_startPoint.position, m_endPoint.position);

		m_currentStartPoint = m_startPoint;
		m_currentEndPoint = m_endPoint;
		m_startTime = Time.time;
		m_GoingTowardsEndPoint = true;

		needReset = false;

		m_spriteR = gameObject.GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
		m_currentFowardVec = Vector3.Normalize(m_currentEndPoint.position - m_currentStartPoint.position);
		if (!CheckForPlayer () && UserInputScript.isHidden) {
			Patrol ();
		} else {
			Attack ();
		}

		if (needReset) {
			Reset ();
		}
    }

	bool CheckForPlayer()
	{
		LayerMask playerLayerMask = LayerMask.GetMask ("Player");
		RaycastHit2D hit2D = Physics2D.Raycast (transform.position, m_currentFowardVec, m_rayCastDistance, playerLayerMask);
		bool result = hit2D.collider != null;
		return result;
	}

	void Attack()
	{
		Vector3 dir = Vector3.Normalize(GameManager.player.transform.position - transform.position);
		float x = (dir.x > 0) ? 1 : -1; // the x value should only be -1 or 1
		dir.x = x;

		Vector3 currentPos = transform.position;
		currentPos.x += dir.x * m_chaseSpeed;

		transform.position = currentPos;
	}

	void Patrol()
	{
		if (transform.position.x != m_currentEndPoint.position.x) {
			// distance moved = time * speed
			float distCovered = (Time.time - m_startTime) * m_patrolSpeed;

			// percentage of the path completed = distance moved / total distace to move
			float percentPathComplete = distCovered / m_pathDistance;

			transform.position = Vector2.Lerp (m_currentStartPoint.position, m_currentEndPoint.position, percentPathComplete);
		} 
		else {
			if (m_GoingTowardsEndPoint) {
				m_currentStartPoint = m_endPoint;
				m_currentEndPoint = m_startPoint;
				m_startTime = Time.time;
				m_GoingTowardsEndPoint = false;
				m_spriteR.flipX = true;
			} 
			else {
				m_currentStartPoint = m_startPoint;
				m_currentEndPoint = m_endPoint;
				m_startTime = Time.time;
				m_GoingTowardsEndPoint = true;
				m_spriteR.flipX = false;
			}
		}
	}

	void Reset()
	{
		transform.position = m_startPoint.position;

		m_currentStartPoint = m_startPoint;
		m_currentEndPoint = m_endPoint;
		m_startTime = Time.time;
		m_GoingTowardsEndPoint = true;

		needReset = false;
	}
}
