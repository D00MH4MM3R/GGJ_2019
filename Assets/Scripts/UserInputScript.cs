using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputScript : MonoBehaviour
{
	public Transform m_startingPos;
	public float m_speed = 0.01f;

	private SpriteRenderer m_spriteR;
	private Animator m_Anim;

	private Collider2D m_doorCollider;
    private Collider2D m_itemCollider;

	private float m_doorDelayTimer = 0.0f;
	public float m_doorDelay = 0.3f;

	private Collider2D currentCollider = null;

	private bool isAbleToHide = false;
	public static bool isHidden = false;

    public ItemType m_HoldingItemType = ItemType.None;


    // Start is called before the first frame update
    void Start()
    {
		transform.position = m_startingPos.position;
		m_spriteR = gameObject.GetComponentInChildren<SpriteRenderer> ();

		m_Anim = GetComponentInChildren<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
		float deltaTime = Time.deltaTime;

		m_doorDelayTimer += deltaTime;

		// update position
		{
			var currentPos = transform.position;
			if (Input.GetButton ("Horizontal") && !isHidden) {
				
				if (Input.GetKey ("d") || Input.GetKey("right")) {
					m_spriteR.flipX = true;
					currentPos.x += m_speed * deltaTime;
				} else if (Input.GetKey ("a") || Input.GetKey("left")) {
					m_spriteR.flipX = false;
					currentPos.x -= m_speed * deltaTime;
				}
			} 
			if (Input.GetButton ("Horizontal")) {
				m_Anim.SetBool ("isWalking", true);
			}
			else if (Input.GetButtonUp ("Horizontal")) {
				m_Anim.SetBool ("isWalking", false);
			}
			transform.position = currentPos;

			if (GameManager.isGameOver && Input.GetKeyDown("space")) {
				GameManager.Reset ();
				Reset ();
			}
		}

		//door
		if (Input.GetKeyDown("w") && m_doorCollider != null && m_doorDelayTimer >= m_doorDelay) {
			m_doorCollider.gameObject.transform.parent.GetComponentInChildren<Animator> ().SetTrigger ("doorEvent");

			if (currentCollider != null) {
				currentCollider.gameObject.SetActive(!currentCollider.gameObject.activeInHierarchy);
			}
			m_doorDelayTimer = 0.0f;
		}

		// hiding
		if (Input.GetKeyDown("w") && isAbleToHide) {
			isHidden = !isHidden;
			m_spriteR.enabled = !isHidden;
		}

        // item
        if (Input.GetKeyDown("r"))
        {

        }
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		switch (col.gameObject.tag) {
		case "Enemy":
			if (!isHidden) {
				GameManager.GameOver ();
			}
			break;
		case "Interact":
			GameManager.interactObject.SetActive (true);
			break;
        case "Item":
            //GameManager.interactObject.SetActive(true);
            //col.gameObject.GetPar
            break;
		case "Door":
			currentCollider = col.GetComponent<GetSillyCollision>().myCollider;
			m_doorCollider = col;
			GameManager.interactObject.SetActive (true);
			break;
		case "Hideable":
			isAbleToHide = true;
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
		case "Hideable":
			isAbleToHide = false;
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
