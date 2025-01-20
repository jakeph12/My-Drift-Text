using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_CarsPrefab;
    [SerializeField] private Transform m_StartPosition;
    [SerializeField] private Ui_Controller m_UiController;
    [SerializeField] private Camera_Follow m_MainCameraScript;
    private TimeAndDriftCounter m_TimeAdnDriftCounter;

    void Start()
    {
        m_TimeAdnDriftCounter = TimeAndDriftCounter.m_staticThis;
        var n = Instantiate(m_CarsPrefab[PlayerInfo.m_IntCurrentId]);
        n.transform.position = m_StartPosition.position;
        n.GetComponent<Main_Car_Controller>().enabled = true;
        m_MainCameraScript.m_gmTarget = n.transform.GetChild(0).gameObject;
        m_MainCameraScript.enabled = true;
        m_UiController.enabled = true;
        n.SetActive(true);
    }

    public void Close()
    {
        Main_Car_Controller.m_staticThis.EndGame(false);

        if (m_TimeAdnDriftCounter.m_clStatTimer != null)
            m_TimeAdnDriftCounter.m_clStatTimer.Cancel();

        if (m_TimeAdnDriftCounter.m_clToken != null)
            m_TimeAdnDriftCounter.m_clToken.Cancel();
        Camera.main.GetComponent<Camera_Follow>().enabled = false;
        Screen_Loader.Load_Async_Scene(0);
    }
}
