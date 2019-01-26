using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputScript : MonoBehaviour
{
	public Transform m_startingPos;
	public float m_speed = 0.01f;

	private SpriteRenderer m_spriteR;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = m_startingPos.position;
		m_spriteR = gameObject.GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
		// update position
		{
			var currentPos = transform.position;
			if (Input.GetButton ("Horizontal")) {
				if (Input.GetKey ("d")) {
					m_spriteR.flipX = false;
					currentPos.x += m_speed;
				} else if (Input.GetKey ("a")) {
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
		default:
			break;
		}
	}

	void Reset ()
	{
		transform.position = m_startingPos.position;
	}
}
