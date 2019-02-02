using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour
{
    public static Text m_text;

	private static string m_defaultText;

    public void Start()
    {
        m_text = GetComponent<Text>();
		m_defaultText = m_text.text;
    }

    public static void SetText(string text)
    {
        if (!m_text.text.Equals(text))
        {
            m_text.text = text;
        }
    }

	public static string GetDefaultText()
	{
		return m_defaultText;
	}
}
