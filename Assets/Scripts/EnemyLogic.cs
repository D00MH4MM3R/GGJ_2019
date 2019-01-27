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

	private float m_pathDistance;

	private bool m_GoingTowardsEndPoint;

	private SpriteRenderer m_spriteR;

	private Vector3 m_currentFowardVec;

	private float m_percentPathComplete = 1.0f;

	enum STATE
	{
		PATROL_STATE,
		ATTACK_STATE,
	};

	private STATE m_currentState = STATE.PATROL_STATE;

    // Start is called before the first frame update
    void Start()
    {
		//transform.position = m_startPoint.position;
		m_pathDistance = Vector2.Distance (m_startPoint.position, m_endPoint.position);

		//m_currentStartPoint = m_startPoint;
		m_currentStartPoint = gameObject.transform;
		m_currentEndPoint = m_endPoint;
		m_GoingTowardsEndPoint = true;

		needReset = false;

		m_spriteR = gameObject.GetComponentInChildren<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
		m_currentFowardVec = Vector3.Normalize(m_currentEndPoint.position - m_currentStartPoint.position);
		if (!CheckForPlayer ()) {
			if (m_currentState == STATE.ATTACK_STATE) {
				m_currentStartPoint = gameObject.transform;
				m_pathDistance = Vector2.Distance (m_currentStartPoint.position, m_currentEndPoint.position);
			}
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
		if (UserInputScript.isHidden) {
			return false;
		}

		RaycastHit2D[] hits;
		hits = Physics2D.RaycastAll(transform.position, m_currentFowardVec, m_rayCastDistance);

		List<Collider2D> hitColliders = new List<Collider2D> ();;
		for (int i = 0; i < hits.Length; i++) {
			RaycastHit2D hit = hits [i];
			if (hit.collider != null && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger) {
				hitColliders.Add (hit.collider);
			}
		}

		hitColliders.Sort ((a, b) =>
			Vector2.Distance (transform.position, a.gameObject.transform.position).
			CompareTo (Vector2.Distance (transform.position, b.gameObject.transform.position)));

		if (hitColliders.Count > 0) {
			string tag = hitColliders[0].gameObject.tag;
			if (tag == "Player") {
				return true;
			} else if (tag == "Door") {
				if (m_GoingTowardsEndPoint) {
					m_currentStartPoint = hitColliders[0].gameObject.transform;
					m_currentEndPoint = m_startPoint;
					m_GoingTowardsEndPoint = false;
					m_spriteR.flipX = true;
				} 
				else {
					m_currentStartPoint = hitColliders[0].gameObject.transform;
					m_currentEndPoint = m_endPoint;
					m_GoingTowardsEndPoint = true;
					m_spriteR.flipX = false;
				}
			}
		}

		return false;
	}

	void Attack()
	{
		m_currentState = STATE.ATTACK_STATE;

		Vector3 dir = Vector3.Normalize(GameManager.player.transform.position - transform.position);
		float x = (dir.x > 0) ? 1 : -1; // the x value should only be -1 or 1
		dir.x = x;

		Vector3 currentPos = transform.position;
		currentPos.x += (dir.x * m_chaseSpeed) * Time.deltaTime;

		transform.position = currentPos;
	}

	void Patrol()
	{
		m_currentState = STATE.PATROL_STATE;

		if(m_percentPathComplete > 0.1f){
			Vector3 dir = Vector3.Normalize(m_currentEndPoint.position - m_currentStartPoint.position);
			Vector3 currentPos = transform.position;
			currentPos.x += Time.deltaTime * (m_patrolSpeed * dir.x);

			float dis = Vector2.Distance (currentPos, m_currentEndPoint.position);

			// percentage of the path completed = distance moved / total distace to move
			m_percentPathComplete = dis / m_pathDistance;

			transform.position = currentPos;
		} 
		else {
			m_percentPathComplete = 1.0f;
			if (m_GoingTowardsEndPoint) {
				m_currentStartPoint = m_endPoint;
				m_currentEndPoint = m_startPoint;
				m_GoingTowardsEndPoint = false;
				m_spriteR.flipX = true;
			} 
			else {
				m_currentStartPoint = m_startPoint;
				m_currentEndPoint = m_endPoint;
				m_GoingTowardsEndPoint = true;
				m_spriteR.flipX = false;
			}
		}
	}

	void Reset()
	{
		m_currentState = STATE.PATROL_STATE;

		transform.position = m_startPoint.position;

		m_currentStartPoint = m_startPoint;
		m_currentEndPoint = m_endPoint;
		m_GoingTowardsEndPoint = true;

		needReset = false;
	}
}
