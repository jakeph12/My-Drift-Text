using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ui_Controller : MonoBehaviour
{
    public CancellationTokenSource m_clToken;
    public Text m_txMainScore, m_txMainTimer;
    public Text m_txTotalScore;
    private int _m_inCoin;
    public int m_inCoin
    {
        get => _m_inCoin;
        set
        {
            _m_inCoin = value;
            if(m_txTotalScore)
                m_txTotalScore.gameObject.SetActive(true);
            m_txTotalScore.text = $"Total score: {_m_inCoin}";
        }
    }
    public CancellationTokenSource m_clStatTimer;
    public GameObject m_gmSpeed;
    [SerializeField]private bool Multiplayer = false;


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
                    Main_Car_Controller.m_sinThis.EndGame();
                }
            }
            if (_m_inSecond >= 10)
                m_txMainTimer.text = $"Time: {m_inMinute}:{_m_inSecond}";
            else
                m_txMainTimer.text = $"Time: {m_inMinute}:0{_m_inSecond}";

        }
    }
    public static Ui_Controller m_sinThis;


    private void Awake()
    {
        m_sinThis = this;
    }

    private void Start()
    {

        m_clStatTimer = new CancellationTokenSource();
        StartTimer().Forget();
    }


    public async UniTask StartTimer()
    {
        try
        {
            while ((m_inMinute > 0 || m_inSecond > 0) && !m_clStatTimer.IsCancellationRequested)
            {
                m_inSecond--;
                MultiPlayerSkin.m_sinThis.m_phView.RPC("SetTime", RpcTarget.Others, m_inSecond, m_inMinute);
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

        try
        {
            m_txMainScore.gameObject.SetActive(true);
            m_txMainScore.text = $"Your Score:0";
            while (Main_Car_Controller.m_sinThis.m_bDrift && !m_clToken.IsCancellationRequested)
            {
                await UniTask.Delay(600);
                float counts = Time.time - Main_Car_Controller.m_sinThis.m_flPrivS;
                if ((int)counts % 2 == 0)
                {
                    m_txMainScore.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f).OnComplete(() =>
                    {
                        m_txMainScore.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
                    });
                }
                m_txMainScore.text = $"Your Score:{(int)(counts * 120)}";

            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }


        if (m_clToken != null)
            m_clToken.Dispose();

        float count = Time.time - Main_Car_Controller.m_sinThis.m_flPrivS;
        m_inCoin += (int)(count * 120);

        m_clToken = null;

        if (m_txMainScore != null)
            m_txMainScore.gameObject.SetActive(false);

    }


}
