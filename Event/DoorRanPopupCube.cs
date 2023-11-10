using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DoorRanPopupCube : MonoBehaviour
{
    public GameObject cubes;
    private Animator m_doorAni;
    private Transform m_transform;
    private int m_randomIndex;
    //private bool m_bmake = false;

    void Start()
    {
        m_transform = transform;
        m_doorAni = GetComponent<Animator>();

        m_randomIndex = Random.Range(1, 5);
        StartCoroutine(DoorDelay(m_randomIndex));
    }

    IEnumerator DoorDelay(int delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (m_doorAni.GetCurrentAnimatorStateInfo(0).IsName("open"))
            {
                this.m_doorAni.speed = 0f;
                yield return new WaitForSeconds(m_randomIndex);   // 1 ~ 5 sec

                if (m_doorAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                {
                    GameObject go = Instantiate(cubes, new Vector2(transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
                    go.transform.parent = transform;
                }
                this.m_doorAni.speed = 1f;
                yield return new WaitForSeconds(m_randomIndex);     // 1 ~ 5 sec
                m_randomIndex = Random.Range(1, 5);
            }
        }

    }
}
