using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;

    private Quaternion m_RelativeRotation;

    // Start is called before the first frame update
    void Start()
    {
        
        m_RelativeRotation = transform.rotation;
        m_RelativeRotation.y = Camera.main.transform.localRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_UseRelativeRotation == true)
        {
            transform.rotation = m_RelativeRotation;
        }
    }
}
