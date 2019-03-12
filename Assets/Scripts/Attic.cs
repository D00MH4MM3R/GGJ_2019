using UnityEngine;

public class Attic : MonoBehaviour
{
    public MeshRenderer m_renderer;

    private float endWait = 0;

    void Update()
    {
        if(m_renderer.enabled)
        {
            endWait += Time.deltaTime;

            if(endWait>10)
            {
                GameManager.Instance.GameWin();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !m_renderer.enabled)
        {
            m_renderer.enabled = true;
        }
    }
}
