using UnityEngine;
using System;

using UnityEngine.Assertions;

public class UserInputScript : MonoBehaviour
{
	public static bool isHidden = false;

	public float m_speed = 0.01f;
	public float m_doorDelay = 0.3f;

    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    private float m_doorDelayTimer = 0.0f;
    private bool isAbleToHide = false;

    private GameObject m_heldItem;

    public Collider2D m_doorCollider;
    public Collider2D m_itemCollider;
    public Collider2D m_hideCollider;
    public Collider2D m_breakCollider;

    private int m_defaultSortingOrder;

    public ItemType m_HoldingItemType = ItemType.None;

    // Start is called before the first frame update
    void Start()
    {
		m_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		m_defaultSortingOrder = m_spriteRenderer.sortingOrder;

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

        // key + door
		if (Input.GetButtonDown("Interact") && m_doorCollider != null && m_doorDelayTimer >= m_doorDelay) 
        {
            DoorLock doorLock = m_doorCollider.GetComponentInParent<DoorLock>();
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
			m_doorCollider.gameObject.SetActive(!m_doorCollider.gameObject.activeInHierarchy);

			string doorText = m_doorCollider.gameObject.activeInHierarchy ? " open door!" : " close door!";
			InteractionText.SetText(InteractionText.GetDefaultText() + doorText);

			m_doorDelayTimer = 0.0f;
		}

		// hiding
		if (Input.GetButtonDown("Interact") && isAbleToHide)
        {
			isHidden = !isHidden;

			if (isHidden) 
			{
				m_animator.SetBool("isWalking", false);
                InteractionText.SetText("");
                m_spriteRenderer.sortingOrder = 4;
			} 
			else 
			{
				m_spriteRenderer.sortingOrder = m_defaultSortingOrder;
            }

            if (m_hideCollider.GetComponent<Animator>() != null)
            {
                m_hideCollider.GetComponent<Animator>().SetTrigger("hideEvent");
            }
        }

        // item
		if (Input.GetButtonDown("Interact") && m_itemCollider != null)
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

        // smash
        if (Input.GetButtonDown("Interact") && m_breakCollider != null && HasCrowbar())
        {
            Destroy(m_breakCollider.gameObject);
            m_breakCollider = null;
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
        case "Item":
			InteractionText.SetText(InteractionText.GetDefaultText() + " pick up item!");
            m_itemCollider = col;
            break;
		case "Door":
			m_doorCollider = col.GetComponent<GetSillyCollision>().myCollider;
			string doorText = m_doorCollider.gameObject.activeInHierarchy ? " open door!" : " close door!";
			InteractionText.SetText(InteractionText.GetDefaultText() + doorText);
            break;
		case "Hideable":
			isAbleToHide = true;
			InteractionText.SetText(InteractionText.GetDefaultText() + " hide!");
            m_hideCollider = col;
            break;
        case "breakable":
            InteractionText.SetText(InteractionText.GetDefaultText() + " smash!");
            m_breakCollider = col;
            break;
		default:
			break;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		switch (col.gameObject.tag)
        {
		case "Door":
			m_doorCollider = null;
			break;
		case "Hideable":
			isAbleToHide = false;
			break;
        case "Item":
            m_itemCollider = null;
            m_hideCollider = null;
            break;
        case "breakable":
            m_breakCollider = null;
            break;
		default:
			break;
		}

        InteractionText.SetText("");
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
