using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour
{
    public static Text m_text;

    public void Start()
    {
        m_text = GetComponent<Text>();
    }

    public static void SetText(string text)
    {
        if (!m_text.text.Equals(text))
        {
            m_text.text = text;
        }
    }
}
