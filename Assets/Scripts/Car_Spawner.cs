using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void Close()
    {
        Main_Car_Controller.m_sinThis.EndGame(false);

        if (Ui_Controller.m_sinThis.m_clStatTimer != null)
            Ui_Controller.m_sinThis.m_clStatTimer.Cancel();

        if (Ui_Controller.m_sinThis.m_clToken != null)
            Ui_Controller.m_sinThis.m_clToken.Cancel();
        Camera.main.GetComponent<Camera_Follow>().enabled = false;
        SceneManager.LoadScene(0);
    }
}
