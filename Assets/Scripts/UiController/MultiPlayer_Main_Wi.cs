using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayer_Main_Wi : WindowUi
{
    [SerializeField]private InputField m_inNameL,m_inNameP;
    [SerializeField]private GameObject[] m_gmPrebsCar;
    [SerializeField] private Transform m_trStarPos;
    [SerializeField] private TimeAndDriftCounter m_TimeAndDrift;
    public void Join()
    {
        if (m_inNameL.text == "" || m_inNameP.text == "") return;
        MultiPlayer_Main.m_sinThis.CreteOrJoin(m_inNameL.text, m_inNameP.text,() =>
        {
            float x = Random.Range(m_trStarPos.position.x - 20, m_trStarPos.position.x + 20);
            float z = Random.Range(m_trStarPos.position.z - 20, m_trStarPos.position.z + 20);
            var n = PhotonNetwork.Instantiate($"Cars/{m_gmPrebsCar[PlayerInfo.m_IntCurrentId].name}",new Vector3(x,m_trStarPos.position.y,z),Quaternion.identity);
            if (n.GetPhotonView().IsMine)
            {
                n.GetComponent<Main_Car_Controller>().enabled = true;
                var t = Camera.main.GetComponent<Camera_Follow>();
                t.m_gmTarget = n.transform.GetChild(0).gameObject;

                t.enabled = true;
                m_TimeAndDrift.enabled = true;
                Ui_Controller.m_staticThis.enabled = true;

                DellObj(1);
            }        
        });
    }
}
