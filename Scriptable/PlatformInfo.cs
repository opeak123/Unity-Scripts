using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Platform", menuName = "Information/Platform", order = 0)]
public class PlatformInfo : ScriptableObject
{
    public Vector2[] m_platformPos;
    
    private void OnEnable()
    {
        m_platformPos = new Vector2[4];
    }
}
