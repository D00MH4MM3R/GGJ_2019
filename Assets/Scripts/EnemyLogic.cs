using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
	public float m_patrolSpeed = 5.0f;		// Speed enemy patrols at
	public float m_chaseSpeed = 8.0f;		// Speed enemy chases player
	public float m_rayCastDisChase = 25.0f;	// Enemy's line of sight distance for begining and maintaining chase on player
	public float m_rayCastDisPatrol = 5.0f;	// Enemy's line of sight distance for patroling (distance to detected Wall or Door before enemy turns)
	public Vector2 m_fowardVec;

	private SpriteRenderer m_spriteR;

    // Start is called before the first frame update
    void Start()
    {
		m_spriteR = gameObject.GetComponentInChildren<SpriteRenderer> ();
		m_spriteR.flipX = m_fowardVec.x > 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (!AbleToSeePlayer ()) 
		{
			Patrol ();
		}
		else 
		{
			Attack ();
		}
    }

	bool AbleToSeePlayer()
	{
		if (UserInputScript.isHidden) {
			return false;
		}
	
		string[] tags = { "Player" };
		return CastRayObjTags (tags, m_rayCastDisChase);
	}

	void Attack()
	{
		Vector3 dir = Vector3.Normalize(GameManager.Instance.player.transform.position - transform.position);
		float x = (dir.x > 0) ? 1 : -1; // the x value should only be -1 or 1
		dir.x = x;

		Vector3 currentPos = transform.position;
		currentPos.x += (dir.x * m_chaseSpeed) * Time.deltaTime;

		transform.position = currentPos;
	}

	void Patrol()
	{
		string[] tags = { "Door", "Wall" };
		if (CastRayObjTags (tags, m_rayCastDisPatrol)) 
		{
			m_fowardVec.x *= -1;
			m_spriteR.flipX = !m_spriteR.flipX;
		}

		Vector3 currentPos = transform.position;
		currentPos.x += Time.deltaTime * (m_patrolSpeed * m_fowardVec.x);

		transform.position = currentPos;
	}

	bool CastRayObjTags(string[] tags, float rayCastDistance)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, m_fowardVec, rayCastDistance);
		List<Collider2D> hitColliders = new List<Collider2D> ();

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit2D hit = hits [i];
			if (hit.collider != null && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger) {
				hitColliders.Add (hit.collider);
			}
		}

		hitColliders.Sort ((a, b) =>
			Vector2.Distance (transform.position, a.gameObject.transform.position).
			CompareTo (Vector2.Distance (transform.position, b.gameObject.transform.position)));

		if (hitColliders.Count > 0) 
		{
			string tag = hitColliders [0].gameObject.tag;
			for (int t = 0; t < tags.Length; ++t) 
			{
				if(tags[t] == tag)
				{
					return true;
				}
			}
		}
		return false;
	}
}