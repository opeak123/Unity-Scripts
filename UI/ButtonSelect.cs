using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSelect : MonoBehaviour
{
    public Button[] m_buttons;
    public Image m_currSelectedBtn;
    SceneManager m_sceneManager;
    private int m_selectedIndex = 0;


    private void Start()
    {
        m_buttons[0].Select();
    }
    void Update()
    {
        ButtonUpDown();
    }

    private void ButtonUpDown()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_selectedIndex = (m_selectedIndex + 1) % m_buttons.Length;
            m_buttons[m_selectedIndex].Select();
            m_currSelectedBtn.rectTransform.position = m_buttons[m_selectedIndex].gameObject.transform.position;
            SoundManager.Instance.PlaySFX("ui-click2-sfx", .5f);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_selectedIndex = (m_selectedIndex - 1 + m_buttons.Length) % m_buttons.Length;
            m_buttons[m_selectedIndex].Select();
            m_currSelectedBtn.rectTransform.position = m_buttons[m_selectedIndex].gameObject.transform.position;
            SoundManager.Instance.PlaySFX("ui-click2-sfx", .5f);

        }
    }
}
