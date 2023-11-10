using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    //컬러 값을 조정할 SpriteRenderer
    public SpriteRenderer m_faderenderer;
    //Fade 중인지 체크 
    public bool pb_isFade = false;
    //Fade 시간 값 
    public float fadeTime = 3f;
    void Start()
    {
        //할당
        m_faderenderer = GetComponent<SpriteRenderer>();
    }


    //Fade In
    public IEnumerator FadeIn()
    {
        GameManager.Instance.canMove = false;
        m_faderenderer.sortingOrder = 10;
        Color color = m_faderenderer.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / fadeTime;
            m_faderenderer.color = color;
            yield return null;
        }
        m_faderenderer.sortingOrder = -10;
        GameManager.Instance.canMove = true;
    }


    //Fade Out
    public IEnumerator FadeOut()
    {
        GameManager.Instance.canMove = false;
        m_faderenderer.sortingOrder = 10;
        Color color = m_faderenderer.color;
        while (color.a <= 255)
        {
            color.a += Time.deltaTime / fadeTime;
            m_faderenderer.color = color;
            if(color.a == 180)
            {
                yield return new WaitForSeconds(2f);
            }
            yield return null;
        }
        m_faderenderer.sortingOrder = -10;
        GameManager.Instance.canMove = true;
        yield return StartCoroutine(FadeIn());
    }
}
