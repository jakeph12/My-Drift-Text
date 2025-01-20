using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class TimeAndDriftCounter : MonoBehaviour
{
    public static TimeAndDriftCounter m_staticThis;
    public CancellationTokenSource m_clStatTimer;
    [SerializeField] private bool Multiplayer = false;
    public CancellationTokenSource m_clToken;
    public int m_inMinute = 2, _m_inSecond = 1;
    public int m_inSecond
    {
        get => _m_inSecond;
        set
        {
            _m_inSecond = value;
            if (_m_inSecond <= 0)
            {
                if (m_inMinute > 0)
                {
                    _m_inSecond = 59;
                    m_inMinute--;
                }
                else
                {
                    m_wiGetReward.Open();
                    Debug.Log(Ui_Controller.m_staticThis.m_inCoin);
                    m_wiGetReward.Init(Ui_Controller.m_staticThis.m_inCoin, Main_Car_Controller.m_staticThis.m_ActionCallBack);
                    Main_Car_Controller.m_staticThis.EndGame();
                }
            }
            m_EventOnTimeChange?.Invoke(new Vector2Int(m_inMinute, _m_inSecond));

        }
    }
    public delegate void OnTimeChange(Vector2Int time);
    public event OnTimeChange m_EventOnTimeChange;
    public event PlayerInfo.OnIntChange m_EventOnDrift,m_EventOnDriftEnd;
    public float m_PriviosTime;
    [SerializeField] private Reward_Ui m_wiGetReward;

    private void Awake()
    {
        m_staticThis = this;
    }

    private void Start()
    {
        if (!Multiplayer || PhotonNetwork.IsMasterClient)
        {
            m_clStatTimer = new CancellationTokenSource();
            StartTimer().Forget();
        }
    }


    public async UniTask StartTimer()
    {
        try
        {
            while ((m_inMinute > 0 || m_inSecond > 0) && !m_clStatTimer.IsCancellationRequested)
            {
                m_inSecond--;
                if (Multiplayer)
                {
                    MultiPlayerSkin.m_sinThis.m_phView.RPC("SetTime", RpcTarget.Others, m_inSecond, m_inMinute);
                    Debug.Log("sdasd");

                }
                await UniTask.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        m_clStatTimer.Dispose();
        m_clStatTimer = null;

    }

    public async UniTask SetTimer()
    {
        m_PriviosTime = Time.time;
        try
        {
            while (Main_Car_Controller.m_staticThis.m_bIfOnDrift && !m_clToken.IsCancellationRequested)
            {
                await UniTask.Delay(600);
                float counts = Time.time - m_PriviosTime;
                m_EventOnDrift?.Invoke((int)(counts * 120));

            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }


        if (m_clToken != null)
            m_clToken.Dispose();

        float count = Time.time - m_PriviosTime;
        m_EventOnDriftEnd?.Invoke((int)(count * 120));

        m_clToken = null;

    }
}
