using UnityEngine;

public class StairsLogic : MonoBehaviour
{
    private bool m_canUse = false;

    public Transform m_connectingStairCase;

    void Update()
    {
        if (m_canUse)
        {
            if (Input.GetButtonDown("Interact"))
            {
                GameManager.Instance.player.transform.position = m_connectingStairCase.position;
                InteractionText.SetText("Press SPACE!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_canUse = true;
            InteractionText.SetText("Press SPACE!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_canUse = false;
            InteractionText.SetText("");
        }
    }
}
