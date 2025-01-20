using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiPlayerSkin : MonoBehaviour ,IPunObservable
{
    public static MultiPlayerSkin m_sinThis;
    [SerializeField] private GameObject m_gmSpoiler, m_gmBody;
    public PhotonView m_phView;
    private bool _m_bSpoiler;
    private float m_flR, m_flG, m_flB;
    private TimeAndDriftCounter m_TimeAndDriftCounter;
    public bool m_bSpoiler
    {
        get => _m_bSpoiler;
        set
        {
            _m_bSpoiler = value;
            m_gmSpoiler.SetActive(_m_bSpoiler);
            PlayerPrefs.SetInt($"{gameObject.name} Spoiler", _m_bSpoiler ? 1 : 0);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            var t = 0;
            if(m_bSpoiler)
            {
                t = 1;
            }
            stream.SendNext(m_flR);
            stream.SendNext(m_flG);
            stream.SendNext(m_flB);
            stream.SendNext(m_bSpoiler);
        }
        else
        {
            float R = (float)stream.ReceiveNext();
            float G = (float)stream.ReceiveNext();
            float B = (float)stream.ReceiveNext();
            m_bSpoiler = (bool)stream.ReceiveNext();
            m_gmBody.GetComponent<Renderer>().material.color = new Color(R, G, B);
        }
    }

    private void Awake()
    {
        m_phView = GetComponent<PhotonView>();
        if (!m_phView.IsMine)
        {
            Destroy(gameObject.GetComponent<Main_Car_Controller>());
        }
        if (m_phView.IsMine)
        {
            m_sinThis = this;
        }
    }
    void Start()
    {
        m_TimeAndDriftCounter = TimeAndDriftCounter.m_staticThis;
        if (m_phView.IsMine)
        {
            gameObject.name = gameObject.name.Replace("(Clone)", "");
            var R = PlayerPrefs.GetFloat($"{gameObject.name} R", 1f);
            var G = PlayerPrefs.GetFloat($"{gameObject.name} G", 1f);
            var B = PlayerPrefs.GetFloat($"{gameObject.name} B", 1f);
            SetColor(R, G, B);
        }
    }

    public void SetColor(float R, float G, float B)
    {
        m_flR = R;
        m_flG = G;
        m_flB = B;

        m_gmBody.GetComponent<Renderer>().material.color = new Color(m_flR, m_flG, m_flB);
    }



    public void NotifyAndCloseRoom()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ServerClosing", RpcTarget.All);
    }

    [PunRPC]
    public void ServerClosing()
    {
        Debug.Log("Сервер закрывается. Вас отключают.");
        End();
        PhotonNetwork.Disconnect();
        Screen_Loader.Load_Async_Scene(0);
    }
    [PunRPC]
    public void SetTime(int Sec, int Min)
    {
        Debug.Log($"Серверное Время:{Min}:{Sec}");
        m_TimeAndDriftCounter.m_inMinute = Min;
        m_TimeAndDriftCounter.m_inSecond = Sec;
    }
    public void LeveFromToMainMenu()
    {
        CloseAll();
    }
    public void CloseAll()
    {
        End();

        if (PhotonNetwork.IsMasterClient)
        {
            NotifyAndCloseRoom();
        }
        else
        {
            PhotonNetwork.Disconnect();
            Screen_Loader.Load_Async_Scene(0);
        }

    }
    void End()
    {
        Main_Car_Controller.m_staticThis.EndGame(false);

        if (m_TimeAndDriftCounter.m_clStatTimer != null)
            m_TimeAndDriftCounter.m_clStatTimer.Cancel();

        if (m_TimeAndDriftCounter.m_clToken != null)
            m_TimeAndDriftCounter.m_clToken.Cancel();
        Camera.main.GetComponent<Camera_Follow>().enabled = false;
    }


}
