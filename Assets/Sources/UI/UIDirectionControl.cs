using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;

    private Quaternion m_RelativeRotation;

    // Start is called before the first frame update
    private void Start()
    {
        m_RelativeRotation = transform.rotation;
        m_RelativeRotation.y = Camera.main.transform.localRotation.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_UseRelativeRotation == true)
        {
            m_RelativeRotation.x = Camera.main.transform.localRotation.x;

            transform.rotation = m_RelativeRotation;
        }
    }
}
