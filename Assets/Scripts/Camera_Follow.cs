using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public GameObject m_gmTarget;
    [SerializeField] private float m_flLerpPower = 0.6f;
    private Vector3 m_vcOffset;
    void Start()
    {
       // m_vcOffset = m_gmTarget.transform.position - transform.position;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (m_gmTarget != null)
        {
            Vector3 po = m_gmTarget.transform.position - m_vcOffset;
            transform.position = Vector3.Lerp(transform.position, po, m_flLerpPower);

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, m_gmTarget.transform.rotation, m_flLerpPower);
    }
}
