using UnityEngine;
using System;

public class UserInputScript : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    private Collider2D currentCollider = null;

    private float m_doorDelayTimer = 0.0f;
    private bool isAbleToHide = false;

    private GameObject m_heldItem;

    public Collider2D m_doorCollider;
    public Collider2D m_itemCollider;

    public Transform m_startingPos;
    public float m_speed = 0.01f;
	public float m_doorDelay = 0.3f;
	public static bool isHidden = false;

    public ItemType m_HoldingItemType = ItemType.None;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = m_startingPos.position;
		m_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		m_animator = GetComponentInChildren<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
		float deltaTime = Time.deltaTime;

		m_doorDelayTimer += deltaTime;

        if (!isHidden)
        {
            float horizontalAxis = Input.GetAxis("Horizontal");

            m_animator.SetBool("isWalking", Math.Abs(horizontalAxis) > 0.001);
            m_spriteRenderer.flipX = horizontalAxis > 0;

            transform.Translate(transform.right * deltaTime * horizontalAxis * m_speed);
        }


        if (GameManager.Instance.isGameOver && Input.GetButton("Interact"))
        {
            GameManager.Instance.Reset();
            Reset();
        }

        if (Input.GetButtonDown("Interact") && m_doorCollider != null && m_doorDelayTimer >= m_doorDelay) 
        {
            DoorLock doorLock = m_doorCollider.GetComponent<DoorLock>();
            if (doorLock)
            {
                if (HasKey())
                {
                    UnlockDoor(doorLock);
                }
                else
                {
                    return;
                }
            }

            m_doorCollider.gameObject.transform.parent.GetComponentInChildren<Animator> ().SetTrigger ("doorEvent");

			if (currentCollider != null)
            {
				currentCollider.gameObject.SetActive(!currentCollider.gameObject.activeInHierarchy);
			}
			m_doorDelayTimer = 0.0f;
		}

		// hiding
		if (Input.GetButtonDown("Interact") && isAbleToHide)
        {
			isHidden = !isHidden;
			m_spriteRenderer.enabled = !isHidden;
		}

        // item
        if (Input.GetButtonDown("PickUp") && m_itemCollider != null)
        {
            Item item = m_itemCollider.GetComponent<Item>();

            if (m_heldItem != null)
            {
                m_heldItem.SetActive(true);
                Vector3 temp = transform.position;
                temp.y = temp.y + 5;
                m_heldItem.transform.position = temp;

                m_heldItem = null;
            }

            if (item != null)
            {
                m_HoldingItemType = item.m_type;
                m_heldItem = item.transform.parent.gameObject;
                m_heldItem.SetActive(false);
            }
        }
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		switch (col.gameObject.tag)
        {
		case "Enemy":
			if (!isHidden)
            {
				GameManager.Instance.GameOver();
			}
			break;
		case "Interact":
            InteractionText.SetText("Press SPACE!");
			break;
        case "Item":
            InteractionText.SetText("Press E!");
            m_itemCollider = col;
            break;
		case "Door":
			currentCollider = col.GetComponent<GetSillyCollision>().myCollider;
			m_doorCollider = col;
            InteractionText.SetText("Press SPACE!");
            break;
		case "Hideable":
			isAbleToHide = true;
            InteractionText.SetText("Press SPACE!");
            break;
		default:
			break;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		switch (col.gameObject.tag)
        {
		case "Interact":
			break;
		case "Door":
			m_doorCollider = null;
			break;
		case "Hideable":
			isAbleToHide = false;
			break;
            case "Item":
            m_itemCollider = null;
            break;
		default:
			break;
		}

        InteractionText.SetText("");
    }

	void Reset()
	{
		transform.position = m_startingPos.position;
	}

    bool HasKey()
    {
        return m_HoldingItemType == ItemType.Key;
    }

    bool HasCrowbar()
    {
        return m_HoldingItemType == ItemType.Crowbar;
    }

    void UnlockDoor(DoorLock doorLock)
    {
        Destroy(doorLock);
        Destroy(m_heldItem);
        m_HoldingItemType = ItemType.None;
    }
}
