using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MirrorTestScript : MonoBehaviour
{
    public Mirror[] m_mirror = new Mirror[5];
    public Mirror m_mirror_target = null;
    public bool t_mirror_test = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            m_mirror[i].enabled = false;
        }
        m_mirror_target = m_mirror[0];
        m_mirror_target.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Comma)){
            t_mirror_test = !t_mirror_test;
        }
        if (t_mirror_test)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                m_mirror_target.enabled = false;
                m_mirror[0].enabled = true;
                m_mirror_target = m_mirror[0];

            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                m_mirror_target.enabled = false;
                m_mirror[1].enabled = true;
                m_mirror_target = m_mirror[1];
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                m_mirror_target.enabled = false;
                m_mirror[2].enabled = true;
                m_mirror_target = m_mirror[2];
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                m_mirror_target.enabled = false;
                m_mirror[3].enabled = true;
                m_mirror_target = m_mirror[3];
            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                m_mirror_target.enabled = false;
                m_mirror[4].enabled = true;
                m_mirror_target = m_mirror[4];
            }
        }
    }
}
