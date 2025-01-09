using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_gmPrefab;
    [SerializeField] private Transform m_gmStartPos;
    [SerializeField] private Ui_Controller m_ctUi;
    [SerializeField] private Camera_Follow m_cmMain;
    // Start is called before the first frame update
    void Start()
    {
        var n =Instantiate(m_gmPrefab[PlayerInfo.m_inId]);
        n.transform.position = m_gmStartPos.position;
        n.GetComponent<Main_Car_Controller>().enabled = true;
        m_cmMain.m_gmTarget = n.transform.GetChild(0).gameObject;
        m_cmMain.enabled = true;
        m_ctUi.enabled = true;
        n.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
