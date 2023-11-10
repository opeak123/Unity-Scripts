using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMove : MonoBehaviour
{
    public Button[] m_buttons;
    public Image m_currSelectedBtn;

    public Image m_currImage;
    public Sprite[] m_images;
    [SerializeField]
    private int m_selectedIndex = 0;


    private void Start()
    {
        m_buttons[0].Select();
    }

    private void Update()
    {
        CurrentButton();
    }

    private void CurrentButton()
    {
        if (SceneManager.FindObjectOfType<SceneManager>().m_currSceneNumber == 1)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_selectedIndex = (m_selectedIndex + 1) % m_buttons.Length;
                m_buttons[m_selectedIndex].Select();
                OnSelectImage();
                m_currSelectedBtn.rectTransform.position = m_buttons[m_selectedIndex].gameObject.transform.position;
                SoundManager.Instance.PlaySFX("ui-click2-sfx", .5f);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_selectedIndex = (m_selectedIndex - 1 + m_buttons.Length) % m_buttons.Length;
                m_buttons[m_selectedIndex].Select();
                OnSelectImage();
                m_currSelectedBtn.rectTransform.position = m_buttons[m_selectedIndex].gameObject.transform.position;
                SoundManager.Instance.PlaySFX("ui-click2-sfx", .5f);

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                m_buttons[m_selectedIndex].onClick.Invoke();
            }
        }
    }

    private void OnSelectImage()
    {
        if(m_selectedIndex == 0)
        {
            m_currImage.sprite = m_images[0];
        }

        else if (m_selectedIndex == 1)
        {
            m_currImage.sprite = m_images[1];
        }

        else if (m_selectedIndex == 2)
        {
            m_currImage.sprite = m_images[2];
        }

        else if (m_selectedIndex == 3)
        {
            m_currImage.sprite = m_images[3];
        }
    }

}
