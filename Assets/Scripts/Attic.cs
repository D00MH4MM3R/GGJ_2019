using UnityEngine;

public class Attic : MonoBehaviour
{
    public MeshRenderer m_renderer;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !m_renderer.enabled)
        {
            m_renderer.enabled = true;
        }
    }
}
