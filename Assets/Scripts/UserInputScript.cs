using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputScript : MonoBehaviour
{
	public Transform m_startingPos;
	public float m_speed = 0.01f;

	private SpriteRenderer m_spriteR;

	private Collider2D m_doorCollider;

	private float m_doorDelayTimer = 0.0f;
	public float m_doorDelay = 0.3f;

	private Collider2D currentCollider = null;


    // Start is called before the first frame update
    void Start()
    {
		transform.position = m_startingPos.position;
		m_spriteR = gameObject.GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
		float deltaTime = Time.deltaTime;

		m_doorDelayTimer += deltaTime;

		// update position
		{
			var currentPos = transform.position;
			if (Input.GetButton ("Horizontal")) {
				if (Input.GetKey ("d") || Input.GetKey("right")) {
					m_spriteR.flipX = false;
					currentPos.x += m_speed;
				} else if (Input.GetKey ("a") || Input.GetKey("left")) {
					m_spriteR.flipX = true;
					currentPos.x -= m_speed;
				}
			} 
			transform.position = currentPos;

			if (GameManager.isGameOver && Input.GetKeyDown("space")) {
				GameManager.Reset ();
				Reset ();
			}
		}

		if (Input.GetKeyDown("e") && m_doorCollider != null && m_doorDelayTimer >= m_doorDelay) {
			m_doorCollider.gameObject.transform.parent.GetComponentInChildren<Animator> ().SetTrigger ("doorEvent");

			if (currentCollider != null) {
				currentCollider.gameObject.SetActive(!currentCollider.gameObject.activeInHierarchy);
			}
			m_doorDelayTimer = 0.0f;
		}
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		switch (col.gameObject.tag) {
		case "Enemy":
			GameManager.GameOver();
			break;
		case "Interact":
			GameManager.interactObject.SetActive (true);
			break;
		case "Door":
			currentCollider = col.GetComponent<GetSillyCollision>().myCollider;
			m_doorCollider = col;
			GameManager.interactObject.SetActive (true);
			break;
		default:
			break;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		switch (col.gameObject.tag) {
		case "Interact":
			GameManager.interactObject.SetActive (false);
			break;
		case "Door":
			m_doorCollider = null;
			GameManager.interactObject.SetActive (false);
			break;
		default:
			break;
		}
	}

	void Reset ()
	{
		transform.position = m_startingPos.position;
	}
}
